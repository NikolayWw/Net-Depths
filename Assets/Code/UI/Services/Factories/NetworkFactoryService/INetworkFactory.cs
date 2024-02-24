using Code.Services;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Lobbies.Models;

namespace Code.UI.Services.Factories.NetworkFactoryService
{
    public interface INetworkFactory : IService
    {
        NetworkManager NetworkManager { get; }
        UnityTransport UnityTransport { get; }
        void CreateFindLobbiesWindow();
        void RefreshLobbiesList(QueryResponse queryResponse);
    }
}