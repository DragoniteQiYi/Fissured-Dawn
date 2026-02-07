using FissuredDawn.Shared.Enums.Core;
using System;

namespace FissuredDawn.Global.Interfaces.GameManagers
{
    /// <summary>
    /// UIManager的初始化时机为游戏运行
    /// </summary>
    public interface IUIManager
    {
        /// <summary>
        /// 系统初始化
        /// </summary>
        void Initialize();

        ///// <summary>
        ///// 根据当前场景展示全部必要UI
        ///// </summary>
        //void ShowNecessary();

        ///// <summary>
        ///// 按需展示特定UI
        ///// </summary>
        ///// <param name="uiName">UI的名称</param>
        //void Show(string uiName);

        ///// <summary>
        ///// 隐藏特定UI
        ///// </summary>
        ///// <param name="uiName">UI的名称</param>
        //void Hide(UIType uiType);
    }
}
