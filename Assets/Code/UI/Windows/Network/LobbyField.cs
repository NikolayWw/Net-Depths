using Code.Services.Network;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.Windows.Network
{
    public class LobbyField : MonoBehaviour
    {
        [SerializeField] private TMP_Text _infoText;
        [SerializeField] private Button _joinButton;

        private string _joinedLobbyId = string.Empty;
        private INetworkService _networkService;

        public void Construct(string context, string lobbyId, INetworkService networkService)
        {
            _infoText.text = context;
            _joinedLobbyId = lobbyId;
            _networkService = networkService;
            _joinButton.onClick.AddListener(Join);
        }

        public void Close()
        {
            Destroy(gameObject);
        }

        private void Join()
        {
            _networkService.StartClient(_joinedLobbyId);
        }
    }
}