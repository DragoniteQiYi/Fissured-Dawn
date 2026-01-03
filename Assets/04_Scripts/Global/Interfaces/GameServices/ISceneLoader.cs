using Cysharp.Threading.Tasks;
using FissuredDawn.Data.Configs;
using System;
using System.Threading;

namespace FissuredDawn.Global.Interfaces.GameServices
{
    public interface ISceneLoader
    {
        bool IsInitialized { get; }

        event Action OnInitialized;

        /// <summary>
        /// 初始化场景配置
        /// </summary>
        UniTask InitializeAsync();

        /// <summary>
        /// 异步加载场景
        /// </summary>
        /// <param name="sceneId"></param>
        /// <returns></returns>
        UniTask LoadSceneAsync(string sceneId, CancellationToken cancellationToken = default);

        /// <summary>
        /// 根据场景ID加载场景（带进度回调）
        /// </summary>
        /// <param name="sceneId">场景ID</param>
        /// <param name="progress">进度回调</param>
        /// <param name="cancellationToken">取消令牌</param>
        UniTask LoadSceneAsync(string sceneId, IProgress<float> progress, 
            CancellationToken cancellationToken = default);

        /// <summary>
        /// 获取指定场景配置
        /// </summary>
        /// <param name="sceneId"></param>
        /// <returns></returns>
        SceneConfig GetSceneConfig(string sceneId);

        /// <summary>
        /// 预加载场景资源
        /// </summary>
        /// <param name="sceneId">场景ID</param>
        UniTask PreloadSceneAsync(string sceneId);

        /// <summary>
        /// 卸载场景
        /// </summary>
        /// <param name="sceneId">场景ID</param>
        UniTask UnloadSceneAsync(string sceneId);

        /// <summary>
        /// 检查场景是否存在
        /// </summary>
        /// <param name="sceneId">场景ID</param>
        /// <returns>是否存在</returns>
        bool SceneExists(string sceneId);
    }
}
