using Code.Character;
using Code.Item.Weapon;
using Code.Logic.FollowTransform;
using Code.Services;
using Code.Services.Factories.Weapon;
using Code.StaticData.Weapon;
using System;
using Unity.Netcode;
using UnityEngine;

namespace Code.Enemy
{
    public class EnemyInventory : MonoBehaviour
    {
        [SerializeField] private ItemInHandFollowTarget _followTarget;
        [SerializeField] private NetworkObject _networkObject;
        [SerializeField] private CharacterAnimator _animator;
        public WeaponAttackState WeaponAttackState { get; private set; }

        private void Start()
        {
            IWeaponFactory weaponFactory = AllServices.Container.Single<IWeaponFactory>();
            NetworkObject weaponNetworkInstance = weaponFactory.CreateWeapon(_networkObject.OwnerClientId, WeaponId.Topor, _followTarget, _animator);
            WeaponAttackState = weaponNetworkInstance.GetComponent<WeaponAttackState>();
        }
    }
}