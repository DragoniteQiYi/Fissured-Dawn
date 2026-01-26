namespace FissuredDawn.Global.Interfaces.GameServices
{
    public interface IGameStateService
    {
        /// <summary>
        /// 保存游戏到指定序号存档
        /// </summary>
        void SaveGame(int index);
        
        /// <summary>
        /// 读取指定序号的存档
        /// </summary>
        /// <param name="index"></param>
        void LoadGame(int index);

        /// <summary>
        /// 删除指定序号的存档
        /// </summary>
        void DeleteSave(int index);
    }
}
