using Cysharp.Threading.Tasks;
using FissuredDawn.Global.GameManagers;
using FissuredDawn.Global.Interfaces.GameManagers;
using FissuredDawn.Infrastructure.DI;
using FissuredDawn.Scope.Exploration.Interfaces;
using FissuredDawn.Shared.Enums.Exploration;
using System;
using System.Threading;
using UnityEngine;
using VContainer;

namespace FissuredDawn.Scope.Exploration.Designer.Trigger
{
    /*
     *  我预留了TriggerType这一类型以应对需要即时触发的场景
     *  比如在游戏启动时直接加载到标题画面而无需交互
     *  注意：场景切换对性能敏感，必须要异步启动
     */
    public class SceneTrigger : MonoBehaviour, ITrigger
    {
        [SerializeField] private string _name = string.Empty;
        [SerializeField] private TriggerType _triggerType = TriggerType.None;
        [SerializeField] private string _sceneId = string.Empty;
        [SerializeField] private bool _enabled = false;

        private UniTaskCompletionSource<bool> _initializationTaskSource;

        // 注入的服务
        private ISceneLoader _sceneLoader;

        public event Action OnTriggerEnter;
        public event Action OnTriggerExit;

        private void Start()
        {
            _sceneLoader = GlobalServiceLocator.Container.Resolve<ISceneLoader>();
            Debug.Log($"[SceneTrigger]：{_name}场景触发器已初始化");

            // 如果 TriggerType 是 Immediate，在 Start 时自动执行
            if (_triggerType == TriggerType.Immediate)
            {
                UniTask.Void(async() => await ExecuteImmediateTrigger());
            }
        }

        public virtual void Execute() { }

        public async UniTask ExecuteAsync()
        {
            if (!_enabled || string.IsNullOrEmpty(_sceneId))
            {
                Debug.LogWarning($"[SceneTrigger]：场景触发器{_name}未启用或场景ID为空");
                return;
            }

            OnTriggerEnter?.Invoke();
            await LoadTargetScene();
        }

        private async UniTask ExecuteImmediateTrigger()
        {
            if (!_enabled || string.IsNullOrEmpty(_sceneId)) return;

            // 等待场景加载器初始化完成
            await WaitForSceneLoaderInitialization();

            OnTriggerEnter?.Invoke();
            await LoadTargetScene();
        }

        private async UniTask LoadTargetScene()
        {
            try
            {
                if (!_sceneLoader.SceneExists(_sceneId))
                {
                    Debug.LogError($"[SceneTrigger]：场景ID {_sceneId} 不存在");
                    return;
                }

                Debug.Log($"[SceneTrigger]：开始加载场景 {_sceneId}");

                // 使用CancellationToken.None表示不可取消
                await _sceneLoader.LoadSceneAsync(_sceneId, CancellationToken.None);

                Debug.Log($"[SceneTrigger]：场景 {_sceneId} 加载完成");
            }
            catch (Exception ex)
            {
                Debug.LogError($"[SceneTrigger]：加载场景 {_sceneId} 时发生错误：{ex.Message}");
            }
        }

        private async UniTask WaitForSceneLoaderInitialization()
        {
            if (_sceneLoader == null)
            {
                Debug.LogError($"[SceneTrigger]：场景加载器未找到");
                return;
            }

            // 如果 SceneLoader 实现了初始化状态接口
            if (_sceneLoader is SceneLoader concreteLoader)
            {
                if (concreteLoader.IsInitialized) return;

                Debug.Log($"[SceneTrigger]：等待场景加载器初始化...");

                // 创建一个任务完成源用于等待初始化
                if (_initializationTaskSource == null)
                {
                    _initializationTaskSource = new UniTaskCompletionSource<bool>();
                    concreteLoader.OnInitialized += () =>
                    {
                        _initializationTaskSource?.TrySetResult(true);
                        _initializationTaskSource = null;
                    };
                }

                await _initializationTaskSource.Task;
            }
        }
    }
}
