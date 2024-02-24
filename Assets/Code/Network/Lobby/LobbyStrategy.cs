using Code.Infrastructure.Logic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using static Code.StaticData.Constants.NetworkConstants;
using Random = UnityEngine.Random;

namespace Code.Network.Lobby
{
    public class LobbyStrategy
    {
        private readonly ICoroutineRunner _coroutineRunner;

        private string PlayerName => $"Nik : {Random.Range(80, 100)}";
        public Unity.Services.Lobbies.Models.Lobby JoinedLobby { get; private set; }
        private IEnumerator _sendHeartbeatCoroutine;

        public LobbyStrategy(ICoroutineRunner coroutineRunner)
        {
            _coroutineRunner = coroutineRunner;
        }

        public async Task CreateLobby(int maxPlayers)
        {
            try
            {
                CreateLobbyOptions options = new() { Player = GetPlayer() };
                JoinedLobby = await LobbyService.Instance.CreateLobbyAsync("Name is: Pro Lobby", maxPlayers, options);

                _coroutineRunner.StartCoroutine(_sendHeartbeatCoroutine = SendHeartbeatTimer());
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
            }
        }

        public async Task JoinLobby(string lobbyId, Action onCompleted = null)
        {
            JoinLobbyByIdOptions options = new() { Player = GetPlayer() };
            try
            {
                JoinedLobby = await Lobbies.Instance.JoinLobbyByIdAsync(lobbyId, options);
                onCompleted?.Invoke();
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
            }
        }

        public async Task SetStartGame(string relayStartGameKey)
        {
            if (JoinedLobby == null)
                return;

            try
            {
                await StartG();
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
            }

            return;
            async Task StartG()
            {
                UpdateLobbyOptions lobbyOptions = new()
                {
                    Data = new Dictionary<string, DataObject>
                    {
                        {RelayStartGameKey, new DataObject(DataObject.VisibilityOptions.Member, relayStartGameKey)}
                    }
                };
                Unity.Services.Lobbies.Models.Lobby updateLobby = await Lobbies.Instance.UpdateLobbyAsync(JoinedLobby.Id, lobbyOptions);
                JoinedLobby = updateLobby;
            }
        }

        public async Task LeaveJoinedLobby()
        {
            if (JoinedLobby == null)
            {
                Debug.Log("No connected lobby");
                return;
            }

            await LobbyService.Instance.RemovePlayerAsync(JoinedLobby.Id, AuthenticationService.Instance.PlayerId);
            JoinedLobby = null;
            if (_sendHeartbeatCoroutine != null)
                _coroutineRunner?.StopCoroutine(_sendHeartbeatCoroutine);
        }

        public async Task GetJoinedLobby(Action onGetJoinedLobby = null)
        {
            if (JoinedLobby == null)
                return;
            try
            {
                JoinedLobby = await LobbyService.Instance.GetLobbyAsync(JoinedLobby.Id);
                onGetJoinedLobby?.Invoke();
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
            }
        }

        private Unity.Services.Lobbies.Models.Player GetPlayer()
        {
            return new Unity.Services.Lobbies.Models.Player
            {
                Data = new Dictionary<string, PlayerDataObject>
                {
                    {PlayerNameKey, new PlayerDataObject(PlayerDataObject.VisibilityOptions.Public, PlayerName)}
                }
            };
        }

        private static async void SendHeartbeatPingAsync(string id)
        {
            try
            {
                await LobbyService.Instance.SendHeartbeatPingAsync(id);
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
            }
        }

        private IEnumerator SendHeartbeatTimer()
        {
            WaitForSeconds wait = new(20);
            while (true)
            {
                yield return wait;
                SendHeartbeatPingAsync(JoinedLobby.Id);
            }
        }
    }
}