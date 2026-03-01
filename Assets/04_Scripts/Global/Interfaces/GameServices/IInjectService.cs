using UnityEngine;
using VContainer.Unity;

namespace FissuredDawn.Global.Interfaces.GameServices
{
    public interface IInjectService
    {
        void Initialize();

        /// <summary> 
        /// 手动为指定GameObject执行依赖注入
        /// </summary>
        void InjectGameObject(GameObject target, LifetimeScope parentScope = null);

        /// <summary>
        /// 启用自动注入
        /// </summary>
        void EnableAutoInject();

        /// <summary>
        /// 禁用自动注入
        /// </summary>
        void DisableAutoInject();
    }
}
