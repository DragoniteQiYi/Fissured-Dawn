using cherrydev;
using Cysharp.Threading.Tasks;
using FissuredDawn.Global.Interfaces.GameManagers;
using System;
using UnityEngine;
using VContainer;

namespace FissuredDawn.Global.GameEvents
{
    [Serializable]
    public class DialogEvent : GameEventBase
    {
        [Header("对话图表")]
        [SerializeField] private DialogNodeGraph _dialogNodeGraph;

        [Inject] private readonly IDialogManager _dialogManager;

        // 这时候硬编码倒是很合理
        public override bool IsAsync => false;

        public override event Action OnEventStart;
        public override event Action OnEventEnd;

        public override void Execute()
        {
            if (_dialogManager.IsRunning)
            {
                Debug.LogWarning("[DialogTrigger]: 对话已在进行中");
                return;
            }

            OnEventStart?.Invoke();
            _dialogManager.StartDialog(_dialogNodeGraph);
            OnEventEnd?.Invoke();
        }

        public override UniTask ExecuteAsync() 
        {
            Debug.LogError("[DialogEvent]: 该事件无需异步触发，请重写脚本");
            return UniTask.CompletedTask;
        }
    }
}
