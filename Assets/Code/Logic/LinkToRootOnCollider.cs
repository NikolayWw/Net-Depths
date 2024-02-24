using Unity.Netcode;
using UnityEngine;

namespace Code.Logic
{
    public class LinkToRootOnCollider : MonoBehaviour
    {
        [field: SerializeField] public NetworkObject Root { get; private set; }
    }
}