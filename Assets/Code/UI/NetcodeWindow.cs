using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI
{
    public class NetcodeWindow : MonoBehaviour
    {
        [SerializeField] private Button _host, _clientButton;

        private void Start()
        {
            _host.onClick.AddListener(() => NetworkManager.Singleton.StartHost());
            _clientButton.onClick.AddListener(() => NetworkManager.Singleton.StartClient());
        }
    }
}