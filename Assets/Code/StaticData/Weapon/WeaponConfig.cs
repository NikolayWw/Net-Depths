using System;
using Unity.Netcode;
using UnityEngine;

namespace Code.StaticData.Weapon
{
    [Serializable]
    public class WeaponConfig : BaseConfig<WeaponId, WeaponIdKeeper, NetworkObject>
    {
        [field: SerializeField] public float Damage { get; private set; } = 5;
        [field: SerializeField] public float Radius { get; private set; } = 1;
        [field: SerializeField] public float DelayBetweenAttack { get; private set; } = 0.5f;
        [field: SerializeField] public float DelayApplyDamage { get; private set; } = 0.2f;
    }
}