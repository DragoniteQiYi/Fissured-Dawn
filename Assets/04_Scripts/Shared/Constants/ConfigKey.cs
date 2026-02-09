namespace FissuredDawn.Shared.Constants
{
    /*
     *  此类用于存储配置文件在Addressable中的键
     *  设计时应该优先使用JSON，仅在必要情况下才使用ScriptableObject
     *  这种静态字符串存储的设计可能并非最佳实践，但至少目前来说很有作用
     *  注意：此类仅适用于设计环节的配置，绝对不能用于客户端可修改配置
     *  客户端配置应该考虑实现新的类型并使用StreamingAssets或PlayerPrefs存储
     */
    public static class ConfigKey
    {
        /// <summary>
        /// 场景配置
        /// </summary>
        public const string SCENE_CONFIG = "Configs/Scenes";

        // public const string AUDIO_CONFIG = ""; → 不可以这样写！
    }
}
