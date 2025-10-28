using FissuredDawn.Core.GameManagers;
using FissuredDawn.Core.Interfaces.GameManagers;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace FissuredDawn.Infrastructure.DI
{
    /*
     * 目前不知道这个脚本有神马用
     *        ╮(╯▽╰)╭
     *        放着不管
     * 2025/10/23 - 我错了！
     */
    public class GameInstaller : IInstaller
    {
        [SerializeField] private InputManager _inputManagerPrefab;

        public void Install(IContainerBuilder builder)
        {
            // builder.Register<IInputManager, InputManager>(Lifetime.Singleton);

            //builder.RegisterComponentOnNewGameObject<InputManager>(Lifetime.Singleton, "InputManager")
            //    .DontDestroyOnLoad();
        }
    }
}
