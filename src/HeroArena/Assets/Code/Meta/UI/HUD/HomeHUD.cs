using Code.Common.Network;
using Code.Common.Transition;
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
        [SerializeField] private Button _startHostButton;
        [Space]
        [SerializeField] private CanvasGroup _introGroup;
        [SerializeField] private CanvasGroup _menuGroup;
        [Space]
        [SerializeField] private InputActionMap _pressAnyBtn;

        private ISceneLoader _sceneLoader;
        private INetworkConnectionService _networkConnectionService;
        private TransitionService _transitionService;

        public IObserver<InputControl> OnAnyButton { get; private set; }

        [Inject]
        private void Construct(ISceneLoader sceneLoader, INetworkConnectionService networkConnectionService, TransitionService transitionService)
        {
            _sceneLoader = sceneLoader;
            _transitionService = transitionService;
            _networkConnectionService = networkConnectionService;
        }

        private void Awake()
        {
            _startBattleButton.onClick.AddListener(EnterBattleLoadingState);
            _startHostButton.onClick.AddListener(StartHost);

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

            _sceneLoader.LocalLoad(_gameSceneName);
        }

        private void StartHost()
        {
            _networkConnectionService.QuickMatch();
        }

        private void OnDestroy()
        {
            _startBattleButton.onClick.RemoveAllListeners();
            _startHostButton.onClick.RemoveAllListeners();
        }
    }
}