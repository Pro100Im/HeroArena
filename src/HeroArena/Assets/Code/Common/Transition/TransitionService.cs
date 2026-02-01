using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Common.Transition
{
    public class TransitionService : MonoBehaviour
    {
        [SerializeField] private float _duration = 0.3f;
        [Space]
        [SerializeField] private Image _image;
   
        public async UniTask Execute(float endValue)
        {
            DOTween.Kill(_image);

            await _image.DOFade(endValue, _duration).AsyncWaitForCompletion();
        }
    }
}