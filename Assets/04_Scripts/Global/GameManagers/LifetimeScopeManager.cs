using FissuredDawn.Data.Configs;
using FissuredDawn.Global.Interfaces.GameManagers;
using FissuredDawn.Global.Interfaces.GameServices;
using FissuredDawn.Shared.Enums.Core;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace FissuredDawn.Global.GameManagers
{
    /*
     *  所有在场景中手动放置游戏对象的组件都不知道依赖注入容器的存在
     *  所以它们在调用核心服务都必须使用静态的公共服务定位
     *  这样一来，使用依赖注入框架的意义何在？
     *  这个类专门用于自动注入所有依赖，避免那些二货组件找不着北......
     *  只要一个游戏对象的组件调用了容器的服务
     *  并且它在设计阶段放置，不是由容器生成
     *  那就把它放在场景的"---DI Root---"之下，这个服务会自动注入所有依赖
     *  由于这个服务的存在，我已经成功把GlobalServiceLocator斩杀，现在它们全都可以[Inject]注入
     */
    public class LifetimeScopeManager : MonoBehaviour, ILifetimeScopeManager
    {
        [Inject] private readonly IInjectService _injectService;
        [Inject] private readonly ISceneService _sceneService;

        [Header("生命周期预制件")]
        [SerializeField] private LifetimeScope _technicLifetimeScope;
        [SerializeField] private LifetimeScope _explorationLifetimeScope;
        [SerializeField] private LifetimeScope _battleLifetimeScope;

        [Header("场景依赖根对象")]
        [SerializeField] private string DI_ROOT_NAME = "---DI Root---";

        private LifetimeScope _currentLifetimeScope;

        private void OnEnable()
        {
            if (_sceneService != null)
            {
                _sceneService.OnSceneLoaded += HandleSceneLoaded;
            }
        }

        private void OnDisable()
        {
            if (_sceneService != null)
            {
                _sceneService.OnSceneLoaded -= HandleSceneLoaded;
            }
        }

        public void Initialize()
        {
            if (_injectService == null || _sceneService == null)
            {
                Debug.LogError("[LifetimeScopeManager]: 依赖未正确注入");
            }
            Debug.Log("[LifetimeScopeManager]: 生命周期管理器已初始化");
        }

        public void SpawnLifetimeScope(SceneTypeEnum sceneType)
        {
            Debug.Log($"[LifetimeScopeManager]: 正在生成{sceneType}场景的生命周期");

            switch (sceneType)
            {
                case SceneTypeEnum.Technic:
                    _currentLifetimeScope = 
                        Instantiate(_technicLifetimeScope);
                    break;

                case SceneTypeEnum.Exploration:
                    _currentLifetimeScope = 
                        Instantiate(_explorationLifetimeScope);
                    break;

                case SceneTypeEnum.Battle:
                    _currentLifetimeScope = 
                        Instantiate(_battleLifetimeScope);
                    break;

                default:
                    break;
            }
        }

        private void HandleSceneLoaded(SceneConfig sceneConfig)
        {
            SpawnLifetimeScope(sceneConfig.SceneType);

            var diRoot = FindDependencyRoot(DI_ROOT_NAME);
            if (diRoot == null)
            {
                Debug.LogError("[InjectManager]: 注入当前场景依赖失败");
            }

            if (_injectService == null)
            {
                Debug.LogError("[LifetimeScopeManager]: InjectService 未注入");
                return;
            }
            _injectService.InjectGameObject(diRoot, _currentLifetimeScope);
        }

        private GameObject FindDependencyRoot(string rootName)
        {
            var diRoot = GameObject.Find(rootName);

            if (diRoot == null)
            {
                Debug.LogError("[InjectManager]: 当前场景不存在DI根对象");
                return null;
            }
            return diRoot;
        }
    }
}
