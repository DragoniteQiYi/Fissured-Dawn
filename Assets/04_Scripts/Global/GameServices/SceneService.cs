using Cysharp.Threading.Tasks;
using FissuredDawn.Data.Configs;
using FissuredDawn.Global.Interfaces.GameServices;
using FissuredDawn.Shared.Constants;
using FissuredDawn.Toolkits.Utilities;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace FissuredDawn.Global.GameServices
{
    public class SceneService : ISceneService
    {
        private Dictionary<string, SceneConfig> _sceneConfigs;
        private bool _isInitialized = false;
        private readonly HashSet<string> _loadedScenes = new();
        private string _currentSceneId;
        private AsyncOperationHandle<SceneInstance> _currentSceneHandle;

        public bool IsInitialized => _isInitialized;

        public event Action OnInitialized;
        public event Action<SceneConfig> OnSceneLoaded;

        public SceneService()
        {
            _sceneConfigs = new Dictionary<string, SceneConfig>();
        }

        public async UniTask InitializeAsync()
        {
            if (_isInitialized)
            {
                Debug.LogWarning("[SceneService]: 场景服务已经初始化过");
                return;
            }

            try
            {
                _sceneConfigs = await JsonHelper.LoadAsync<Dictionary<string, SceneConfig>>
                    (ConfigKey.SCENE_CONFIG);

                _isInitialized = true;
                OnInitialized?.Invoke();
                Debug.Log($"[SceneService]: 场景服务初始化完成，共加载 {_sceneConfigs.Count} 个场景配置");
            }
            catch (Exception ex)
            {
                Debug.LogError($"[SceneService]: 场景服务初始化失败: {ex.Message}");
                throw;
            }
        }

        public async UniTask LoadSceneAsync(string sceneId, CancellationToken cancellationToken = default)
        {
            await LoadSceneAsync(sceneId, null, cancellationToken);
        }

        public async UniTask LoadSceneAsync(string sceneId, IProgress<float> progress,
            CancellationToken cancellationToken = default)
        {
            if (!_isInitialized)
            {
                throw new InvalidOperationException(
                    "[SceneService]: 场景服务未初始化，请先调用 InitializeAsync");
            }

            if (!SceneExists(sceneId))
            {
                throw new ArgumentException($"[SceneService]: 场景ID '{sceneId}' 不存在");
            }

            AsyncOperationHandle<SceneInstance>? sceneHandle = null;

            try
            {
                var config = GetSceneConfig(sceneId);

                Debug.Log($"[SceneService]: 开始通过Addressable加载场景 {sceneId}");

                // 如果有当前场景，先卸载
                if (!string.IsNullOrEmpty(_currentSceneId) && _currentSceneId != sceneId)
                {
                    await UnloadCurrentSceneAsync();
                }

                var loadSceneMode = LoadSceneMode.Single;
                var loadParams = new LoadSceneParameters(loadSceneMode);

                // 使用Addressable加载场景
                sceneHandle = Addressables.LoadSceneAsync(
                    sceneId,
                    loadParams,
                    activateOnLoad: true
                );

                // 进度回调
                if (progress != null)
                {
                    while (!sceneHandle.Value.IsDone)
                    {
                        if (cancellationToken.IsCancellationRequested)
                        {
                            if (sceneHandle.HasValue)
                            {
                                Addressables.Release(sceneHandle.Value);
                            }
                            cancellationToken.ThrowIfCancellationRequested();
                        }

                        progress.Report(sceneHandle.Value.PercentComplete);
                        await UniTask.Yield();
                    }
                }
                else
                {
                    await sceneHandle.Value.WithCancellation(cancellationToken);
                }

                // 等待场景完全激活
                if (sceneHandle.Value.Result.Scene.isLoaded)
                {
                    sceneHandle.Value.Result.ActivateAsync().completed += (op) =>
                    {
                        Debug.Log($"[SceneService]: 场景 {sceneId} 已激活");
                    };

                    await UniTask.WaitUntil(() => sceneHandle.Value.Result.Scene.isLoaded);
                }

                _currentSceneId = sceneId;
                _currentSceneHandle = sceneHandle.Value;
                _loadedScenes.Add(sceneId);

                OnSceneLoaded?.Invoke(config);

                Debug.Log($"[SceneService]: 场景 {sceneId} 加载完成");
            }
            catch (OperationCanceledException)
            {
                Debug.Log($"[SceneService]: 场景 {sceneId} 加载被取消");

                if (sceneHandle.HasValue)
                {
                    Addressables.Release(sceneHandle.Value);
                }

                throw;
            }
            catch (Exception ex)
            {
                Debug.LogError($"[SceneService]: 加载场景 {sceneId} 失败: {ex.Message}");

                if (sceneHandle.HasValue)
                {
                    Addressables.Release(sceneHandle.Value);
                }

                throw;
            }
        }

        public SceneConfig GetSceneConfig(string sceneId)
        {
            if (_sceneConfigs.TryGetValue(sceneId, out var config))
            {
                Debug.Log($"[SceneService]: 获取场景配置：" +
                    $"{sceneId}, {config.Name}, {config.SceneType}");
                return config;
            }

            Debug.LogError($"[SceneService]: 未找到场景配置: {sceneId}");
            return null;
        }

        public bool SceneExists(string sceneId)
        {
            if (_sceneConfigs == null)
            {
                Debug.LogError($"[SceneService] _sceneConfigs 是 null!");
                return false;
            }

            if (string.IsNullOrEmpty(sceneId))
            {
                Debug.LogWarning($"[SceneService] 场景ID为空");
                return false;
            }

            Debug.Log($"[SceneService] 检查场景ID: '{sceneId}'");
            Debug.Log($"[SceneService] 配置数量: {_sceneConfigs.Count}");

            bool exists = _sceneConfigs.ContainsKey(sceneId);
            Debug.Log($"[SceneService] 存在: {exists}");

            if (!exists)
            {
                Debug.Log($"[SceneService] 可用ID: {string.Join(", ", _sceneConfigs.Keys)}");
            }

            return exists;
        }

        public async UniTask UnloadSceneAsync(string sceneId)
        {
            if (!_loadedScenes.Contains(sceneId))
            {
                Debug.LogWarning($"[SceneService]: 场景 {sceneId} 未加载，无需卸载");
                return;
            }

            try
            {
                var config = GetSceneConfig(sceneId);
                string sceneName = config.Name;

                var unloadOperation = SceneManager.UnloadSceneAsync(sceneName);
                if (unloadOperation != null)
                {
                    await unloadOperation;
                }

                _loadedScenes.Remove(sceneId);
                if (_currentSceneId == sceneId)
                {
                    _currentSceneId = null;
                }

                await GarbageCollectAsync();

                Debug.Log($"[SceneService]: 场景 {sceneId} 卸载完成");
            }
            catch (Exception ex)
            {
                Debug.LogError($"[SceneService]: 卸载场景 {sceneId} 失败: {ex.Message}");
                throw;
            }
        }

        public string GetCurrentSceneId()
        {
            return _currentSceneId;
        }

        #region 私有方法
        private async UniTask UnloadCurrentSceneAsync()
        {
            if (_currentSceneHandle.IsValid())
            {
                Debug.Log($"[SceneService]: 开始卸载场景 {_currentSceneId}");

                await Addressables.UnloadSceneAsync(
                    _currentSceneHandle,
                    autoReleaseHandle: true
                ).Task.AsUniTask();

                Debug.Log($"[SceneService]: 场景 {_currentSceneId} 卸载完成");
            }

            if (!string.IsNullOrEmpty(_currentSceneId))
            {
                _loadedScenes.Remove(_currentSceneId);
            }

            _currentSceneId = null;
            _currentSceneHandle = default;
        }

        private async UniTask GarbageCollectAsync()
        {
            await UniTask.Delay(100);
            GC.Collect();
            await Resources.UnloadUnusedAssets();
        }
        #endregion
    }
}