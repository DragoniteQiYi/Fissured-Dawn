using FissuredDawn.Core.Interfaces.GameManagers;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FissuredDawn.Core.GameManagers
{
    public class InputManager : MonoBehaviour, IInputManager
    {
        [SerializeField] private PlayerInput _playerInput;
        [SerializeField] private bool _isEnabled = true;

        public event Action<Vector2> OnDirectionChanged;
        public event Action OnSprintStateChanged;
        public event Action OnInteractPressed;
        public event Action OnMenuPressed;
        public event Action OnConfirmPressed;
        public event Action OnCancelPressed;

        // 当前输入状态
        private Vector2 _currentDirection;
        private bool _isSprinting;

        public void Disable()
        {
            _isEnabled = false;
            _currentDirection = Vector2.zero;
            Debug.Log("InputManager 已禁用");
        }

        public void Enable()
        {
            _isEnabled = true;
            Debug.Log("InputManager 已启用");
        }

        public void Initialize()
        {
            throw new System.NotImplementedException();
        }

        public void Start()
        {
            Debug.Log("仅仅测试一下");
        }
    }
}
