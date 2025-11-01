using FissuredDawn.Global.GameManagers;
using FissuredDawn.Global.GameServices;
using FissuredDawn.Global.Interfaces.GameManagers;
using FissuredDawn.Global.Interfaces.GameServices;
using FissuredDawn.Infrastructure.Startup;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace FissuredDawn.Infrastructure.DI
{
    public class MainLifetimeScope : LifetimeScope
    {
        #region MonoBehaviour依赖
        [SerializeField] private InputManager _inputManager;
        #endregion

        protected override void Configure(IContainerBuilder builder)
        {
            DontDestroyOnLoad(gameObject);

            // 注册MonoBehaviour依赖
            builder.UseComponents(components =>
            {
                components.AddInNewPrefab(_inputManager, Lifetime.Singleton)
                    .UnderTransform(gameObject.transform)
                    .As<IInputManager>()
                    .AsSelf();
            });

            // 注册纯C#类
            builder.Register<SceneLoader>(Lifetime.Singleton)
                .AsImplementedInterfaces()
                .AsSelf();

            // 注册初始化器
            builder.RegisterEntryPoint<MainStartupConfiguration>();
        }
    }
}

