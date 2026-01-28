using Code.Infrastructure.States.Factory;
using Code.Infrastructure.States.StateInfrastructure;
using UnityEngine;

namespace Code.Infrastructure.States.StateMachine
{
    public class GameStateMachine : IGameStateMachine
    {
        private readonly IStateFactory _stateFactory;
        private IExitableState _activeState;

        public GameStateMachine(IStateFactory stateFactory)
        {
            _stateFactory = stateFactory;
        }

        public void Update()
        {
            if(_activeState is IUpdateable updateableState)
                updateableState.Update();
        }

        public void Enter<TState>() where TState : class, IState
        {
            IState state = ChangeState<TState>();

            state.Enter();
        }

        public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadState<TPayload>
        {
            IPayloadState<TPayload> state = ChangeState<IPayloadState<TPayload>>();

            state.Enter(payload);
        }

        private TState ChangeState<TState>() where TState : class, IExitableState
        {
            if(_stateFactory is null)
            {
                Debug.Log("_stateFactory is null");
            }

            _activeState?.Exit();

            TState state = _stateFactory.GetState<TState>();
            _activeState = state;

            return state;
        }
    }
}