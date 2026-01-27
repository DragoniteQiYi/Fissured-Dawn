using FissuredDawn.Shared.Enums.Exploration;
using UnityEngine;

namespace FissuredDawn.Scope.Exploration.Interfaces
{
    /// <summary>
    /// 可交互物体接口
    /// </summary>
    public interface IInteractable
    {
        /// <summary>
        /// 交互物体类型
        /// </summary>
        InteractableType InteractableType { get; }

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
        /// 当取消交互准备时
        /// </summary>
        void OnDeactivate(GameObject initiator);

        /// <summary>
        /// 交互逻辑
        /// </summary>
        void OnInteractionStart(GameObject initiator);

        /// <summary>
        /// 交互结束逻辑
        /// </summary>
        void OnInteractionCompleted(GameObject initiator);

        /// <summary>
        /// 初始化逻辑
        /// </summary>
        void OnInitialized();
    }
}
