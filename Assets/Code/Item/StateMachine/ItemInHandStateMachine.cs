using Code.Item.StateMachine.States;
using Code.Item.Weapon;
using Code.Logic;
using Code.Logic.SurfaceId;
using Code.Services.InputService;
using Code.StaticData.Weapon;
using UnityEngine;

namespace Code.Item.StateMachine
{
    public class ItemInHandStateMachine : MonoBehaviour
    {
        [SerializeField] private WeaponAttackState _weaponAttackState;
        private readonly InHandLoopState _loopState = new();
        private IInHandExitState _activeState = new InHandLoopState();

        public void EnterWeaponState(IInput input, WeaponId id, ulong ownId, IHealth ignoreCharacterHealth, params SurfaceId[] ignoreTargets)
        {
            _activeState.Exit();
            _activeState = _weaponAttackState;
            _weaponAttackState.Enter(input, id, ownId, ignoreCharacterHealth, ignoreTargets);
        }

        public void EnterLoopState()
        {
            _activeState.Exit();
            _activeState = _loopState;
        }
    }
}