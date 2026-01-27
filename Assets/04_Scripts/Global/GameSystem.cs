using Cysharp.Threading.Tasks;
using FissuredDawn.Global.Interfaces;
using FissuredDawn.Global.Interfaces.GameServices;
using System;
using System.Threading;
using UnityEngine;
using VContainer;

/*
 *  目前不知道这个脚本有什么用......
 *  大概以前本人想拿它做事件总线吧
 *  以后再看，现在没啥必要动它
 *  ——奇异 2026/1/28
 */
namespace FissuredDawn
{
    public class GameSystem : MonoBehaviour, IGameSystem
    {
        [Inject] private ISceneLoader _sceneLoader;

        public async UniTask LoadSceneAsync(string sceneId, CancellationToken cancellationToken = default)
        {
            await _sceneLoader.LoadSceneAsync(sceneId, cancellationToken);
        }

        public async UniTask LoadSceneAsync(string sceneId, IProgress<float> progress, CancellationToken cancellationToken = default)
        {
            await _sceneLoader.LoadSceneAsync(sceneId, progress, cancellationToken);    
        }
    }
}
