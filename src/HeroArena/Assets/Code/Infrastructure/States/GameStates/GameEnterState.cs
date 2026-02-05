using Code.Common.Network;
using Code.Common.Transition;
using Code.Game.Features.Player.Factory;
using Code.Infrastructure.States.StateInfrastructure;
using Code.Infrastructure.States.StateMachine;
using Cysharp.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace Code.Infrastructure.States.GameStates
{
    public class GameEnterState : SimpleState
    {
        private readonly IGameStateMachine _stateMachine;
        private readonly IPlayerFactory _playerFactory;
        private readonly INetworkSessionService _networkSessionService;

        private readonly TransitionService _transitionService;

        public GameEnterState(IGameStateMachine stateMachine, IPlayerFactory playerFactory, INetworkSessionService networkSessionService, TransitionService transitionService)
        {
            _stateMachine = stateMachine;
            _playerFactory = playerFactory;
            _networkSessionService = networkSessionService;
            _transitionService = transitionService;
        }

        public override void Enter()
        {
            NetworkManager.Singleton.SceneManager.OnSynchronizeComplete += SceneManager_OnSynchronizeComplete;
        }

        private void SceneManager_OnSynchronizeComplete(ulong clientId)
        {
            var totalClients = NetworkManager.Singleton.ConnectedClientsIds.Count;

            //foreach (var kvp in NetworkManager.Singleton.SpawnManager.SpawnedObjects)
            //{
            //    if (kvp.Value.IsPlayerObject)
            //        Debug.Log($"Игрок → ClientId: {kvp.Value.OwnerClientId},  NetId: {kvp.Value.NetworkObjectId}");
            //}

            if(totalClients < _networkSessionService.GetMaxPlayersCount())
            {
                Debug.Log($"totalClients {totalClients}");

                return;
            }

            NetworkManager.Singleton.SceneManager.OnSynchronizeComplete -= SceneManager_OnSynchronizeComplete;

            //var player = _playerFactory.CreatePlayer(Vector3.zero);

            _stateMachine.Enter<GameLoopState>();
            _transitionService.Execute(0).AsTask();
        }
    }
}