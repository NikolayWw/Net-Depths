using System;
using Unity.Netcode;
using UnityEngine;

namespace Code.StaticData.Player
{
    [Serializable]
    public class PlayerConfig : BaseConfig<PlayerId, PlayerIdKeeper, NetworkObject>
    {
        [field: SerializeField] public float MaxHealth { get; private set; } = 100;
    }
}