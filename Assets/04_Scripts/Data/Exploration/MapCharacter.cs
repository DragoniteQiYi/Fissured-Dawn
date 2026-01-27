using FissuredDawn.Global.Interfaces.GameManagers;
using System;
using UnityEngine;
using VContainer;

namespace FissuredDawn.Data.Exploration
{
    public class MapCharacter : MapEntity
    {
        [Inject] private IInputManager _inputManager;

        protected new EntityType entityType = EntityType.Character;

        public event Action<Vector2> OnDirectionChanged;

        protected override void Awake()
        {
            base.Awake();
            if (facingDirection == Vector2.zero)
            {
                facingDirection = Vector2.down;
            }
            _inputManager.OnDirectionChanged += HandleDirectionChanged;
        }

        private void HandleDirectionChanged(Vector2 direction)
        {
            facingDirection = direction;
            OnDirectionChanged?.Invoke(facingDirection);
        }
    }
}