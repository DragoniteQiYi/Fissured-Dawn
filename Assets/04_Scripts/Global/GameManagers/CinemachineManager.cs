using Cinemachine;
using FissuredDawn.Data.Exploration;
using FissuredDawn.Scope.Exploration.Interfaces;
using UnityEngine;
using VContainer;

namespace FissuredDawn.Global.GameManagers
{
    public class CinemachineManager : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera _virtualCamera;
        [SerializeField] private Transform _playerTransform;

        [Inject] IEntityManager<MapCharacter> _characterManager;


    }
}
