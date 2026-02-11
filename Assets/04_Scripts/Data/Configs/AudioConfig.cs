using FissuredDawn.Shared.Enums;
using System;
using UnityEngine;

namespace FissuredDawn.Data.Configs
{
    [Serializable]
    public class AudioConfig
    {
        /// <summary>
        /// 音频名
        /// </summary>
        public string Name = string.Empty;

        /// <summary>
        /// 音频分类
        /// </summary>
        public AudioGroupType GroupType;

        [Range(1,256)]
        /// <summary>
        /// 播放优先级
        /// </summary>
        public int Priority;

        /// <summary>
        /// 是否循环播放
        /// </summary>
        public bool Loop = false;

        /// <summary>
        /// 循环播放起始时间戳
        /// </summary>
        public float LoopStartTime = 0f;

        /// <summary>
        /// 基础音量
        /// </summary>
        public float BasicVolume = 1f;
    }
}
