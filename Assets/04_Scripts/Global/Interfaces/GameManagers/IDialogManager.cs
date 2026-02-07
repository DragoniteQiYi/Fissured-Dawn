using cherrydev;
using System;

namespace FissuredDawn.Global.Interfaces.GameManagers
{
    public interface IDialogManager
    {
        DialogBehaviour DialogBehaviour { get; }

        public bool IsRunning { get; }

        event Action OnDialogStart;
        event Action OnDialogPause;
        event Action OnDialogResume;
        event Action OnDialogEnd;

        /// <summary>
        /// 初始化
        /// </summary>
        void Initialize();

        /// <summary>
        /// 启动指定对话
        /// </summary>
        /// <param name="dialogue"></param>
        void StartDialog(DialogNodeGraph dialogue);

        /// <summary>
        /// 暂停对话
        /// </summary>
        void PauseDialog();

        /// <summary>
        /// 恢复对话
        /// </summary>
        void ResumeDialog();

        /// <summary>
        /// 修改配置
        /// </summary>
        void ModifyConfig();
    }
}
