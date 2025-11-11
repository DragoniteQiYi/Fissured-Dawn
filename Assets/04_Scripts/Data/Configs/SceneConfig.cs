using FissuredDawn.Shared.Enums.Core;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace FissuredDawn.Data.Configs
{
    /// <summary>
    /// 场景配置
    /// </summary>
    [Serializable]
    public class SceneConfig
    {
        ///// <summary>
        ///// 场景ID
        ///// </summary>
        //public string Id;

        /// <summary>
        /// 场景名
        /// </summary>
        public string SceneName;

        /// <summary>
        /// 场景BGM_ID
        /// </summary>
        public string BGMId;

        /// <summary>
        /// 场景类型
        /// </summary>
        public SceneTypeEnum SceneType;

        /// <summary>
        /// 读取模式
        /// </summary>
        public LoadSceneMode LoadSceneMode;

        /// <summary>
        /// 子场景
        /// </summary>
        public List<string> Children;
    }
}
