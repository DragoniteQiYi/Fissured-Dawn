using System;
using UnityEngine;

namespace FissuredDawn.Data.Exploration
{
    /*
     *  这曾是一个抽象类
     *  直到本人发现衍生出一堆子类屁用没有
     *  反正它们都不包含业务逻辑
     *  如果一个对象可交互，那么它必定是一个MapEntity
     *  但Trigger可以独立于MapEntity存在
     */
    public class MapEntity : MonoBehaviour
    {
        [Header("实体基础数据")]
        [SerializeField] private string _entityId;
        [SerializeField] private EntityType _entityType;
        [SerializeField] private string _displayName;
        [SerializeField] private string _description;

        [Header("实体状态")]
        [SerializeField] private EntityState _currentState = EntityState.Idle;
        [SerializeField] private Vector2 _facingDirection = new(0, 0);
        [SerializeField] private bool _isVisible = true;
        [SerializeField] private bool _isPersistent = false;

        // 基础事件
        public event Action<EntityState, EntityState> OnStateChanged;
        public event Action<MapEntity> OnEntityEnabled;
        public event Action<MapEntity> OnEntityDisabled;

        // 基础属性
        public string EntityId => _entityId;
        public EntityType EntityType => _entityType;
        public EntityState CurrentState => _currentState;
        public Vector2 FacingDirection => _facingDirection;
        public string DisplayName => _displayName;

        protected virtual void Awake()
        {
            // 确保有唯一的ID
            if (string.IsNullOrEmpty(_entityId))
            {
                _entityId = $"{_entityType}_{Guid.NewGuid().ToString("N")[..8]}";
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
            OnStateChanged = null;
            OnEntityEnabled = null;
            OnEntityDisabled = null;
        }

        // 状态管理
        public virtual bool SetState(EntityState newState)
        {
            if (_currentState == newState) return false;

            var previousState = _currentState;
            _currentState = newState;
            OnStateChanged?.Invoke(previousState, _currentState);
            return true;
        }
    }
}
