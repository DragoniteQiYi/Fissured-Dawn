using Cysharp.Threading.Tasks;
using FissuredDawn.Global.Interfaces;
using FissuredDawn.Global.Interfaces.GameServices;
using System;
using System.Threading;
using UnityEngine;
using VContainer;

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
