using Code.Services;
using Code.Services.Network;
using Code.Services.StaticData;
using Code.UI.Services.Factories.UIFactoryService;
using Code.UI.Windows.Network;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using static Code.StaticData.Constants.NetworkConstants;

namespace Code.UI.Services.Factories.NetworkFactoryService
{
    public class NetworkFactory : INetworkFactory
    {
        private readonly AllServices _services;
        private readonly List<LobbyField> _currentFields = new();

        private FindLobbyWindow _lobbyWindow;

        public NetworkManager NetworkManager { get; private set; }
        public UnityTransport UnityTransport { get; private set; }

        public NetworkFactory(AllServices services, NetworkManager networkManager)
        {
            _services = services;
            NetworkManager = networkManager;
            UnityTransport = networkManager.GetComponent<UnityTransport>();
        }

        public void CreateFindLobbiesWindow()
        {
            IStaticDataService staticDataService = GetService<IStaticDataService>();
            INetworkService networkService = GetService<INetworkService>();
            IUIFactory uiFactory = GetService<IUIFactory>();
            FindLobbyWindow prefab = staticDataService.WindowsData.FindLobbyWindowPrefab;

            FindLobbyWindow window = Object.Instantiate(prefab, uiFactory.UIRoot);
            window.Construct(networkService);
            _lobbyWindow = window;
        }

        public void RefreshLobbiesList(QueryResponse queryResponse)
        {
            IStaticDataService staticDataService = GetService<IStaticDataService>();
            INetworkService networkService = GetService<INetworkService>();
            LobbyField lobbyFieldPrefab = staticDataService.WindowsData.LobbyFieldPrefab;

            foreach (LobbyField field in _currentFields.Where(field => field != null))
                field.Close();
            _currentFields.Clear();

            StringBuilder sb = new();
            foreach (Lobby lobby in queryResponse.Results)
            {
                sb.AppendLine(lobby.Name);
                sb.AppendLine($"Players: {lobby.Players.Count}/{lobby.MaxPlayers}");
                lobby.Players.ForEach(player => sb.AppendLine(player.Data[PlayerNameKey].Value));

                LobbyField field = Object.Instantiate(lobbyFieldPrefab, _lobbyWindow.LobbiesRoot);
                field.Construct(sb.ToString(), lobby.Id, networkService);
                sb.Clear();
                _currentFields.Add(field);
            }
        }

        private TService GetService<TService>() where TService : IService =>
            _services.Single<TService>();
    }
}