using FissuredDawn.Core.Interfaces.GameManagers;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace FissuredDawn.Infrastructure.Startup
{
    public class MainStartupConfiguration : IStartable
    {
        [Inject] private readonly IInputManager _inputManager;

        public void Start()
        {
            Debug.Log("[MainStartupConfiguration]: 游戏正在初始化......");
            _inputManager.Initialize();
        }
    }
}
