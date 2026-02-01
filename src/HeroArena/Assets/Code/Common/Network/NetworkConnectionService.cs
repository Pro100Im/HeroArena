using Unity.Netcode;

namespace Code.Common.Network
{
    public class NetworkConnectionService : INetworkConnectionService
    {
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
            NetworkManager.Singleton.StartHost();
        }

        public void StopHost()
        {
            throw new System.NotImplementedException();
        }
    }
}