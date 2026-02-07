using Cysharp.Threading.Tasks;
using System;

namespace FissuredDawn.Scope.Exploration.Interfaces
{
    /*
     * ITrigger只关心它要执行什么，而不在乎谁触发了它
     * 因此它不应该知道IInteractable的存在
     * 它的功能应该由实现类决定，而非枚举对象
     * 为什么又和IInteractable不同了？因为每种Trigger拥有单一职责
     */
    /// <summary>
    /// 触发器接口
    /// </summary>
    public interface ITrigger
    {
        event Action OnTriggerEnter;
        event Action OnTriggerExit;

        /// <summary>
        /// 适用于普遍的操作
        /// </summary>
        void Execute();

        /// <summary>
        /// 适用于场景加载等独立线程需求
        /// </summary>
        /// <returns></returns>
        UniTask ExecuteAsync();
    }
}
