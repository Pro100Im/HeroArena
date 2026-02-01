using Code.Common.Transition;
using Code.Infrastructure.States.GameStates;
using Code.Infrastructure.States.StateMachine;
using Cysharp.Threading.Tasks;
using VContainer.Unity;

namespace Code.Infrastructure.DI.EntryPoints
{
    public class GameWorld : ITickable, IInitializable
    {
        private IGameStateMachine _gameStateMachine;
        private TransitionService _transitionService;

        public GameWorld(IGameStateMachine gameStateMachine, TransitionService transitionService)
        {
            _gameStateMachine = gameStateMachine;
            _transitionService = transitionService;
        }

        public void Initialize()
        {
            _transitionService.Execute(0).AsTask();
            _gameStateMachine.Enter<GameEnterState>();
        }

        public void Tick()
        {
            _gameStateMachine.Update();
        }
    }
}