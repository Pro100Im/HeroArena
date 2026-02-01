using Code.Common.Transition;
using Code.Common.Windows;
using Code.Infrastructure.Loading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using VContainer;

namespace Code.Meta.UI.HUD
{
    public class HomeHUD : MonoBehaviour
    {
        [SerializeField] private float _fadeDuration = 0.2f;
        [SerializeField] private string _gameSceneName = "Game";
        [Space]
        [SerializeField] private Button _startBattleButton;
        [Space]
        [SerializeField] private CanvasGroup _introGroup;
        [SerializeField] private CanvasGroup _menuGroup;
        [Space]
        [SerializeField] private InputActionMap _pressAnyBtn;

        private ISceneLoader _sceneLoader;
        private TransitionService _transitionService;

        public IObserver<InputControl> OnAnyButton { get; private set; }

        [Inject]
        private void Construct(ISceneLoader sceneLoader, TransitionService transitionService)
        {
            _sceneLoader = sceneLoader;
            _transitionService = transitionService;
        }

        private void Awake()
        {
            _startBattleButton.onClick.AddListener(EnterBattleLoadingState);
            _transitionService.Execute(0).AsTask();
            _pressAnyBtn.actionTriggered += OnAnyButtonPress;
            _pressAnyBtn.Enable();
        }

        private void OnAnyButtonPress(InputAction.CallbackContext context)
        {
            if (context.canceled)
            {
                _pressAnyBtn.Disable();
                _pressAnyBtn.actionTriggered -= OnAnyButtonPress;

                _introGroup.DOFade(0, _fadeDuration);
                _introGroup.blocksRaycasts = false;

                _menuGroup.DOFade(1, _fadeDuration);
                _menuGroup.blocksRaycasts = true;
            }
        }

        private async void EnterBattleLoadingState()
        {
            await _transitionService.Execute(1);

            _sceneLoader.LoadScene(_gameSceneName);
        }

        private void OnDestroy()
        {
            _startBattleButton.onClick.RemoveAllListeners();
        }
    }
}