using Code.Common.Network;
using Code.Common.Transition;
using Code.Game.Features.Network;
using Code.Game.Features.Player.Factory;
using Code.Infrastructure.States.StateInfrastructure;
using Code.Infrastructure.States.StateMachine;
using Cysharp.Threading.Tasks;
using Unity.Collections;
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
            NetworkManager.Singleton.CustomMessagingManager.RegisterNamedMessageHandler(RequestTypes.CreatePlayerEntity.ToString(), CreatePlayerMessageHandler);

            NetworkManager.Singleton.SceneManager.OnSynchronizeComplete += SceneManager_OnSynchronizeComplete;
        }

        private void SceneManager_OnSynchronizeComplete(ulong clientId)
        {
            var totalClients = NetworkManager.Singleton.ConnectedClientsList.Count;

            if (totalClients < _networkSessionService.GetMaxPlayersCount()) 
            {
                Debug.Log($"totalClients {totalClients}");

                return;
            }

            if (NetworkManager.Singleton.IsHost)
            {
                for (int i = 0; i < totalClients; i++)
                {
                    var id = NetworkManager.Singleton.ConnectedClientsList[i].ClientId;

                    // Отправляем сообщение только этому клиенту
                    NetworkManager.Singleton.CustomMessagingManager.SendNamedMessage(
                        RequestTypes.CreatePlayerEntity.ToString(),
                        NetworkManager.Singleton.ConnectedClientsIds,
                        SerializePayload(id));

                    Debug.Log($"Отправляем запрос на создание игрока для ClientId: {id}");
                }
            }

            NetworkManager.Singleton.SceneManager.OnSynchronizeComplete -= SceneManager_OnSynchronizeComplete;

            _stateMachine.Enter<GameLoopState>();
            _transitionService.Execute(0).AsTask();
        }

        private void CreatePlayerMessageHandler(ulong senderClientId, FastBufferReader reader)
        {
            reader.ReadValueSafe(out ulong playerId);

            _playerFactory.CreatePlayer(playerId);
        }

        private FastBufferWriter SerializePayload(ulong value)
        {
            var totalSize = sizeof(ulong);

            using var writer = new FastBufferWriter(totalSize, Allocator.Temp);
            writer.WriteValueSafe(value);

            return writer;
        }

        protected override void Exit()
        {
            Debug.Log("Exiting GameEnterState");
            //NetworkManager.Singleton.CustomMessagingManager.UnregisterNamedMessageHandler(RequestTypes.CreatePlayerEntity.ToString());
        }
    }
}