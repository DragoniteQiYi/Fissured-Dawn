using System;
using UnityEngine;

namespace FissuredDawn.Scene.Data
{
    public abstract class MapEntity : MonoBehaviour
    {
        [Header("实体基础数据")]
        [SerializeField] protected string entityId;
        [SerializeField] protected EntityType entityType;
        [SerializeField] protected string displayName;
        [SerializeField] protected string description;

        [Header("实体状态")]
        [SerializeField] protected EntityState currentState = EntityState.Idle;
        [SerializeField] protected bool isInteractable = true;
        [SerializeField] protected bool isPersistent = false;

        // 基础事件
        public event Action<MapEntity> OnInteract;
        public event Action<EntityState, EntityState> OnStateChanged;
        public event Action<MapEntity> OnEntityEnabled;
        public event Action<MapEntity> OnEntityDisabled;

        // 基础属性
        public string EntityId => entityId;
        public EntityType EntityType => entityType;
        public EntityState CurrentState => currentState;
        public string DisplayName => displayName;
        public bool IsInteractable => isInteractable;

        protected virtual void Awake()
        {
            // 确保有唯一的ID
            if (string.IsNullOrEmpty(entityId))
            {
                entityId = $"{entityType}_{Guid.NewGuid().ToString("N").Substring(0, 8)}";
            }
        }

        protected virtual void OnEnable()
        {
            OnEntityEnabled?.Invoke(this);
        }

        protected virtual void OnDisable()
        {
            OnEntityDisabled?.Invoke(this);
        }

        protected virtual void OnDestroy()
        {
            // 清理事件订阅
            OnInteract = null;
            OnStateChanged = null;
            OnEntityEnabled = null;
            OnEntityDisabled = null;
        }

        // 状态管理
        public virtual bool SetState(EntityState newState)
        {
            if (currentState == newState) return false;

            var previousState = currentState;
            currentState = newState;
            OnStateChanged?.Invoke(previousState, currentState);
            return true;
        }

        // 交互系统
        public virtual bool CanInteract()
        {
            return isInteractable && currentState != EntityState.Disabled;
        }

        public virtual void Interact(MapEntity interactor = null)
        {
            if (!CanInteract()) return;

            OnInteract?.Invoke(interactor ?? this);
            //HandleInteraction(interactor);
        }
    }
}
