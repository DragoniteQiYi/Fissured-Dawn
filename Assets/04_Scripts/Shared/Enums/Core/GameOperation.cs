using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace FissuredDawn
{
    public enum GameOperation
    {
        /// <summary>
        /// 保存游戏
        /// </summary>
        SaveGame,

        /// <summary>
        /// 读取游戏
        /// </summary>
        LoadGame,

        /// <summary>
        /// 开始对话
        /// </summary>
        StartDialogue,

        /// <summary>
        /// 切换场景
        /// </summary>
        SwitchScene,
    }
}
