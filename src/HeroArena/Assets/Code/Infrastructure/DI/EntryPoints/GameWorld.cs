using Code.Infrastructure.States.GameStates;
using Code.Infrastructure.States.StateMachine;
using UnityEngine;
using VContainer.Unity;

namespace Code.Infrastructure.DI.EntryPoints
{
    public class GameWorld : ITickable, IInitializable
    {
        private IGameStateMachine _gameStateMachine;

        public GameWorld(IGameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
        }

        public void Initialize()
        {
            Debug.LogWarning("Initialize()");
            _gameStateMachine.Enter<BootstrapState>();
        }

        public void Tick()
        {
            _gameStateMachine.Update();
        }
    }
}