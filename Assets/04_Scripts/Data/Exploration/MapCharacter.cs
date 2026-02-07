using FissuredDawn.Global.Interfaces.GameManagers;
using System;
using UnityEngine;
using VContainer;

namespace FissuredDawn.Data.Exploration
{
    public class MapCharacter : MonoBehaviour
    {
        [Inject] private IInputManager _inputManager;

        [SerializeField] private Vector2 _facingDirection;

        public Vector2 FacingDirection => _facingDirection;
        public event Action<Vector2> OnDirectionChanged;

        private void Awake()
        {
            if (_facingDirection == Vector2.zero)
            {
                _facingDirection = Vector2.down;
            }
            _inputManager.OnDirectionChanged += HandleDirectionChanged;
        }

        private void HandleDirectionChanged(Vector2 direction)
        {
            _facingDirection = direction;
            OnDirectionChanged?.Invoke(_facingDirection);
        }
    }
}