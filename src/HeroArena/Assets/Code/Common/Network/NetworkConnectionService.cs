using DG.Tweening.Core.Easing;
using System;
using System.Threading;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Networking.Transport;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Multiplayer;
using UnityEngine;

namespace Code.Common.Network
{
    public class NetworkConnectionService : INetworkConnectionService
    {
        public ISession Session { get; private set; }
        public NetworkEndpoint ListenEndpoint { get; private set; }
        public NetworkEndpoint ConnectEndpoint { get; private set; }
        public NetworkType SessionConnectionType { get; private set; }

        public void Connect()
        {
            throw new System.NotImplementedException();
        }

        public void Disconnect()
        {
            throw new System.NotImplementedException();
        }

        public void StartHost()
        {
            JoinOrCreateMatchmakerGameAsync(new CancellationToken());
        }

        public void StopHost()
        {
            throw new System.NotImplementedException();
        }

        public async Task JoinOrCreateMatchmakerGameAsync(CancellationToken cancellationToken)
        {
            await StartServicesAsync();

            var sessionOptions = new SessionOptions()
            {
                MaxPlayers = 2
            }.WithRelayNetwork();

            var matchOptions = new MatchmakerOptions
            {
                QueueName = "TestArena",
            };

            Session = await MultiplayerService.Instance.MatchmakeSessionAsync(matchOptions, sessionOptions, cancellationToken);

            Debug.LogWarning("Connected!");
        }

        private async Task StartServicesAsync()
        {
            if (UnityServices.State != ServicesInitializationState.Initialized)
            {
                await UnityServices.InitializeAsync();
            }

            if (!AuthenticationService.Instance.IsAuthorized)
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            }
        }
    }
}