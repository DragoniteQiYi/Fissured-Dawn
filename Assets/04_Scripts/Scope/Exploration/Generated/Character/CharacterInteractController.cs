using FissuredDawn.Data.Exploration;
using FissuredDawn.Global.Interfaces.GameManagers;
using FissuredDawn.Scope.Exploration.Interfaces;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

/*
 *  读后感：优雅！实在是优雅！
 *  你说冗余字段？没BUG我改它干嘛！
 *  —— 奇异 2026/1/28 2:48
 */
namespace FissuredDawn.Scope.Exploration.Generated.Character
{
    [RequireComponent(typeof(MapCharacter))]
    public class CharacterInteractController : MonoBehaviour
    {
        [Inject] private readonly IInputManager _inputManager;

        [Header("组件引用")]
        [SerializeField] private MapCharacter _character;

        [Header("基础配置")]
        [SerializeField] private float _interactionRange = 2f;
        [SerializeField] private float _interactionAngle = 60f;
        [SerializeField] private LayerMask _interactionLayerMask = ~0;
        [SerializeField] private LayerMask _obstructionLayerMask;
        [SerializeField] private Vector2 _raycastDirection;
        [SerializeField] private float _boxCastWidth = 0.5f;
        [SerializeField] private float _boxCastHeight = 1f;
        [SerializeField] private float _targetUpdateInterval = 0.1f;

        [Header("视觉反馈")]
        [SerializeField] private bool _showDebugRay = true;
        [SerializeField] private Color _rayColor = Color.green;
        [SerializeField] private Color _hitRayColor = Color.red;

        [Header("当前检测到的可交互物体")]
        [SerializeField] private IInteractable _currentInteractable;
        [SerializeField] private GameObject _currentTarget;

        [Header("定时器")]
        [SerializeField] private float _targetCheckTimer;

        [Header("交互状态")]
        [SerializeField] private bool _isInteracting = false;

        // 缓存变量
        private readonly Collider2D[] _overlapResults = new Collider2D[10];
        private List<GameObject> _validTargets = new();
        private Transform _characterTransform;
        private Vector2 _characterPosition => _characterTransform.position;

        private void Awake()
        {
            _character = GetComponent<MapCharacter>();
            // 确保_characterTransform在Awake中就有效
            if (_character != null)
            {
                _characterTransform = _character.transform;
            }
            else
            {
                _characterTransform = transform;  // 后备方案
            }

            // 初始化方向
            if (_character != null && _character.FacingDirection.magnitude > 0)
            {
                _raycastDirection = _character.FacingDirection;
            }
        }

        private void Start()
        {
            // 在Start中确保所有组件初始化
            if (_character == null)
            {
                Debug.LogError("[CharacterInteractController]: MapCharacter组件缺失！");
                enabled = false;
                return;
            }

            // 强制初始化方向
            HandleDirectionChanged(_character.FacingDirection);
        }

        private void Update()
        {
            UpdateTargetCheckTimer();
            UpdateCurrentInteractable();
        }

        private void OnEnable()
        {
            SetupInputListeners();
            _character.OnDirectionChanged += HandleDirectionChanged;
        }

        private void OnDisable()
        {
            _character.OnDirectionChanged -= HandleDirectionChanged;
            CleanupInputListeners();
        }

        private void OnDrawGizmos()
        {
            if (!_showDebugRay)
                return;

            // 绘制交互范围扇形
            DrawInteractionSector();

            // 绘制当前检测方向
            DrawRaycastDirection();

            // 绘制当前目标
            if (_currentTarget != null)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(_currentTarget.transform.position, 0.5f);
            }
        }

        #region 输入处理
        private void SetupInputListeners()
        {
            if (_inputManager == null)
            {
                Debug.LogError("[CharacterInputController]: InputManager未注入！");
                return;
            }
            // 订阅交互事件
            _inputManager.OnInteractPressed += OnInteractPressed;
            _inputManager.OnInteractReleased += OnInteractReleased;
        }

        private void CleanupInputListeners()
        {
            if (_inputManager == null)
                return;

            _inputManager.OnInteractPressed -= OnInteractPressed;
            _inputManager.OnInteractReleased -= OnInteractReleased;
        }

        private void OnInteractPressed()
        {
            if (_currentInteractable == null || !_currentInteractable.IsInteractable)
                return;

            // 开始交互
            _isInteracting = true;
           _currentInteractable.OnInteractionStart(gameObject);

            Debug.Log($"开始与 {_currentTarget.name} 交互");
        }

        private void OnInteractReleased()
        {
            if (!_isInteracting || _currentInteractable == null)
                return;

            // 结束交互
            _currentInteractable.OnInteractionCompleted(gameObject);
            _isInteracting = false;

            Debug.Log($"结束与 {_currentTarget?.name} 的交互");
        }

        private void HandleDirectionChanged(Vector2 direction)
        {
            if (direction.magnitude > 0.1f)
            {
                _raycastDirection = direction.normalized;
            }
        }
        #endregion

        #region 目标检测
        private void UpdateTargetCheckTimer()
        {
            _targetCheckTimer -= Time.deltaTime;
            if (_targetCheckTimer <= 0)
            {
                DetectPotentialTargets();
                _targetCheckTimer = _targetUpdateInterval;
            }
        }

        private void DetectPotentialTargets()
        {
            // 清空有效目标列表
            _validTargets.Clear();

            // 圆形检测范围内的所有可交互物体
            int hitCount = Physics2D.OverlapCircleNonAlloc(
                _characterPosition,
                _interactionRange,
                _overlapResults,
                _interactionLayerMask
            );

            for (int i = 0; i < hitCount; i++)
            {
                var collider = _overlapResults[i];
                var obj = collider.gameObject;

                // 跳过自己
                if (obj == gameObject)
                    continue;

                // 检查是否实现IInteractable接口
                var interactable = obj.GetComponent<IInteractable>();
                if (interactable == null || !interactable.IsInteractable)
                    continue;

                // 检查是否在视野角度内
                if (!IsWithinInteractionAngle(obj))
                    continue;

                // 检查是否有遮挡
                if (IsObstructed(obj))
                    continue;

                _validTargets.Add(obj);
            }

            // 选择最近的物体作为目标
            SelectClosestTarget();
        }

        private bool IsWithinInteractionAngle(GameObject target)
        {
            var collider = target.GetComponent<Collider2D>();
            if (collider == null)
                return false;

            // 获取碰撞体边界
            Bounds bounds = collider.bounds;

            // 检查碰撞体的几个关键点是否在扇形内
            Vector2[] testPoints = new Vector2[]
            {
                bounds.center,
                new(bounds.min.x, bounds.min.y),
                new(bounds.max.x, bounds.min.y),
                new(bounds.min.x, bounds.max.y),
                new(bounds.max.x, bounds.max.y),
                new(bounds.center.x, bounds.min.y),
                new(bounds.center.x, bounds.max.y),
                new(bounds.min.x, bounds.center.y),
                new(bounds.max.x, bounds.center.y)
            };

            float halfAngle = _interactionAngle / 2f;

            foreach (var point in testPoints)
            {
                Vector2 directionToPoint = (point - _characterPosition).normalized;
                float angle = Vector2.Angle(_raycastDirection, directionToPoint);

                // 如果任何一个点在扇形内，就认为可交互
                if (angle <= halfAngle)
                {
                    // 额外检查距离
                    float distance = Vector2.Distance(_characterPosition, point);
                    if (distance <= _interactionRange)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private bool IsObstructed(GameObject target)
        {
            if (_obstructionLayerMask == 0)
                return false;

            Vector2 direction = (target.transform.position - _characterTransform.position);
            float distance = direction.magnitude;
            Vector2 directionNormalized = direction.normalized;

            // 使用BoxCast检测遮挡
            var hit = Physics2D.BoxCast(
                _characterPosition,
                new Vector2(_boxCastWidth, _boxCastHeight),
                0f,
                directionNormalized,
                distance,
                _obstructionLayerMask
            );

            // 如果有遮挡物且不是目标本身
            return hit.collider != null && hit.collider.gameObject != target;
        }

        private void SelectClosestTarget()
        {
            GameObject closestTarget = null;
            float closestDistance = float.MaxValue;

            foreach (var target in _validTargets)
            {
                float distance = Vector2.Distance(_characterPosition, target.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestTarget = target;
                }
            }

            // 更新当前目标
            if (closestTarget != _currentTarget)
            {
                UpdateCurrentTarget(closestTarget);
            }
        }

        private void UpdateCurrentTarget(GameObject newTarget)
        {
            // 清除之前的目标提示
            if (_currentInteractable != null && _currentTarget != null)
            {
                _currentInteractable.OnDeactivate(gameObject);
            }

            _currentTarget = newTarget;
            _currentInteractable = newTarget?.GetComponent<IInteractable>();

            // 设置新的目标提示
            if (_currentInteractable != null && _currentTarget != null)
            {
                _currentInteractable.OnActivate(gameObject);
            }
        }
        #endregion

        #region 交互状态更新
        private void UpdateCurrentInteractable()
        {
            if (_currentInteractable == null)
                return;

            // 检查目标是否仍然可交互
            if (!_currentInteractable.IsInteractable)
            {
                ClearCurrentTarget();
                return;
            }

            // 检查目标是否超出范围
            if (_currentTarget != null)
            {
                float distance = Vector2.Distance(_characterPosition, _currentTarget.transform.position);
                if (distance > _interactionRange || !IsWithinInteractionAngle(_currentTarget))
                {
                    ClearCurrentTarget();
                    return;
                }

                // 持续检测遮挡
                if (IsObstructed(_currentTarget))
                {
                    ClearCurrentTarget();
                    return;
                }
            }
        }

        private void ClearCurrentTarget()
        {
            if (_currentInteractable != null)
            {
                _currentInteractable.OnDeactivate(gameObject);
                if (_isInteracting)
                {
                    _currentInteractable.OnInteractionCompleted(gameObject);
                    _isInteracting = false;
                }
            }

            _currentTarget = null;
            _currentInteractable = null;
        }
        #endregion

        #region Debug可视化
        private void DrawInteractionSector()
        {
            Gizmos.color = new Color(0, 1, 0, 0.2f);

            int segments = 20;
            float stepAngle = _interactionAngle / segments;
            Vector2 prevPoint = _characterPosition;

            for (int i = 0; i <= segments; i++)
            {
                float angle = -_interactionAngle / 2 + stepAngle * i;
                Vector2 dir = Quaternion.Euler(0, 0, angle) * _raycastDirection;
                Vector2 point = _characterPosition + dir * _interactionRange;

                if (i > 0)
                {
                    Gizmos.DrawLine(prevPoint, point);
                }
                prevPoint = point;
            }

            // 绘制边界线
            Gizmos.color = new Color(0, 1, 0, 0.5f);
            Vector2 leftBoundary = Quaternion.Euler(0, 0, -_interactionAngle / 2) * _raycastDirection * _interactionRange;
            Vector2 rightBoundary = Quaternion.Euler(0, 0, _interactionAngle / 2) * _raycastDirection * _interactionRange;

            Gizmos.DrawLine(_characterPosition, _characterPosition + leftBoundary);
            Gizmos.DrawLine(_characterPosition, _characterPosition + rightBoundary);
        }

        private void DrawRaycastDirection()
        {
            Gizmos.color = _raycastDirection.magnitude > 0 ? _hitRayColor : _rayColor;
            Gizmos.DrawRay(_characterPosition, _raycastDirection * _interactionRange);

            // 绘制BoxCast范围
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(
                (Vector3)_characterPosition + (Vector3)_raycastDirection * _interactionRange / 2f,
                new Vector3(_boxCastWidth, _boxCastHeight, 0)
            );
        }
        #endregion

        #region 公共方法
        /// <summary>
        /// 强制清除当前交互目标
        /// </summary>
        public void ForceClearTarget()
        {
            ClearCurrentTarget();
        }

        /// <summary>
        /// 手动设置交互目标
        /// </summary>
        public bool SetTarget(GameObject target)
        {
            if (target == null)
            {
                ClearCurrentTarget();
                return false;
            }

            var interactable = target.GetComponent<IInteractable>();
            if (interactable == null || !interactable.IsInteractable)
                return false;

            UpdateCurrentTarget(target);
            return true;
        }

        /// <summary>
        /// 获取当前可交互物体
        /// </summary>
        public IInteractable GetCurrentInteractable()
        {
            return _currentInteractable;
        }

        /// <summary>
        /// 获取当前目标物体
        /// </summary>
        public GameObject GetCurrentTarget()
        {
            return _currentTarget;
        }

        /// <summary>
        /// 是否正在交互中
        /// </summary>
        public bool IsInteracting()
        {
            return _isInteracting;
        }

        /// <summary>
        /// 是否有可交互的目标
        /// </summary>
        public bool HasInteractableTarget()
        {
            return _currentInteractable != null && _currentInteractable.IsInteractable;
        }
        #endregion
    }
}