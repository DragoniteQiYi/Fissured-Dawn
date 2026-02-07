using cherrydev;
using FissuredDawn.Global.Interfaces.GameManagers;
using System;
using UnityEngine;

namespace FissuredDawn.Global.GameManagers
{
    /*
     *  它唯一的作用是呼出对话系统并提供对外接口
     *  不要让它处理对话业务
     */
    public class DialogManager : MonoBehaviour, IDialogManager
    {
        [SerializeField] private DialogBehaviour _dialogBehaviour;

        public DialogBehaviour DialogBehaviour => _dialogBehaviour;
        public bool IsRunning { get; private set; }

        public event Action OnDialogStart;
        public event Action OnDialogPause;
        public event Action OnDialogResume;
        public event Action OnDialogEnd;

        public void Initialize()
        {
            _dialogBehaviour = GetComponent<DialogBehaviour>();
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

        /// <summary>
        /// 看看再说，还没到这个功能，不确定是否用它
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        public void ModifyConfig()
        {
            throw new System.NotImplementedException();
        }

        private void EndDialog()
        {
            _dialogBehaviour.OnDialogFinished.RemoveListener(EndDialog);
            OnDialogEnd?.Invoke();
            IsRunning = false;
        }
    }
}
