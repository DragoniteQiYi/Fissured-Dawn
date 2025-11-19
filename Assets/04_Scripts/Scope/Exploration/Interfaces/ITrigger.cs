using Cysharp.Threading.Tasks;
using FissuredDawn.Shared.Enums.Exploration;
using System;

namespace FissuredDawn.Scope.Exploration.Interfaces
{
    public interface ITrigger
    {
        public TriggerType TriggerType { get; }

        event Action OnCollision;
        event Action OnImmediate;
        event Action OnInteract;

        void AddCondition();
        void RemoveCondition();

        void Execute();
        UniTask ExecuteAsync();

        bool CheckConditions();
    }
}
