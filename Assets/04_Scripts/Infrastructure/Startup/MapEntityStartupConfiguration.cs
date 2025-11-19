using FissuredDawn.Scope.Exploration.Generated.Character;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace FissuredDawn.Infrastructure.Startup
{
    public class MapEntityStartupConfiguration : IStartable
    {
        [Inject] private readonly CharacterManager _characterManager;

        public void Start()
        {
            Debug.Log("[MapEntityStartupConfiguration]: 正在初始化地图实体......");
            _characterManager.Initialize();
        }
    }
}
