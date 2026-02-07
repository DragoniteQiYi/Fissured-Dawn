using UnityEngine;

namespace FissuredDawn.Scope.Exploration.Interfaces
{
    /*
     * 这个接口用于定义交互行为本身
     * 它只关心通过怎样的方式交互，但不需要知道触发了什么
     * 谁跟我交互？我能交互吗？我有什么行为？
     * IInteractable与ITrigger为严格的单向传递关系
     * IInteractable的行为由实现类决定，而非枚举对象 → 这句话在放屁！
     * 玩家是地图中唯一的交互发起者，所以不需要设计额外接口
     */
    /// <summary>
    /// 可交互物体接口
    /// </summary>
    public interface IInteractable
    {
        /// <summary>
        /// 当前是否可交互
        /// </summary>
        bool IsInteractable { get; }

        /// <summary>
        /// 交互优先级（多个物体的选择）
        /// </summary>
        int Priority { get; }

        /// <summary>
        /// 交互后是否消失
        /// </summary>
        bool RemoveAfterInteraction { get; }


        bool RequiresLineOfSight { get; }
        LayerMask ObstructionLayers { get; }
        bool SupportsAlternateInteraction { get; }

        /// <summary>
        /// 当交互准备时
        /// </summary>
        void OnActivate(GameObject initiator);

        /// <summary>
        /// 当交互准备取消
        /// </summary>
        void OnDeactivate(GameObject initiator);

        /// <summary>
        /// 交互逻辑
        /// </summary>
        void OnInteractionStart(GameObject initiator);

        /// <summary>
        /// 交互结束逻辑
        /// </summary>
        void OnInteractionComplete(GameObject initiator);

        /// <summary>
        /// 初始化逻辑
        /// </summary>
        void OnInitialized();
    }
}
