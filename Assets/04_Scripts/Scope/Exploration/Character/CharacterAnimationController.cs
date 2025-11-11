using UnityEngine;

namespace FissuredDawn.Scope.Exploration.Character
{
    [RequireComponent(typeof(Animator))]
    public class CharacterAnimationController : MonoBehaviour
    {
        [Header("引用组件")]
        [SerializeField] private CharacterMovementController _movementController;
        [SerializeField] private Animator _animator;

        [Header("动画状态")]
        [SerializeField] private float _faceX;
        [SerializeField] private float _faceY;
        [SerializeField] private bool _isMoving;
        [SerializeField] private bool _isSprinting;

        private readonly int _faceXHash = Animator.StringToHash("FaceX");
        private readonly int _faceYHash = Animator.StringToHash("FaceY");
        private readonly int _isMovingHash = Animator.StringToHash("IsWalking");
        private readonly int _isSprintingHash = Animator.StringToHash("IsSprinting");

        private void Awake()
        {
            _movementController = GetComponent<CharacterMovementController>();
            _animator = GetComponent<Animator>();
            Debug.Log("[CharacterAnimationController]: 角色动画组件初始化成功");
        }

        private void OnEnable()
        {
            if (_movementController == null) return;

            _movementController.OnMoveDirectionChanged += HandleDirectionChanged;
            _movementController.OnSprintStateChanged += HandleSprintStateChanged;
        }

        private void OnDisable()
        {
            if (_movementController == null) return;

            _movementController.OnMoveDirectionChanged -= HandleDirectionChanged;
            _movementController.OnSprintStateChanged -= HandleSprintStateChanged;
        }

        private void HandleDirectionChanged(Vector2 direction)
        {
            if (direction == Vector2.zero)
            {
                _isMoving = false;
                _animator.SetBool(_isMovingHash, _isMoving);
                return;
            }

            _isMoving = true;
            _faceX = direction.x;
            _faceY = direction.y;

            _animator.SetFloat(_faceXHash, _faceX);
            _animator.SetFloat(_faceYHash, _faceY);
            _animator.SetBool(_isMovingHash, _isMoving);
        }

        private void HandleSprintStateChanged(bool isSprinting)
        {
            if (isSprinting)
            {
                _isSprinting = true;
                _animator.SetBool(_isSprintingHash, _isSprinting);
            }
            else
            {
                _isSprinting = false;
                _animator.SetBool(_isSprintingHash, _isSprinting);
            }
        }
    }
}
