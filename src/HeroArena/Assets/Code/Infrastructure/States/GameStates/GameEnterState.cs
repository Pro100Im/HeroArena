using Code.Game.Features.Player.Factory;
using Code.Infrastructure.States.StateInfrastructure;
using Code.Infrastructure.States.StateMachine;
using UnityEngine;

namespace Code.Infrastructure.States.GameStates
{
    public class GameEnterState : SimpleState
    {
        private readonly IGameStateMachine _stateMachine;
        private readonly IPlayerFactory _playerFactory;

        public GameEnterState(IGameStateMachine stateMachine, IPlayerFactory playerFactory)
        {
            _stateMachine = stateMachine;
            _playerFactory = playerFactory;
        }

        public override void Enter()
        {
            var player = _playerFactory.CreatePlayer(Vector3.zero);

            _stateMachine.Enter<GameLoopState>();
        }
    }
}