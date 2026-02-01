using UnityEngine;

namespace Code.Common.Network
{
    public interface INetworkConnectionService
    {
        public void StartHost();
        public void StopHost();

        public void Connect();
        public void Disconnect();
    }
}