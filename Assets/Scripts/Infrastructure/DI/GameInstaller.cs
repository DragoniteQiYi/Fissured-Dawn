using FissuredDawn.Core.GameManagers;
using FissuredDawn.Core.Interfaces.GameManagers;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace FissuredDawn.Infrastructure.DI
{
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
