using FissuredDawn.Global.Interfaces.GameManagers;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FissuredDawn.Global.GameManagers
{
    public class InputManager : MonoBehaviour, IInputManager
    {
        [SerializeField] private PlayerInput _playerInput;
        [SerializeField] private bool _isEnabled = true;

        private InputAction _moveAction;
        private InputAction _sprintAction;
        private InputAction _interactAction;
        private InputAction _menuAction;
        private InputAction _submitAction;
        private InputAction _cancelAction;

        public event Action<Vector2> OnDirectionChanged;
        public event Action<bool> OnSprintStateChanged;
        public event Action OnInteractPressed;
        public event Action OnInteractReleased;
        public event Action OnMenuPressed;
        public event Action OnConfirmPressed;
        public event Action OnCancelPressed;

        // 当前输入状态
        private Vector2 _currentDirection;
        private bool _isSprinting;
        private bool _isInitialized = false;

        public void Disable()
        {
            _isEnabled = false;
            _currentDirection = Vector2.zero;
            if (_playerInput != null)
            {
                _playerInput.actions.Disable();
            }
            Debug.Log("[InputManager]: 组件已禁用");
        }

        public void Enable()
        {
            if (_playerInput != null)
            {
                _playerInput.actions.Enable();
            }
            _isEnabled = true;
            _isInitialized = true;
            Debug.Log("[InputManager]: 组件已启用");
        }

        public void Initialize()
        {
            if (_isInitialized)
            {
                Debug.LogWarning("[InputManager]: InputManager出现重复初始化，将跳过本次初始化。");
                return;
            }
            if (_playerInput == null)
            {
                Debug.LogError("[InputManager]: 初始化失败，请检查Inspector中PlayerInput的配置！ ");
                return;
            }
            // Actions存在性检查
            if (!ValidateInputActions())
            {
                return;
            }

            // 缓存所有InputAction引用
            _moveAction = _playerInput.actions["Move"];
            _sprintAction = _playerInput.actions["Sprint"];
            _interactAction = _playerInput.actions["Interact"];
            _menuAction = _playerInput.actions["Menu"];
            //_submitAction = _playerInput.actions["Submit"];
            //_cancelAction = _playerInput.actions["Cancel"];

            // 绑定事件
            _sprintAction.started += OnSprintStarted;
            _sprintAction.canceled += OnSprintCanceled;
            _interactAction.performed += OnInteractPerformed;
            _interactAction.canceled += OnInteractEnd;
            _menuAction.performed += OnMenuPerformed;
            //_submitAction.performed += OnSubmitPerformed;
            //_cancelAction.performed += OnCancelPerformed;

            Enable();
        }

        private void Update()
        {
            // 处理持续输入（如移动方向）
            Vector2 newDirection = _moveAction.ReadValue<Vector2>();
            if (newDirection != _currentDirection)
            {
                _currentDirection = newDirection;
                OnDirectionChanged?.Invoke(_currentDirection);
            }
        }

        private void OnSprintStarted(InputAction.CallbackContext context)
        {
            if (!_isEnabled) return;
            SetSprintState(true);
        }

        private void OnSprintCanceled(InputAction.CallbackContext context)
        {
            if (!_isEnabled) return;
            SetSprintState(false);
        }

        private void OnInteractPerformed(InputAction.CallbackContext context)
        {
            if (!_isEnabled) return;
            OnInteractPressed?.Invoke();
        }

        private void OnInteractEnd(InputAction.CallbackContext context)
        {
            if (!_isEnabled) return;
            OnInteractReleased?.Invoke();
        }

        private void OnMenuPerformed(InputAction.CallbackContext context)
        {
            if (!_isEnabled) return;
            OnMenuPressed?.Invoke();
        }

        private void OnSubmitPerformed(InputAction.CallbackContext context)
        {
            if (!_isEnabled) return;
            OnConfirmPressed?.Invoke();
        }

        private void OnCancelPerformed(InputAction.CallbackContext context)
        {
            if (!_isEnabled) return;
            OnCancelPressed?.Invoke();
        }

        private void SetSprintState(bool isSprinting)
        {
            if (_isSprinting != isSprinting)
            {
                _isSprinting = isSprinting;
                OnSprintStateChanged?.Invoke(_isSprinting);
            }
        }

        private void OnDestroy()
        {
            // 清理事件绑定
            if (_sprintAction != null)
            {
                _sprintAction.started -= OnSprintStarted;
                _sprintAction.canceled -= OnSprintCanceled;
            }

            if (_interactAction != null)
            {
                _interactAction.performed -= OnInteractPerformed;
                _interactAction.canceled -= OnInteractEnd;
            }

            if (_menuAction != null)
            {
                _menuAction.performed -= OnMenuPerformed;
            }

            if (_submitAction != null)
            {
                _submitAction.performed -= OnSubmitPerformed;
            }

            if (_cancelAction != null)
            {
                _cancelAction.performed -= OnCancelPerformed;
            }
        }

        // 2. 验证Input Actions
        private bool ValidateInputActions()
        {
            var actions = _playerInput.actions;
            if (actions == null)
            {
                Debug.LogError("[InputManager]: PlayerInput未配置InputActionAsset");
                return false;
            }

            string[] requiredActions = { "Move", "Sprint", "Interact", "Menu" };
            foreach (var actionName in requiredActions)
            {
                if (actions.FindAction(actionName) == null)
                {
                    Debug.LogError($"[InputManager]: 缺少必需的InputAction: {actionName}");
                    return false;
                }
            }
            return true;
        }

        // 新增：获取当前输入状态的方法
        public Vector2 GetCurrentDirection() => _currentDirection;
        public bool IsSprinting() => _isSprinting;
        public bool IsEnabled() => _isEnabled && _isInitialized;
    }
}
