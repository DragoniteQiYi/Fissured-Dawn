namespace FissuredDawn.Data.Exploration
{
    /// <summary>
    /// 基础实体状态
    /// </summary>
    public enum EntityState
    {
        Idle,
        Moving,
        Interacting,
        Disabled,
        Hidden
    }

    /// <summary>
    /// 实体类型分类
    /// </summary>
    public enum EntityType
    {
        Character,
        NPC,
        Environment,
        Collectable,
        Portal
    }
}
