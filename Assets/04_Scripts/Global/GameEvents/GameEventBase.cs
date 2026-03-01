using Cysharp.Threading.Tasks;
using FissuredDawn.Global.Interfaces;
using System;
using VContainer;

namespace FissuredDawn.Global.GameEvents
{
    [Serializable]
    public class GameEventBase : IGameEvent
    {
        public virtual bool IsAsync { get; }
        public virtual event Action OnEventStart;
        public virtual event Action OnEventEnd;
        public virtual void Execute() { }
        public virtual UniTask ExecuteAsync() { return UniTask.CompletedTask; }

        public virtual void InjectDependencies(IObjectResolver container)
        {
            container.Inject(this);
        }
    }
}
