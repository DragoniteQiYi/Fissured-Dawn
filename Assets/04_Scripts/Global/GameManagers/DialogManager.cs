using cherrydev;
using FissuredDawn.Global.Interfaces.GameManagers;
using FissuredDawn.Shared.Constants;
using System;
using UnityEngine;
using VContainer;

namespace FissuredDawn.Global.GameManagers
{
    /*
     *  它唯一的作用是呼出对话系统并提供对外接口
     *  不要让它处理对话业务逻辑
     */
    public class DialogManager : MonoBehaviour, IDialogManager
    {
        [SerializeField] private DialogBehaviour _dialogBehaviour;

        public DialogBehaviour DialogBehaviour => _dialogBehaviour;
        public bool IsRunning { get; private set; }

        public event Action OnDialogStart;
        public event Action OnDialogPause;
        public event Action OnDialogTyping;
        public event Action OnDialogResume;
        public event Action OnDialogEnd;

        [Inject] private IAudioManager _audioManager;

        private void Start()
        {
            if (_audioManager != null)
            {
                OnDialogStart += () => _audioManager.PlaySound(AudioResource.UI_DIALOG_OPEN);
                OnDialogTyping += () => _audioManager.PlaySound(AudioResource.UI_DIALOG_TYPE);
            }
        }

        public void Initialize()
        {
            _dialogBehaviour = GetComponent<DialogBehaviour>();
            _dialogBehaviour.DialogTextCharWrote += HandleSentenceTyping;
            Debug.Log("[DialogManger]: 对话管理器已初始化");
        }

        public void StartDialog(DialogNodeGraph dialog)
        {
            _dialogBehaviour.OnDialogFinished.AddListener(EndDialog);
            _dialogBehaviour.StartDialog(dialog);
            OnDialogStart?.Invoke();
            IsRunning = true;
        }

        public void PauseDialog()
        {
            OnDialogPause?.Invoke();
        }

        public void ResumeDialog()
        {
            OnDialogResume?.Invoke();
        }

        private void HandleSentenceTyping()
        {
            OnDialogTyping?.Invoke();
        }

        private void EndDialog()
        {
            _dialogBehaviour.OnDialogFinished.RemoveListener(EndDialog);
            OnDialogEnd?.Invoke();
            IsRunning = false;
        }
    }
}
