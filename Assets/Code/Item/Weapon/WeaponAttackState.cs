using Code.Character;
using Code.Item.StateMachine;
using Code.Logic;
using Code.Logic.FollowTransform;
using Code.Logic.SurfaceId;
using Code.Services;
using Code.Services.Factories.Weapon;
using Code.Services.InputService;
using Code.Services.StaticData;
using Code.StaticData.Weapon;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Code.Item.Weapon
{
    public class WeaponAttackState : NetworkBehaviour, IInHandExitState
    {
        [SerializeField] private ItemInHandFollowTarget _transformFollowKeepTarget;
        [SerializeField] private CharacterAnimator _characterAnimator;
        [SerializeField] private SurfaceId[] _ignoreTargets;
        [SerializeField] private Transform _pointAttack;

        private readonly Dictionary<SurfaceId, int> _ignoreTargetsDi = new();
        private readonly Collider2D[] _targetColliders = new Collider2D[15];
        private readonly WaitForSeconds _waitForApplyDamage = new(0.5f);
        private IWeaponFactory _weaponFactory;
        private IInput _input;
        private IHealth _ignoreCharacterHealth;
        private IEnumerator _attackTimer;

        private WeaponConfig _config;
        private NetworkObject _weaponInstance;

        private float _currentDelayBetweenAttack;
        private IStaticDataService _dataService;
        private IEnumerator _updateEnumerator;

        public override void OnNetworkSpawn()
        {
            if (IsServer)
            {
                _weaponFactory = AllServices.Container.Single<IWeaponFactory>();
            }

            if (IsOwner == false)
                return;

            _dataService = AllServices.Container.Single<IStaticDataService>();
        }

        public void OnStart()
        { }

        public void Enter(IInput input, WeaponId id, ulong ownId, IHealth ignoreCharacterHealth, params SurfaceId[] ignoreTargets)
        {
            _input = input;
            _ignoreTargets = ignoreTargets;
            _ignoreCharacterHealth = ignoreCharacterHealth;
            _config = _dataService.ForWeapon(id);
            CreateWeaponServerRpc(id, ownId, _transformFollowKeepTarget.NetworkObject, ignoreTargets);
            StartCoroutine(_updateEnumerator = UpdateNumerator());
        }

        public void Exit()
        {
            if (_weaponInstance != null)
                CloseWeaponServerRpc(_weaponInstance);

            StopCoroutine(_updateEnumerator);
        }

        private IEnumerator UpdateNumerator()
        {
            while (true)
            {
                _currentDelayBetweenAttack += Time.deltaTime;
                if (_input.IsAttack)
                    UpdateAttack();

                yield return null;
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void CloseWeaponServerRpc(NetworkObjectReference weaponReference)
        {
            _weaponFactory.HideWeapon(weaponReference);
        }

        private void UpdateAttack()
        {
            if (_currentDelayBetweenAttack < _config.DelayBetweenAttack)
                return;

            _currentDelayBetweenAttack = 0;
            _characterAnimator.PlayAttack();

            if (_attackTimer != null)
                StopCoroutine(_attackTimer);
            StartCoroutine(_attackTimer = AttackTimer());
        }

        private IEnumerator AttackTimer()
        {
            yield return _waitForApplyDamage;
            ApplyDamage();
        }

        private void ApplyDamage()
        {
            int count = Physics2D.OverlapCircleNonAlloc(_pointAttack.position, _config.Radius, _targetColliders);
            for (int i = 0; i < count; i++)
            {
                IHealth health = GetTarget(_targetColliders[i]);
                if (health == null)
                    continue;

                health.TakeDamageServerRpc(_config.Damage);
                break;
            }
            return;

            IHealth GetTarget(Collider2D foundCollider)
            {
                if (foundCollider.TryGetComponent(out LinkToRootOnCollider linkToRootOnCollider)
                    && linkToRootOnCollider.Root.TryGetComponent(out IHealth health)
                    && health != _ignoreCharacterHealth
                    && _ignoreTargetsDi.TryGetValue(health.SurfaceId, out _) == false)
                {
                    return health;
                }

                return null;
            }
        }

        [ServerRpc]
        private void CreateWeaponServerRpc(WeaponId id, ulong ownId, NetworkObjectReference followTarget, params SurfaceId[] ignoreTargets)
        {
            followTarget.TryGet(out NetworkObject networkFollowTarget);
            ItemInHandFollowTarget itemInHandFollowTarget = networkFollowTarget.GetComponent<ItemInHandFollowTarget>();
            NetworkObject weaponInstance = _weaponFactory.CreateWeapon(ownId, id, itemInHandFollowTarget, _characterAnimator, ignoreTargets);
            InvokeCreatedWeaponClientRpc(weaponInstance);
        }

        [ClientRpc]
        private void InvokeCreatedWeaponClientRpc(NetworkObjectReference weaponReference)
        {
            if (_weaponInstance != null)
                CloseWeaponServerRpc(_weaponInstance);

            _weaponInstance = weaponReference;
        }

        private void OnDrawGizmos()
        {
            if (_config == null)
                return;
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(_pointAttack.position, _config.Radius);
        }
    }
}