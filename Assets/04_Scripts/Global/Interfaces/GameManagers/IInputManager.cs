using System;
using UnityEngine;

namespace FissuredDawn.Global.Interfaces.GameManagers
{
    public interface IInputManager
    {
        /// <summary>
        /// 键盘移动方向(UI/角色)
        /// </summary>
        event Action<Vector2> OnDirectionChanged { add { } remove { } }

        /// <summary>
        /// 按住疾跑键
        /// </summary>
        event Action<bool> OnSprintStateChanged { add { } remove { } }

        /// <summary>
        /// 按下交互键
        /// </summary>
        event Action OnInteractPressed { add { } remove { } }

        /// <summary>
        /// 按下菜单键
        /// </summary>
        event Action OnMenuPressed { add { } remove { } }

        /// <summary>
        /// 按下确定键
        /// </summary>
        event Action OnConfirmPressed { add { } remove { } }

        /// <summary>
        /// 按下返回键
        /// </summary>
        event Action OnCancelPressed { add { } remove { } }

        // 生命周期

        /// <summary>
        /// 初始化
        /// </summary>
        void Initialize();

        /// <summary>
        /// 启用
        /// </summary>
        void Enable();

        /// <summary>
        /// 禁用
        /// </summary>
        void Disable();

        /// <summary>
        /// 获取当前方向
        /// </summary>
        /// <returns></returns>
        Vector2 GetCurrentDirection();

        /// <summary>
        /// 是否正在加速/疾跑
        /// </summary>
        /// <returns></returns>
        bool IsSprinting();

        /// <summary>
        /// 组件是否已启用
        /// </summary>
        /// <returns></returns>
        bool IsEnabled();
    }
}
