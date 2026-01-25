using Code.Common.Windows;
using Code.Infrastructure.States.GameStates;
using Code.Infrastructure.States.StateMachine;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Code.Meta.UI.HUD
{
    public class HomeHUD : MonoBehaviour
    {
        [SerializeField] private string _gameSceneName = "Game";
        [Space]
        [SerializeField] private Button _startBattleButton;

        private IGameStateMachine _stateMachine;
        private IWindowService _windowService;

        [Inject]
        private void Construct(IGameStateMachine gameStateMachine, IWindowService windowService)
        {
            _stateMachine = gameStateMachine;
            _windowService = windowService;
        }

        private void Awake()
        {
            _startBattleButton.onClick.AddListener(EnterBattleLoadingState);
        }

        private void EnterBattleLoadingState()
        {
            _stateMachine.Enter<LoadingGameState, string>(_gameSceneName);
        }

        private void OnDestroy()
        {
            _startBattleButton.onClick.RemoveAllListeners();
        }
    }
}