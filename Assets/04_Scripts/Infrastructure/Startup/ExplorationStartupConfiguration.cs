using FissuredDawn.Scope.Exploration.Generated.Character;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace FissuredDawn.Infrastructure.Startup
{
    public class ExplorationStartupConfiguration : IStartable
    {
        [Inject] private readonly CharacterManager _characterManager;

        public void Start()
        {
            Debug.Log("[ExplorationStartupConfiguration]: 正在初始化地图实体......");
            _characterManager.Initialize();
        }
    }
}
