using FissuredDawn.Global.Interfaces.GameManagers;
using FissuredDawn.UI.Views;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace cherrydev
{
    public class SentencePanel : MonoBehaviour
    {
        [Header("组件引用")]
        [SerializeField] private TextMeshProUGUI _dialogNameText;
        [SerializeField] private TextMeshProUGUI _dialogText;
        [SerializeField] private Image _dialogCharacterImage;
        [SerializeField] private Image _dialogArrow;

        [Header("参数")]
        [SerializeField] private float _zoomSpeed = 1f;

        [Header("动画")]
        [SerializeField] private AnimationCurve _showCurve;
        [SerializeField] private AnimationCurve _hideCurve;

        private IDialogManager _dialogManager;

        private string _currentFullText;

        /// <summary>
        /// Setting dialogText max visible characters to zero
        /// </summary>
        public void ResetDialogText()
        {
            _dialogText.maxVisibleCharacters = 0;
            _currentFullText = string.Empty;
        }

        /// <summary>
        /// Set dialog text max visible characters to dialog text length
        /// </summary>
        /// <param name="text"></param>
        public void ShowFullDialogText(string text)
        {
            _currentFullText = text;
            _dialogText.text = text;
            _dialogText.maxVisibleCharacters = text.Length;
        }

        /// <summary>
        /// Increasing max visible characters
        /// </summary>
        public void IncreaseMaxVisibleCharacters() => _dialogText.maxVisibleCharacters++;
        
        /// <summary>
        /// Assigning dialog name text, character image sprite and dialog text
        /// </summary>
        public void Setup(string characterName, string text, Sprite sprite)
        {
            _dialogNameText.text = characterName;
            _dialogText.text = text;
            _currentFullText = text;

            if (sprite == null)
            {
                _dialogCharacterImage.color = new Color(_dialogCharacterImage.color.r,
                    _dialogCharacterImage.color.g, _dialogCharacterImage.color.b, 0);
                return;
            }

            _dialogCharacterImage.color = new Color(_dialogCharacterImage.color.r,
                _dialogCharacterImage.color.g, _dialogCharacterImage.color.b, 255);
            _dialogCharacterImage.sprite = sprite;
        }

        /// <summary>
        /// 展示指示箭头
        /// </summary>
        public void ShowArrow()
        {
            _dialogArrow.gameObject.SetActive(true);
        }

        /// <summary>
        /// 展示指示箭头
        /// </summary>
        public void HideArrow()
        {
            _dialogArrow.gameObject.SetActive(false);
        }

        public IEnumerator PlayShowAnimation()
        {
            Debug.Log("[SentencePanel]: 播放展示动画");
            float timer = 0f;

            while (timer <= 1f)
            {
                transform.localScale = Vector3.one * _showCurve.Evaluate(timer);
                timer += Time.deltaTime * _zoomSpeed;
                yield return null;
            }
        }

        public IEnumerator PlayHideAnimation()
        {
            Debug.Log("[SentencePanel]: 播放关闭动画");
            float timer = 0f;

            while (timer <= 1f)
            {
                transform.localScale = Vector3.one * _showCurve.Evaluate(timer);
                timer += Time.deltaTime * _zoomSpeed;
                yield return null;
            }
            yield return null;
        }
    }
}