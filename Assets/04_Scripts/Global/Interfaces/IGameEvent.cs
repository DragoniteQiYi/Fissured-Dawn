using Cysharp.Threading.Tasks;
using FissuredDawn.Shared.Enums.Core;
using System;

namespace FissuredDawn.Global.Interfaces
{
    /*
     *  一个NPC对应一种Trigger？
     *  我放你马的屁！！！
     *  要是我想对话完给玩家塞个物品怎么办？
     *  要是我想拿到东西马上触发过场怎么办？
     *  我现在就要传送回两个月前给我自己一耳光！
     */
    public interface IGameEvent
    {
        event Action OnEventStart;

        event Action OnEventEnd;

        bool IsAsync { get; }

        void Execute();

        UniTask ExecuteAsync();
    }
}
