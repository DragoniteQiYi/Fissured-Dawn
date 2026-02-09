using FissuredDawn.Data.Exploration;
using FissuredDawn.Scope.Exploration.Interfaces;
using UnityEngine;

namespace FissuredDawn.Scope.Exploration.Interactable
{
    /*
     *  事实证明：多个不同的交互组件是冗余设计
     *  应该让属性决定交互行为而非子类
     *  FUCKING BULLSHIT......
     */
    [RequireComponent(typeof(MapEntity))]
    [RequireComponent(typeof(ITrigger))]
    public class InteractableEntity : MonoBehaviour, IInteractable
    {
        [Header("主体")]
        [SerializeField] private MapEntity _entity;

        [Header("触发事件")]
        [SerializeField] private ITrigger _trigger;

        //[field: SerializeField] public InteractableType InteractableType { get; private set; }
        [field: SerializeField] public bool IsInteractable { get; private set; }
        [field: SerializeField] public int Priority { get; private set; }
        [field: SerializeField] public bool RemoveAfterInteraction { get; private set; } = false;
        [field: SerializeField] public bool RequiresLineOfSight { get; private set; }
        [field: SerializeField] public LayerMask ObstructionLayers { get; private set; }
        [field: SerializeField] public bool SupportsAlternateInteraction { get; private set; }

        // 为什么不用Awake：确保比其依赖组件更晚初始化
        private void Start()
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
            _trigger.Execute();
        }

        public void OnInteractionComplete(GameObject initiator)
        {
            Debug.Log($"[InteractableNPC]: {_entity.EntityId} 交互完成");
        }

        public void OnInitialized()
        {
            _entity = GetComponent<MapEntity>();
            _trigger = GetComponent<ITrigger>();
            Debug.Log($"[InteractableNPC]: {_entity.EntityId} 交互组件已初始化");
        }
    }
}