using VContainer;

namespace FissuredDawn.Infrastructure.DI
{
    /// <summary>
    /// 如果游戏对象不由容器创建，那么使用它来获取服务
    /// </summary>
    public class GlobalServiceLocator
    {
        public static IObjectResolver Container { get; private set; }

        public static void SetContainer(IObjectResolver container)
        {
            Container = container;
        }
    }
}
