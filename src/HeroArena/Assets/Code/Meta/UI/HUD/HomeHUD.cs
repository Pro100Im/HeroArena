using Code.Common.Network;
using Code.Common.Transition;
using Code.Infrastructure.Loading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using Unity.Netcode;
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
        [SerializeField] private Button _trainButton;
        [SerializeField] private Button _quickMatchButton;
        [SerializeField] private Button _cancelSearchingButton;
        [Space]
        [SerializeField] private CanvasGroup _introGroup;
        [SerializeField] private CanvasGroup _menuGroup;
        [SerializeField] private CanvasGroup _searchingGroup;
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
            _trainButton.onClick.AddListener(EnterBattleLoadingState);
            _quickMatchButton.onClick.AddListener(QuickMatch);
            _cancelSearchingButton.onClick.AddListener(CancleQuickMatch);

            _transitionService.Execute(0).AsTask();

            _pressAnyBtn.actionTriggered += OnAnyButtonPress;
            _pressAnyBtn.Enable();

            //NetworkManager.Singleton.OnClientStarted += () =>
            //{
            //    Debug.Log("client started");
            //};
        }

        private void Start()
        {
            NetworkManager.Singleton.OnServerStarted += EnterNetworkBattleLoadingState;
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

        private async void EnterNetworkBattleLoadingState()
        {
            _searchingGroup.blocksRaycasts = false;

            await _transitionService.Execute(1);

            _sceneLoader.NetworkLoad(_gameSceneName);       
        }

        private void QuickMatch()
        {
            _menuGroup.DOFade(0, _fadeDuration);
            _menuGroup.blocksRaycasts = false;

            _searchingGroup.DOFade(1, _fadeDuration);
            _searchingGroup.blocksRaycasts = true;

            _networkConnectionService.QuickMatch();
        }

        private void CancleQuickMatch()
        {
            _networkConnectionService.CancelSerching();

            _menuGroup.DOFade(1, _fadeDuration);
            _menuGroup.blocksRaycasts = true;

            _searchingGroup.DOFade(0, _fadeDuration);
            _searchingGroup.blocksRaycasts = false;
        }

        private void OnDestroy()
        {
            NetworkManager.Singleton.OnServerStarted -= EnterNetworkBattleLoadingState;

            _trainButton.onClick.RemoveAllListeners();
            _quickMatchButton.onClick.RemoveAllListeners();
            _cancelSearchingButton.onClick.RemoveAllListeners();
        }
    }
}