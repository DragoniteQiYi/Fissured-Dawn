using FissuredDawn.Shared.Enums.Core;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace FissuredDawn.Data.Configs
{
    /*
     *  不需要场景ID，因为键本身就是ID
     */
    /// <summary>
    /// 场景配置
    /// </summary>
    [Serializable]
    public class SceneConfig
    {
        /// <summary>
        /// 场景名（仅用于标识）
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

        /// <summary>
        /// 是否预加载资源
        /// </summary>
        public bool PreloadAssets;
    }
}
