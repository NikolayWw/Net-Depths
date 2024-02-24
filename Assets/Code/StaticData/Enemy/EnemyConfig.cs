using Code.StaticData.Weapon;
using System;
using Code.Logic.SurfaceId;
using Unity.Netcode;
using UnityEngine;

namespace Code.StaticData.Enemy
{
    [Serializable]
    public class EnemyConfig : BaseConfig<EnemyId, EnemyIdKeeper, NetworkObject>
    {
        [field: SerializeField] public float MaxHealth { get; private set; } = 10;
        [field: SerializeField] public WeaponId WeaponId { get; private set; }
        [field: SerializeField] public SurfaceId[] IgnoreTargets { get; private set; }
    }
}