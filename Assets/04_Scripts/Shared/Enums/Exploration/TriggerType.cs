using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FissuredDawn.Shared.Enums.Exploration
{
    /// <summary>
    /// 场景触发器类型
    /// </summary>
    public enum TriggerType
    {
        /// <summary>
        /// 接触触发
        /// </summary>
        Collision = 0,

        /// <summary>
        /// 直接触发（进入场景）
        /// </summary>
        Immediate = 1,

        /// <summary>
        /// 交互触发
        /// </summary>
        Interact = 2,
    }
}
