using Code.Services.Network;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.Windows.Network
{
    public class FindLobbyWindow : MonoBehaviour
    {
        [field: SerializeField] public Transform LobbiesRoot { get; private set; }
        [SerializeField] private Button _refreshButton;
        [SerializeField] private Button _startButton;
        private INetworkService _networkService;

        public void Construct(INetworkService networkService)
        {
            _networkService = networkService;
        }

        private void Start()
        {
            _startButton.onClick.AddListener(() => _networkService.StartGame());
            _refreshButton.onClick.AddListener(() => _networkService.RefreshLobbiesList());
        }
    }
}