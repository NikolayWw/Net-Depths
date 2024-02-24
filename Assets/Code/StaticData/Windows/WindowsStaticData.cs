using Code.UI.Windows.Network;
using Unity.Netcode;
using UnityEngine;

namespace Code.StaticData.Windows
{
    [CreateAssetMenu(menuName = "Static Data/Windows Static Data", order = 0)]
    public class WindowsStaticData : ScriptableObject
    {
        [field: SerializeField] public GameObject UIRootPrefab { get; private set; }
        [field: SerializeField] public NetworkObject HealthWindowPrefab { get; private set; }
        [field: SerializeField] public FindLobbyWindow FindLobbyWindowPrefab { get; private set; }
        [field: SerializeField] public LobbyField LobbyFieldPrefab { get; private set; }
    }
}