using Cysharp.Threading.Tasks;
using FissuredDawn.Global.GameEvents;
using FissuredDawn.Scope.Exploration.Interfaces;
using FissuredDawn.Shared.Enums.Exploration;
using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

namespace FissuredDawn.Scope.Exploration.Designer
{
    public class GameEventTrigger : MonoBehaviour, ITrigger
    {
        [SerializeReference] private List<GameEventBase> _gameEvents = new();
        [SerializeField] private TriggerType _triggerType = TriggerType.None;

        public event Action OnTriggerEnter;
        public event Action OnTriggerExit;

        [Inject] private IObjectResolver _container;

        public void Execute()
        {
            if (_container == null)
            {
                Debug.LogError("[GameEventTrigger]: ÕÒ²»µ½ÒÀÀµ×¢ÈëÈÝÆ÷£¬FUCK!");
            }

            foreach (var gameEvent in _gameEvents)
            {
                gameEvent.InjectDependencies(_container);

                if (gameEvent.IsAsync)
                {
                    UniTask.Void(async () =>
                    {
                        await gameEvent.ExecuteAsync();
                    });
                }
                else
                {
                    gameEvent.Execute();
                }
            }
        }
    }
}
