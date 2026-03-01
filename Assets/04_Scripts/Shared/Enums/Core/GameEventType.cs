namespace FissuredDawn.Shared.Enums.Core
{
    /// <summary>
    /// 用于判断一个游戏事件是同步还是异步触发
    /// </summary>
    public enum GameEventType
    {
        /// <summary>
        /// 游戏事件同步触发
        /// </summary>
        Sync,

        /// <summary>
        /// 游戏事件异步触发
        /// </summary>
        Async
    }
}
