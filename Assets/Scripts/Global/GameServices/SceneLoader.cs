using Cysharp.Threading.Tasks;
using FissuredDawn.Data.Configs;
using FissuredDawn.Global.Interfaces.GameServices;
using FissuredDawn.Shared.Constants;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;

namespace FissuredDawn.Global.GameServices
{
    public class SceneLoader : ISceneLoader
    {
        private Dictionary<string, SceneConfig> _sceneConfigs;
        private bool _isInitialized = false;

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
                throw new InvalidOperationException("[SceneLoader]: 场景加载器未初始化，请先调用 InitializeAsync");
            }

            if (!SceneExists(sceneId))
            {
                throw new ArgumentException($"[SceneLoader]: 场景ID '{sceneId}' 不存在");
            }
            try
            {
                // TODO: 资源加载逻辑
            }
            catch (Exception ex) 
            {

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
            throw new NotImplementedException();
        }

        public bool SceneExists(string sceneId)
        {
            throw new NotImplementedException();
        }

        public UniTask UnloadSceneAsync(string sceneId)
        {
            throw new NotImplementedException();
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

        #endregion
    }
}
