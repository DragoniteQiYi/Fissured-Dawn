using FissuredDawn.Core.GameManagers;
using FissuredDawn.Core.Interfaces.GameManagers;
using FissuredDawn.Infrastructure.Startup;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace FissuredDawn.Infrastructure.DI
{
    public class MainLifetimeScope : LifetimeScope
    {
        [SerializeField] private Transform _systemTransform;
        [SerializeField] private InputManager _inputManager;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.UseComponents(components =>
            {
                components.AddInNewPrefab(_inputManager, Lifetime.Singleton)
                    .UnderTransform(_systemTransform)
                    .DontDestroyOnLoad()
                    .As<IInputManager>()
                    .AsSelf();
            });
            // 注册初始化器
            builder.RegisterEntryPoint<MainStartupConfiguration>();
        }
    }
}

