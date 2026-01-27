using FissuredDawn.Data.Exploration;
using FissuredDawn.Scope.Exploration.Interfaces;
using FissuredDawn.Shared.Enums.Exploration;
using UnityEngine;

namespace FissuredDawn.Scope.Exploration.Interact
{
    [RequireComponent(typeof(MapNPC))]
    public class InteractableNPC : MonoBehaviour, IInteractable
    {
        [Header("主体")]
        [SerializeField] private MapNPC _entity;

        [field: SerializeField] public InteractableType InteractableType { get; private set; }
        [field: SerializeField] public bool IsInteractable { get; private set; }
        [field: SerializeField] public int Priority { get; private set; }
        [field: SerializeField] public bool RemoveAfterInteraction { get; private set; } = false;
        [field: SerializeField] public bool RequiresLineOfSight { get; private set; }
        [field: SerializeField] public LayerMask ObstructionLayers { get; private set; }
        [field: SerializeField] public bool SupportsAlternateInteraction { get; private set; }

        private void Awake()
        {
            OnInitialized();
        }

        public void OnActivate(GameObject initiator)
        {
            Debug.Log($"[InteractableNPC]: {_entity.EntityId} 进入交互范围");
        }

        public void OnDeactivate(GameObject initiator)
        {
            Debug.Log($"[InteractableNPC]: {_entity.EntityId} 离开交互范围");
        }

        public void OnInteractionStart(GameObject initiator)
        {
            Debug.Log($"[InteractableNPC]: {_entity.EntityId} 产生交互");
        }

        public void OnInteractionCompleted(GameObject initiator)
        {
            Debug.Log($"[InteractableNPC]: {_entity.EntityId} 交互完成");
        }

        public void OnInitialized()
        {
            _entity = GetComponent<MapNPC>();
            Debug.Log($"[InteractableNPC]: {_entity.EntityId} 交互组件已初始化");
        }
    }
}