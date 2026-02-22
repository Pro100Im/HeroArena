using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;

namespace Code.Common.Transition
{
    public class TransitionService : MonoBehaviour
    {
        [SerializeField] private UIDocument _transitionScreenDoc;
        [SerializeField] private float _fadeDuration = 0.2f;

        private VisualElement _canvas;

        private void Awake()
        {
            var root = _transitionScreenDoc.rootVisualElement;
            _canvas = root.Q<VisualElement>("Canvas");
        }

        public async UniTask Execute(float endValue)
        {
            await DOTween.To(() => _canvas.style.opacity.value, x => _canvas.style.opacity = x, endValue, _fadeDuration).AsyncWaitForCompletion();
        }
    }
}