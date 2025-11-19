using Cysharp.Threading.Tasks;
using System;
using System.Threading;

namespace FissuredDawn.Global.Interfaces
{
    public interface IGameSystem
    {
        UniTask LoadSceneAsync(string sceneId, CancellationToken cancellationToken = default);
        UniTask LoadSceneAsync(string sceneId, IProgress<float> progress,
            CancellationToken cancellationToken = default);
    }
}
