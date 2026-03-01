using FissuredDawn.Shared.Enums.Core;

namespace FissuredDawn.Global.Interfaces.GameManagers
{
    public interface ILifetimeScopeManager
    {
        void Initialize();

        void SpawnLifetimeScope(SceneTypeEnum sceneType);
    }
}
