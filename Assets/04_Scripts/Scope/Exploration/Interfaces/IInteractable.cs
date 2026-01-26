using FissuredDawn.Shared.Enums.Exploration;
using UnityEngine;

namespace FissuredDawn.Scope.Exploration.Interfaces
{
    /// <summary>
    /// 可交互物体接口
    /// </summary>
    public interface IInteractable
    {
        string ObjectName { get; }
        InteractableType InteractableType { get; }
        bool IsInteractable { get; }
        string BlockReason { get; }
        int Priority { get; }
        bool RemoveAfterInteraction { get; }
        bool RequiresLineOfSight { get; }
        LayerMask ObstructionLayers { get; }
        bool SupportsAlternateInteraction { get; }

        void OnPlayerEnterRange();
        void OnPlayerExitRange();
        void OnHighlight();
        void OnUnhighlight();
        void OnInitialized();
    }
}
