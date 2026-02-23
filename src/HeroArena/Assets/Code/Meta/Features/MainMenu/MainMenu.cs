using Code.Common.Network;
using Code.Common.UI.Transition;
using Code.Infrastructure.Loading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using VContainer;

namespace Code.Meta.UI.MainMenu
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private string _gameSceneName = "Game";
        [Space]
        [SerializeField] private UIDocument _mainMenuDoc;
        [SerializeField] private float _fadeDuration = 0.2f;
        [Space]
        [SerializeField] private InputActionMap _pressAnyBtn;

        private VisualElement _searchingPopUp;
        private VisualElement _mainMenu;
        private VisualElement _intro;

        private Button _quickMatchButton;
        private Button _exitButton;
        private Button _cancelSearchingButton;

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
            var root = _mainMenuDoc.rootVisualElement;

            _intro = root.Q<VisualElement>("Intro");

            _searchingPopUp = root.Q<VisualElement>("SearchingPopUp");
            _mainMenu = root.Q<VisualElement>("MainMenu");

            _quickMatchButton = root.Q<Button>("QuickPlayButton");
            _quickMatchButton.clickable.clicked += QuickMatch;

            _exitButton = root.Q<Button>("ExitButton");
            _exitButton.clickable.clicked += Exit;

            _cancelSearchingButton = root.Q<Button>("CancelButton");
            _cancelSearchingButton.clickable.clicked += CancleQuickMatch;

            _pressAnyBtn.actionTriggered += OnAnyButtonPress;
            _pressAnyBtn.Enable();
        }

        private void Start()
        {
            NetworkManager.Singleton.OnServerStarted += EnterNetworkBattleLoadingState;

            _transitionService.Execute(0).AsTask();
        }

        private void OnAnyButtonPress(InputAction.CallbackContext context)
        {
            if (context.canceled)
            {
                _pressAnyBtn.Disable();
                _pressAnyBtn.actionTriggered -= OnAnyButtonPress;

                DOTween.To(() => _intro.style.opacity.value, x => _intro.style.opacity = x, 0f, _fadeDuration).SetEase(Ease.InOutQuad);

                DOTween.To(() => _mainMenu.style.opacity.value, x => _mainMenu.style.opacity = x, 1f, _fadeDuration).SetEase(Ease.InOutQuad);
                _quickMatchButton.pickingMode = PickingMode.Position;
                _exitButton.pickingMode = PickingMode.Position;
            }
        }

        private async void EnterNetworkBattleLoadingState()
        {
            _cancelSearchingButton.pickingMode = PickingMode.Ignore;

            await _transitionService.Execute(1);

            _sceneLoader.NetworkLoad(_gameSceneName);       
        }

        private void QuickMatch()
        {
            DOTween.To(() => _mainMenu.style.opacity.value, x => _mainMenu.style.opacity = x, 0f, _fadeDuration).SetEase(Ease.InOutQuad);
            _quickMatchButton.pickingMode = PickingMode.Ignore;
            _exitButton.pickingMode = PickingMode.Ignore;

            DOTween.To(() => _searchingPopUp.style.opacity.value, x => _searchingPopUp.style.opacity = x, 1f, _fadeDuration).SetEase(Ease.InOutQuad);
            _cancelSearchingButton.pickingMode = PickingMode.Position;

            _networkConnectionService.QuickMatch();
        }

        private void CancleQuickMatch()
        {
            _networkConnectionService.CancelSerching();

            DOTween.To(() => _mainMenu.style.opacity.value, x => _mainMenu.style.opacity = x, 1f, _fadeDuration).SetEase(Ease.InOutQuad);
            _quickMatchButton.pickingMode = PickingMode.Position;
            _exitButton.pickingMode = PickingMode.Position;

            DOTween.To(() => _searchingPopUp.style.opacity.value, x => _searchingPopUp.style.opacity = x, 0f, _fadeDuration).SetEase(Ease.InOutQuad);
            _cancelSearchingButton.pickingMode = PickingMode.Ignore;
        }

        private void Exit()
        {

        }

        private void OnDestroy()
        {
            if(NetworkManager.Singleton)
                NetworkManager.Singleton.OnServerStarted -= EnterNetworkBattleLoadingState;

            _quickMatchButton.clickable.clicked -= QuickMatch;
            _exitButton.clickable.clicked -= Exit;
            _cancelSearchingButton.clickable.clicked -= CancleQuickMatch;
        }
    }
}