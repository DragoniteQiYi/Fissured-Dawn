using Cysharp.Threading.Tasks;
using FissuredDawn.Data.Configs;
using FissuredDawn.Global.Interfaces.GameServices;
using FissuredDawn.Shared.Constants;
using FissuredDawn.Shared.Enums.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace FissuredDawn.Global.GameServices
{
    public class SceneLoader : ISceneLoader
    {
        private readonly Dictionary<SceneTypeEnum, LoadSceneMode> loadTypes = new()
        {
            {SceneTypeEnum.Technic, LoadSceneMode.Single },
            {SceneTypeEnum.Battle, LoadSceneMode.Single },
            {SceneTypeEnum.Field, LoadSceneMode.Single },
            {SceneTypeEnum.Town, LoadSceneMode.Single },
            {SceneTypeEnum.Indoor, LoadSceneMode.Additive },
            {SceneTypeEnum.Dungeon, LoadSceneMode.Single },
            {SceneTypeEnum.MiniGame, LoadSceneMode.Additive },
        };

        private Dictionary<string, SceneConfig> _sceneConfigs;
        private bool _isInitialized = false;
        private HashSet<string> _loadedScenes = new();
        private string _currentSceneId;
        private AsyncOperationHandle<SceneInstance> _currentSceneHandle;

        public bool IsInitialized { get => _isInitialized; }
        public event Action OnInitialized;

        public async UniTask InitializeAsync()
        {
            if (_isInitialized)
            {
                Debug.LogWarning("[SceneLoader]: 场景加载器已经初始化过");
                return;
            }

            try
            {
                string configPath = ConfigPath.SceneConfigPath;

                _sceneConfigs = new Dictionary<string, SceneConfig>();

                await LoadSceneConfigsAsync(configPath);

                _isInitialized = true;
                OnInitialized?.Invoke();
                Debug.Log($"[SceneLoader]: 场景加载器初始化完成，共加载 {_sceneConfigs.Count} 个场景配置");
            }
            catch (Exception ex)
            {
                Debug.LogError($"[SceneLoader]: 场景加载器初始化失败: {ex.Message}");
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
                    "[SceneLoader]: 场景加载器未初始化，请先调用 InitializeAsync");
            }

            if (!SceneExists(sceneId))
            {
                throw new ArgumentException($"[SceneLoader]: 场景ID '{sceneId}' 不存在");
            }
            AsyncOperationHandle<SceneInstance>? sceneHandle = null;

            try
            {
                var config = GetSceneConfig(sceneId);

                Debug.Log($"[SceneLoader]: 开始通过Addressable加载场景 {sceneId}");

                // 如果有当前场景，先卸载
                if (!string.IsNullOrEmpty(_currentSceneId) && _currentSceneId != sceneId)
                {
                    await UnloadCurrentSceneAsync();
                }

                // 通过Addressable加载场景
                var loadSceneMode = loadTypes[config.SceneType];

                var loadParams = new LoadSceneParameters(loadSceneMode);

                // 使用Addressable加载场景
                sceneHandle = Addressables.LoadSceneAsync(
                    sceneId, // 使用Addressable的key而不是路径
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
                        Debug.Log($"[SceneLoader]: 场景 {sceneId} 已激活");
                    };

                    await UniTask.WaitUntil(() => sceneHandle.Value.Result.Scene.isLoaded);
                }

                _currentSceneId = sceneId;
                _currentSceneHandle = sceneHandle.Value; // 保存句柄以便后续卸载
                _loadedScenes.Add(sceneId);

                Debug.Log($"[SceneLoader]: 场景 {sceneId} 加载完成");
            }
            catch (OperationCanceledException)
            {
                Debug.Log($"[SceneLoader]: 场景 {sceneId} 加载被取消");

                // 取消时清理资源
                if (sceneHandle.HasValue)
                {
                    Addressables.Release(sceneHandle.Value);
                }

                throw;
            }
            catch (Exception ex)
            {
                Debug.LogError($"[SceneLoader]: 加载场景 {sceneId} 失败: {ex.Message}");

                // 异常时清理资源
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
                return config;
            }

            throw new KeyNotFoundException($"未找到场景配置: {sceneId}");
        }

        public async UniTask PreloadSceneAsync(string sceneId)
        {
            if (!_isInitialized)
            {
                throw new InvalidOperationException("[SceneLoader]: 场景加载器未初始化");
            }

            if (!SceneExists(sceneId))
            {
                throw new ArgumentException($"[SceneLoader]: 场景ID '{sceneId}' 不存在");
            }

            if (_loadedScenes.Contains(sceneId))
            {
                Debug.Log($"[SceneLoader]: 场景 {sceneId} 已经预加载过");
                return;
            }

            try
            {
                var config = GetSceneConfig(sceneId);
                string scenePath = Path.Combine(ResourcePath.ScenePath, config.SceneName);

                // 预加载场景（异步加载但不激活）
                var loadOperation = SceneManager.LoadSceneAsync(scenePath,
                    new LoadSceneParameters
                    {
                        loadSceneMode = LoadSceneMode.Additive,
                        localPhysicsMode = LocalPhysicsMode.None
                    });

                if (loadOperation != null)
                {
                    loadOperation.allowSceneActivation = false;

                    while (!loadOperation.isDone)
                    {
                        await UniTask.Yield();
                        if (loadOperation.progress >= 0.9f) // 加载到90%时停止
                            break;
                    }

                    _loadedScenes.Add(sceneId);
                    Debug.Log($"[SceneLoader]: 场景 {sceneId} 预加载完成");
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"[SceneLoader]: 预加载场景 {sceneId} 失败: {ex.Message}");
                throw;
            }
        }

        public bool SceneExists(string sceneId)
        {
            if (_sceneConfigs == null)
            {
                Debug.LogError($"[SceneLoader] _sceneConfigs 是 null!");
                return false;
            }

            if (string.IsNullOrEmpty(sceneId))
            {
                Debug.LogWarning($"[SceneLoader] 场景ID为空");
                return false;
            }

            Debug.Log($"[SceneLoader] 检查场景ID: '{sceneId}'");
            Debug.Log($"[SceneLoader] 配置数量: {_sceneConfigs.Count}");

            bool exists = _sceneConfigs.ContainsKey(sceneId);
            Debug.Log($"[SceneLoader] 存在: {exists}");

            if (!exists)
            {
                Debug.Log($"[SceneLoader] 可用ID: {string.Join(", ", _sceneConfigs.Keys)}");
            }

            return exists;
        }

        public async UniTask UnloadSceneAsync(string sceneId)
        {
            if (!_loadedScenes.Contains(sceneId))
            {
                Debug.LogWarning($"[SceneLoader]: 场景 {sceneId} 未加载，无需卸载");
                return;
            }

            try
            {
                var config = GetSceneConfig(sceneId);
                string sceneName = config.SceneName;

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

                Debug.Log($"[SceneLoader]: 场景 {sceneId} 卸载完成");
            }
            catch (Exception ex)
            {
                Debug.LogError($"[SceneLoader]: 卸载场景 {sceneId} 失败: {ex.Message}");
                throw;
            }
        }

        #region 私有方法
        private async UniTask LoadSceneConfigsAsync(string configPath)
        {
            try
            {
                // 异步读取文件
                string jsonContent;
                using (var reader = new StreamReader(configPath))
                {
                    jsonContent = await reader.ReadToEndAsync();
                    Debug.Log("[SceneLoader]: 已读取场景配置" + jsonContent);
                }

                // 反序列化JSON
                _sceneConfigs = JsonConvert.DeserializeObject<Dictionary<string, SceneConfig>>(jsonContent);
                foreach (var sceneConfig in _sceneConfigs)
                {
                    Debug.Log(sceneConfig.Key + ": " + sceneConfig.Value);
                }

                if (_sceneConfigs.Count == 0)
                {
                    Debug.LogWarning("[SceneLoader]: 场景配置文件为空或格式不正确");
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"[SceneLoader]: 加载场景配置失败: {ex.Message}");
                throw;
            }
        }



        private async UniTask LoadSceneAsync(string scenePath, LoadSceneMode loadMode,
            IProgress<float> progress, CancellationToken cancellationToken)
        {
            try
            {
                var handle = Addressables.LoadSceneAsync(scenePath, loadMode);
                while (!handle.IsDone)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    progress?.Report(handle.PercentComplete);
                    await UniTask.Yield();
                }

                if (handle.Status == AsyncOperationStatus.Failed)
                {
                    throw new Exception($"[SceneLoader]: Addressable场景加载失败: {scenePath}");
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"[SceneLoader]: Addressable加载失败: {ex.Message}");
                throw;
            }
        }

        //        private async UniTask LoadSceneWithProgressAsync(string scenePath, LoadSceneMode loadMode,
        //            IProgress<float> progress, CancellationToken cancellationToken)
        //        {
        //            // 方法1: 使用Unity内置的SceneManager
        //            try
        //            {
        //                var loadOperation = SceneManager.LoadSceneAsync(scenePath,
        //                    new LoadSceneParameters
        //                    {
        //                        loadSceneMode = loadMode
        //                    });

        //                if (loadOperation == null)
        //                {
        //                    throw new InvalidOperationException($"无法加载场景: {scenePath}");
        //                }

        //                loadOperation.allowSceneActivation = false;

        //                // 模拟进度更新（实际加载进度只能到0.9）
        //                while (!loadOperation.isDone)
        //                {
        //                    cancellationToken.ThrowIfCancellationRequested();

        //                    float loadProgress = Mathf.Clamp01(loadOperation.progress / 0.9f);
        //                    progress?.Report(loadProgress);

        //                    if (loadOperation.progress >= 0.9f)
        //                    {
        //                        // 加载完成，激活场景
        //                        loadOperation.allowSceneActivation = true;
        //                    }

        //                    await UniTask.Yield();
        //                }

        //                progress?.Report(1.0f);
        //            }
        //            catch (Exception)
        //            {
        //                // 如果内置方法失败，尝试使用Addressables
        //                await LoadSceneWithAddressablesAsync(scenePath, loadMode, progress, cancellationToken);
        //            }
        //        }

        //        private async UniTask LoadSceneWithAddressablesAsync(string scenePath, LoadSceneMode loadMode,
        //            IProgress<float> progress, CancellationToken cancellationToken)
        //        {
        //            // 方法2: 使用Addressable系统
        //            try
        //            {
        //#if UNITY_EDITOR || USING_ADDRESSABLES
        //                var handle = Addressables.LoadSceneAsync(scenePath, loadMode);

        //                while (!handle.IsDone)
        //                {
        //                    cancellationToken.ThrowIfCancellationRequested();
        //                    progress?.Report(handle.PercentComplete);
        //                    await UniTask.Yield();
        //                }

        //                if (handle.Status == AsyncOperationStatus.Failed)
        //                {
        //                    throw new Exception($"Addressable场景加载失败: {scenePath}");
        //                }
        //#else
        //                throw new NotSupportedException("Addressable系统未启用");
        //#endif
        //            }
        //            catch (Exception ex)
        //            {
        //                Debug.LogError($"[SceneLoader]: Addressable加载失败: {ex.Message}");
        //                throw;
        //            }
        //        }

        // 对应的卸载方法
        private async UniTask UnloadCurrentSceneAsync()
        {
            if (_currentSceneHandle.IsValid())
            {
                Debug.Log($"[SceneLoader]: 开始卸载场景 {_currentSceneId}");

                await Addressables.UnloadSceneAsync(
                    _currentSceneHandle,
                    autoReleaseHandle: true
                ).Task.AsUniTask();

                Debug.Log($"[SceneLoader]: 场景 {_currentSceneId} 卸载完成");
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
            // 异步执行垃圾回收
            await UniTask.Delay(100); // 短暂延迟确保资源完全释放
            GC.Collect();
            await Resources.UnloadUnusedAssets();
        }
        #endregion

        #region 析构函数和资源清理

        ~SceneLoader()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _loadedScenes?.Clear();
                _sceneConfigs?.Clear();
            }
            _isInitialized = false;
        }

        #endregion
    }
}
