using Code.Common.Windows;
using Code.Infrastructure.Loading;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Code.Meta.UI.HUD
{
    public class HomeHUD : MonoBehaviour
    {
        [SerializeField] private string _gameSceneName = "Game";
        [Space]
        [SerializeField] private Button _startBattleButton;

        private ISceneLoader _sceneLoader;
        private IWindowService _windowService;

        [Inject]
        private void Construct(ISceneLoader sceneLoader, IWindowService windowService)
        {
            _sceneLoader = sceneLoader;
            _windowService = windowService;
        }

        private void Awake()
        {
            _startBattleButton.onClick.AddListener(EnterBattleLoadingState);
        }

        private void EnterBattleLoadingState()
        {
            _sceneLoader.LoadScene(_gameSceneName);
        }

        private void OnDestroy()
        {
            _startBattleButton.onClick.RemoveAllListeners();
        }
    }
}