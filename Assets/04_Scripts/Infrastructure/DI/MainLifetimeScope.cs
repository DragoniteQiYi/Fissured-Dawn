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
    /*
     *  [LifetimeScope]: 我的存在，就是为了把全局单例和上帝类摁在地上摩擦！
     */
    public class MainLifetimeScope : LifetimeScope
    {
        #region MonoBehaviour依赖
        [SerializeField] private InputManager _inputManager;
        [SerializeField] private DialogManager _dialogManager;
        [SerializeField] private UIManager _uiManager;
        [SerializeField] private AudioManager _audioManager;
        [SerializeField] private LifetimeScopeManager _lifetimeScopeManager;
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
                components.AddInNewPrefab(_audioManager, Lifetime.Singleton)
                    .UnderTransform(transform)
                    .As<IAudioManager>()
                    .AsSelf();
                components.AddInNewPrefab(_lifetimeScopeManager, Lifetime.Singleton)
                    .UnderTransform(transform)
                    .As<ILifetimeScopeManager>()
                    .AsSelf();
            });

            // 注册纯C#类
            builder.Register<SceneService>(Lifetime.Singleton)
                .As<ISceneService>()
                .AsSelf();
            builder.Register<InjectService>(Lifetime.Singleton)
                .As<IInjectService>()
                .AsSelf();

            // 注册初始化器
            builder.RegisterEntryPoint<MainStartupConfiguration>();
        }

        protected override void Awake()
        {
            base.Awake();
            GlobalServiceLocator.SetContainer(Container);

            // 自己注入自己，避免那些蠢货组件找不着北
            Container.InjectGameObject(gameObject);
        }
    }
}

