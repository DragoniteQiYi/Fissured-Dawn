using System;

namespace FissuredDawn.Data.Settings
{
    /// <summary>
    /// 音量设置
    /// </summary>
    [Serializable]
    public struct AudioSetting
    {
        /// <summary>
        /// 主音量
        /// </summary>
        public float MainVolume;

        /// <summary>
        /// 音乐音量
        /// </summary>
        public float MusicVolume;

        /// <summary>
        /// UI音效音量
        /// </summary>
        public float UIVolume;

        /// <summary>
        /// 特效音量
        /// </summary>
        public float SFXVolume;

        /// <summary>
        /// 环境音量
        /// </summary>
        public float AmbientVolume;
    }
}
