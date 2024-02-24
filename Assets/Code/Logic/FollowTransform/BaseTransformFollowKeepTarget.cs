using Unity.Netcode;
using UnityEngine;

namespace Code.Logic.FollowTransform
{
    public abstract class BaseTransformFollowKeepTarget : NetworkBehaviour
    {
        [field: SerializeField] public Transform FollowTarget { get; private set; }
    }
}