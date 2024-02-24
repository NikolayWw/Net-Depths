using System;
using Unity.Services.Lobbies.Models;

namespace Code.Services.Network
{
    public interface INetworkService : IService
    {
        bool GetJoinedLobby(out Lobby lobby);

        void RefreshLobbiesList();

        void StartGame();

        void StartClient(string joinLobbyId, Action onClientStarted = null);
    }
}