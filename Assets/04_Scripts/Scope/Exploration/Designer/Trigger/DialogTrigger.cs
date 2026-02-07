using cherrydev;
using Cysharp.Threading.Tasks;
using FissuredDawn.Global.Interfaces.GameManagers;
using FissuredDawn.Infrastructure.DI;
using FissuredDawn.Scope.Exploration.Interfaces;
using System;
using UnityEngine;
using VContainer;

namespace FissuredDawn.Scope.Exploration.Designer.Trigger
{
    public class DialogTrigger : MonoBehaviour, ITrigger
    {
        [Header("对话图表")]
        [SerializeField] private DialogNodeGraph _dialogNodeGraph;

        private IDialogManager _dialogManager;

        public event Action OnTriggerEnter;
        public event Action OnTriggerExit;

        private void Start()
        {
            _dialogManager = GlobalServiceLocator.Container.Resolve<IDialogManager>();
        }

        public void Execute()
        {
            if (_dialogManager.IsRunning)
            {
                Debug.LogWarning("[DialogTrigger]: 对话已在进行中");
                return;
            }

            OnTriggerEnter?.Invoke();
            _dialogManager.StartDialog(_dialogNodeGraph);
            OnTriggerExit?.Invoke();
        }

        public UniTask ExecuteAsync()
        {
            throw new NotImplementedException();
        }
    }
}
