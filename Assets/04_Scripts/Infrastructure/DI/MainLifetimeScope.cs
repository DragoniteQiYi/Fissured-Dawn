using FissuredDawn.Global;
using FissuredDawn.Global.GameManagers;
using FissuredDawn.Global.GameServices;
using FissuredDawn.Global.Interfaces.GameManagers;
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
        [SerializeField] private DialogManager _dialogManager;
        [SerializeField] private UIManager _uiManager;
        #endregion

        protected override void Configure(IContainerBuilder builder)
        {
            DontDestroyOnLoad(gameObject);

            // 注册MonoBehaviour依赖
            builder.UseComponents(components =>
            {
                components.AddInNewPrefab(_inputManager, Lifetime.Singleton)
                    .UnderTransform(transform)
                    .As<IInputManager>()
                    .AsSelf();
                components.AddInNewPrefab(_dialogManager, Lifetime.Singleton)
                    .UnderTransform(transform)
                    .As<IDialogManager>()
                    .AsSelf();
                components.AddInNewPrefab(_uiManager, Lifetime.Singleton)
                    .UnderTransform(transform)
                    .As<IUIManager>()
                    .AsSelf();
            });

            // 注册纯C#类
            builder.Register<SceneLoader>(Lifetime.Singleton)
                .AsImplementedInterfaces()
                .AsSelf();      

            // 注册初始化器
            builder.RegisterEntryPoint<MainStartupConfiguration>();
        }

        protected override void Awake()
        {
            base.Awake();
            GlobalServiceLocator.SetContainer(Container);
        }
    }
}

