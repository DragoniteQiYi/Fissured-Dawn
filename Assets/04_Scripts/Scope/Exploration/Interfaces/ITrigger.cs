using Cysharp.Threading.Tasks;
using FissuredDawn.Shared.Enums.Exploration;
using System;

namespace FissuredDawn.Scope.Exploration.Interfaces
{
    public interface ITrigger
    {
        event Action OnTrigger;

        UniTask ExecuteAsync();
    }
}
