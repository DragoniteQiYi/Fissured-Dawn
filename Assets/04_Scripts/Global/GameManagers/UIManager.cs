using FissuredDawn.Global.Interfaces.GameManagers;
using UnityEngine;
using VContainer;

namespace FissuredDawn.Global.GameManagers
{
    public class UIManager : MonoBehaviour, IUIManager
    {
        [SerializeField] private GameObject _canvasPrefab;
        [SerializeField] private Canvas _windowCanvas;
        [SerializeField] private Canvas _popupCanvas;
        [SerializeField] private Canvas _hudCanvas;
        [SerializeField] private Canvas _backgroundCanvas;

        public bool IsCanvasActive { get; private set; }
        public bool IsWindowCanvasActive => _windowCanvas.gameObject.activeSelf;
        public bool IsPopupCanvasActive => _popupCanvas.gameObject.activeSelf;
        public bool IsHudCanvasActive => _hudCanvas.gameObject.activeSelf;
        public bool IsBackgroundCanvasActive => _backgroundCanvas.gameObject.activeSelf;

        [Inject] private readonly IDialogManager _dialogManager;

        private void OnEnable()
        {
            if (_dialogManager != null)
            {
                _dialogManager.OnDialogStart += HideHud;
                _dialogManager.OnDialogEnd += ShowHud;
            }
        }

        private void OnDisable()
        {
            if (_dialogManager != null)
            {
                _dialogManager.OnDialogStart -= HideHud;
                _dialogManager.OnDialogEnd -= ShowHud;
            } 
        }

        /*
         *  任何情况下，Intialize必须只有在启动项才能调用
         */
        public void Initialize()
        {
            if (_canvasPrefab == null)
            {
                Debug.LogError("[UIManager]: Canvas预制件为空，检查UI Manager预制体");
                return;
            }
            if (IsCanvasActive)
            {
                return;
            }

            _canvasPrefab = Instantiate(_canvasPrefab);
            DontDestroyOnLoad(_canvasPrefab);
            IsCanvasActive = true;
        }

        #region 私有方法
        /*
         *  分情况考虑：
         *  如果对话终止但过场没有终止
         *  那么HideHUD应该检查，阻止HUD展示
         */
        /// <summary>
        /// 显示HUD
        /// </summary>
        private void ShowHud()
        {

        }

        /// <summary>
        /// 隐藏HUD
        /// </summary>
        private void HideHud()
        {

        }

        #endregion
    }
}
