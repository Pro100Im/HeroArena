using Code.Infrastructure.Loading;
using System.Threading;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Multiplayer;
using UnityEngine;

namespace Code.Common.Network
{
    public class NetworkConnectionService : INetworkConnectionService
    {
        private ISceneLoader _sceneLoader;
        private CancellationTokenSource _cancellationTokenSource;

        public NetworkConnectionService(ISceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
        }

        public void QuickMatch()
        {
            if(_cancellationTokenSource != null)
            {
                _cancellationTokenSource.Cancel();
                _cancellationTokenSource.Dispose();
                _cancellationTokenSource = null;
            }

            _cancellationTokenSource = new CancellationTokenSource();

            JoinOrCreateMatchmakerGameAsync(_cancellationTokenSource.Token);
        }

        public void CancelSerching()
        {
            _cancellationTokenSource?.Cancel();
        }

        public async void JoinOrCreateMatchmakerGameAsync(CancellationToken cancellationToken)
        {
            NetworkManager.Singleton.OnClientStarted += () =>
            {
                Debug.Log("client started");
            };

            NetworkManager.Singleton.OnServerStarted += () =>
            {
                Debug.Log("server started");
                _sceneLoader.NetworkLoad("Game");
            };

            await StartServicesAsync();

            var sessionOptions = new SessionOptions()
            {
                MaxPlayers = 2
            }.WithRelayNetwork();

            var matchOptions = new MatchmakerOptions
            {
                QueueName = "TestArena",
            };

            await MultiplayerService.Instance.MatchmakeSessionAsync(matchOptions, sessionOptions, cancellationToken);

            Debug.LogWarning("Connected!");
        }

        private async Task StartServicesAsync()
        {
            if (UnityServices.State != ServicesInitializationState.Initialized)
                await UnityServices.InitializeAsync();

            if (!AuthenticationService.Instance.IsAuthorized)
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
    }
}