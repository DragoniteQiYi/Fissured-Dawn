using FissuredDawn.Global.Interfaces.GameServices;
using UnityEngine;
using VContainer.Unity;

namespace FissuredDawn.Global.GameServices
{
    public class InjectService : IInjectService
    {
        private bool _autoInjectEnabled = true;

        public const string DI_ROOT_NAME = "--- DI Root ---";

        public void Initialize()
        {
            Debug.Log("[InjectService]: 依赖注入服务已初始化");
        }

        // ---------- 公开方法 ----------
        public void InjectGameObject(GameObject target, LifetimeScope parentScope = null)
        {
            var scope = parentScope;
            if (scope == null)
            {
                Debug.Log("[InjectService]: 注入的生命周期为空");
            }

            scope.Container.InjectGameObject(target);
            Debug.Log($"[InjectService]: 尝试在{target.name}中注入依赖");
        }

        public void EnableAutoInject() => _autoInjectEnabled = true;
        public void DisableAutoInject() => _autoInjectEnabled = false;
    }
}
