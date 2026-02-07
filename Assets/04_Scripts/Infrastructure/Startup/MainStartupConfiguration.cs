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
     *  游戏运行时，所有的核心系统已初始化
     */
    /// <summary>
    /// 系统初始化总线
    /// </summary>
    public class MainStartupConfiguration : IStartable
    {
        #region MonoBehaviour依赖
        [Inject] private readonly IInputManager _inputManager;
        [Inject] private readonly IDialogManager _dialogManager;
        [Inject] private readonly IUIManager _uiManager;
        #endregion

        #region 纯C#类
        [Inject] private readonly ISceneLoader _sceneLoader;
        #endregion

        public void Start()
        {
            Debug.Log("[MainStartupConfiguration]: 游戏正在初始化......");
            InitializeAsync().Forget();
            InitializeSync();
        }

        private void InitializeSync()
        {
            _inputManager.Initialize();
            _dialogManager.Initialize();
            _uiManager.Initialize();
        }

        private async UniTaskVoid InitializeAsync()
        {
            try
            {
                await _sceneLoader.InitializeAsync();
            }
            catch (Exception ex)
            {
                Debug.LogError($"[MainStartupConfiguration]: 异步初始化失败: {ex.Message}");
            }
        }
    }
}
