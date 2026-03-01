using Cysharp.Threading.Tasks;
using FissuredDawn.Data.Configs;
using System;
using System.Threading;

namespace FissuredDawn.Global.Interfaces.GameServices
{
    public interface ISceneService
    {
        bool IsInitialized { get; }

        event Action OnInitialized;
        event Action<SceneConfig> OnSceneLoaded;

        /// <summary>
        /// 初始化场景配置
        /// </summary>
        UniTask InitializeAsync();

        /// <summary>
        /// 异步加载场景
        /// </summary>
        UniTask LoadSceneAsync(string sceneId, CancellationToken cancellationToken = default);

        /// <summary>
        /// 根据场景ID加载场景（带进度回调）
        /// </summary>
        UniTask LoadSceneAsync(string sceneId, IProgress<float> progress,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// 获取指定场景配置
        /// </summary>
        SceneConfig GetSceneConfig(string sceneId);

        /// <summary>
        /// 检查场景是否存在
        /// </summary>
        bool SceneExists(string sceneId);

        /// <summary>
        /// 卸载场景
        /// </summary>
        UniTask UnloadSceneAsync(string sceneId);

        /// <summary>
        /// 获取当前场景ID
        /// </summary>
        string GetCurrentSceneId();
    }
}