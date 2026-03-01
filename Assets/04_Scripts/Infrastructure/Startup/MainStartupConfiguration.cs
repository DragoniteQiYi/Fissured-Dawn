using Cysharp.Threading.Tasks;
using FissuredDawn.Global.Interfaces.GameManagers;
using FissuredDawn.Global.Interfaces.GameServices;
using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace FissuredDawn.Infrastructure.Startup
{
    /*
     *  这个脚本是为了保证：
     *  第一个场景加载之前，所有的核心系统已初始化
     *  重构方向：核心服务向纯C#类迁移
     */
    public class MainStartupConfiguration : IStartable
    {
        #region MonoBehaviour依赖
        [Inject] private readonly IInputManager _inputManager;
        [Inject] private readonly IDialogManager _dialogManager;
        [Inject] private readonly IUIManager _uiManager;
        [Inject] private readonly IAudioManager _audioManager;
        [Inject] private readonly ILifetimeScopeManager _lifetimeScopeManager;
        #endregion

        #region 纯C#类
        [Inject] private readonly ISceneService _sceneService;
        [Inject] private readonly IInjectService _injectService;
        #endregion

        public void Start()
        {
            Debug.Log("[MainStartupConfiguration]: 游戏正在初始化......");
            InitializeAsync().Forget();
            InitializeServices();
            InitializeManagers();
        }

        private void InitializeServices()
        {
            _injectService.Initialize();
        }

        private void InitializeManagers()
        {
            _inputManager.Initialize();
            _dialogManager.Initialize();
            _uiManager.Initialize();
            _audioManager.Initialize();
            _lifetimeScopeManager.Initialize();
        }

        private async UniTaskVoid InitializeAsync()
        {
            try
            {
                await _sceneService.InitializeAsync();
            }
            catch (Exception ex)
            {
                Debug.LogError($"[MainStartupConfiguration]: 异步初始化失败: {ex.Message}");
            }
        }
    }
}
