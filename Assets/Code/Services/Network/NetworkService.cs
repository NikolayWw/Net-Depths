using Code.Infrastructure.Logic;
using Code.Network.Lobby;
using Code.Network.Relay;
using Code.UI.Services.Factories.NetworkFactoryService;
using System;
using System.Collections;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
using UnityEngine;
using static Code.StaticData.Constants.NetworkConstants;
namespace Code.Services.Network
{
    public class NetworkService : INetworkService
    {
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly INetworkFactory _networkFactory;
        private readonly NetworkManager _networkManager;
        private readonly UnityTransport _unityTransport;

        private readonly LobbyStrategy _lobbyStrategy;
        private readonly RelayStrategy _relayStrategy;

        private readonly WaitForSeconds _waitToSendUpdateTLobby = new(2f);

        public Action OnLobbyUpdate;
        public Action OnStartClient;
        private IEnumerator _sendLobbyUpdateCoroutine;

        public NetworkService(ICoroutineRunner coroutineRunner, INetworkFactory networkFactory, NetworkManager networkManager)
        {
            _coroutineRunner = coroutineRunner;
            _networkFactory = networkFactory;
            _networkManager = networkManager;
            _unityTransport = networkManager.GetComponent<UnityTransport>();

            _lobbyStrategy = new LobbyStrategy(_coroutineRunner);
            _relayStrategy = new RelayStrategy();
        }


        public async void StartGame()
        {
            try
            {
                CreateRelayData relayData = await _relayStrategy.CreateRelay(4);
                _unityTransport.SetRelayServerData(relayData.RelayServerData);
                await CreateLobby(4);
                await _lobbyStrategy.SetStartGame(relayData.RelayCode);
                _networkManager.StartHost();
            }
            catch (RelayServiceException e)
            {
                Debug.Log(e);
            }
        }

        public async void StartClient(string joinLobbyId, Action onClientStarted = null)
        {
            await _lobbyStrategy.JoinLobby(joinLobbyId, () =>
            {
                try
                {
                    SetRelayAndStartClient();
                }
                catch (RelayServiceException e)
                {
                    Debug.LogError(e);
                }
            });
            return;

            async void SetRelayAndStartClient()
            {
                string relayJoinCode = _lobbyStrategy.JoinedLobby.Data[RelayStartGameKey].Value;
                CreateRelayData data = await _relayStrategy.JoinRelay(relayJoinCode);
                _unityTransport.SetRelayServerData(data.RelayServerData);
                _networkManager.StartClient();
                onClientStarted?.Invoke();
            }
        }

        public bool GetJoinedLobby(out Lobby lobby)
        {
            if (_lobbyStrategy.JoinedLobby != null)
            {
                lobby = _lobbyStrategy.JoinedLobby;
                return true;
            }

            lobby = null;
            return false;
        }

        public async void RefreshLobbiesList()
        {
            try
            {
                QueryResponse queryResponse = await LobbyService.Instance.QueryLobbiesAsync();
                _networkFactory.RefreshLobbiesList(queryResponse);
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
            }
        }

        private async Task CreateLobby(int maxPlayers)
        {
            await _lobbyStrategy.CreateLobby(maxPlayers);
            SendLobbyUpdate();
        }

        private async void SendLobbyUpdate()
        {
            if (_lobbyStrategy.JoinedLobby != null)
            {
                await _lobbyStrategy.GetJoinedLobby(() =>
                {
                    if (_lobbyStrategy.JoinedLobby != null)
                    {
                        OnLobbyUpdate?.Invoke();
                    }
                });
                _coroutineRunner?.StartCoroutine(_sendLobbyUpdateCoroutine = SendLobbyUpdateDelay());
            }
        }

        private IEnumerator SendLobbyUpdateDelay()
        {
            yield return _waitToSendUpdateTLobby;
            SendLobbyUpdate();
        }
    }
}