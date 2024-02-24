using Code.Character;
using Code.Item.StateMachine;
using Code.StaticData.Weapon;
using Unity.Netcode;
using UnityEngine;

namespace Code.Enemy
{
    public class EnemyBrain : NetworkBehaviour
    {
        [SerializeField] private EnemyFindTarget _findTarget;
        [SerializeField] private CharacterMove _move;
        [SerializeField] private EnemyHealth _health;
        [SerializeField] private EnemyInput _enemyInput;
        [SerializeField] private ItemInHandStateMachine _itemInHandState;

        private void Start()
        {
            if (IsServer == false)
                return;
            _health.OnHappened += OnHappened;
            _itemInHandState.EnterWeaponState(_enemyInput, WeaponId.Topor, OwnerClientId, _health);
        }

        private void Update()
        {
            if (IsServer == false)
                return;

            if (_findTarget.TargetTransform == null)
                return;

            float distance = Vector2.Distance(_findTarget.TargetTransform.position, transform.position);
            if (distance < 2f)
            {
                _enemyInput.SetAttack(true);
                _move.UpdateMove(Vector2.zero);
            }
            else
            {
                Vector2 direction = (_findTarget.TargetTransform.position - transform.position).normalized;
                _enemyInput.SetAttack(false);
                _move.UpdateMove(direction);
            }
        }

        private void OnHappened()
        {
            _itemInHandState.EnterLoopState();
            DespawnThisServerRpc();
        }

        [ServerRpc]
        private void DespawnThisServerRpc()
        {
            NetworkObject.Despawn();
        }
    }
}