using Code.Common.Transition;
using Code.Game.Features.Player.Factory;
using Code.Infrastructure.States.StateInfrastructure;
using Code.Infrastructure.States.StateMachine;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Infrastructure.States.GameStates
{
    public class GameEnterState : SimpleState
    {
        private readonly IGameStateMachine _stateMachine;
        private readonly IPlayerFactory _playerFactory;

        private readonly TransitionService _transitionService;

        public GameEnterState(IGameStateMachine stateMachine, IPlayerFactory playerFactory, TransitionService transitionService)
        {
            _stateMachine = stateMachine;
            _playerFactory = playerFactory;
            _transitionService = transitionService;
        }

        public override void Enter()
        {
            var player = _playerFactory.CreatePlayer(Vector3.zero);

            _transitionService.Execute(0).AsTask();
            _stateMachine.Enter<GameLoopState>();
        }
    }
}