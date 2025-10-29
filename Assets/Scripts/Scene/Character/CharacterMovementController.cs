using FissuredDawn.Core.Interfaces.GameManagers;
using System;
using UnityEngine;
using VContainer;

namespace FissuredDawn.Scene.Character
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class CharacterMovementController : MonoBehaviour
    {
        [Inject] private readonly IInputManager _inputManager;

        [Header("组件引用")]
        [SerializeField] private Rigidbody2D _rigidbody;

        [Header("移动属性")]
        [SerializeField] private float _speed = 2f;
        [SerializeField] private float _sprintMultiplier = 2f;

        [Header("移动状态")]
        [SerializeField] private Vector2 _moveDirection;
        [SerializeField] private bool _isSprinting;

        // 暴露事件
        public event Action<Vector2> OnMoveDirectionChanged;
        public event Action<bool> OnSprintStateChanged;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            Debug.Log("[CharacterMovementController]: 角色移动组件初始化成功");
        }

        private void OnEnable()
        {
            if (_inputManager == null) return;

            _inputManager.OnDirectionChanged += HandleDirectionChanged;
            _inputManager.OnSprintStateChanged += HandleSprintStateChanged;
        }

        private void OnDisable()
        {
            if (_inputManager == null) return;

            _inputManager.OnDirectionChanged -= HandleDirectionChanged;
            _inputManager.OnSprintStateChanged -= HandleSprintStateChanged;
        }

        private void FixedUpdate()
        {
            Move(_moveDirection);
        }

        private void Move(Vector2 direction)
        {
            if (_rigidbody == null) return;

            // 计算最终速度
            float currentSpeed = _speed * (_isSprinting ? _sprintMultiplier : 1f);
            Vector2 velocity = _moveDirection * currentSpeed;

            _rigidbody.velocity = velocity;
        }

        private void HandleDirectionChanged(Vector2 direction)
        {
            _moveDirection = direction.normalized;
            OnMoveDirectionChanged?.Invoke(_moveDirection);
        }

        private void HandleSprintStateChanged(bool isSprinting)
        {
            _isSprinting = isSprinting;
            OnSprintStateChanged?.Invoke(_isSprinting);
        }
    }
}
