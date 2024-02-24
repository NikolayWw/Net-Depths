using Code.Item.StateMachine;
using Code.Logic.SurfaceId;
using Code.Services;
using Code.Services.InputService;
using Code.StaticData.Weapon;
using Unity.Netcode;
using UnityEngine;
using Input = UnityEngine.Input;

namespace Code.Player
{
    public class PlayerShowItemInHand : NetworkBehaviour
    {
        [SerializeField] private ItemInHandStateMachine _itemStateMachine;
        [SerializeField] private PlayerHealth _health;

        private IInput _input;
        private bool _entered;

        public override void OnNetworkSpawn()
        {
            if (!IsOwner)
                return;

            _input = AllServices.Container.Single<IInput>();
        }

        private void Update()
        {
            if (!IsOwner)
                return;

            if (Input.GetKeyDown(KeyCode.Alpha1))
                ShowOrHideWeapon();
        }

        private void ShowOrHideWeapon()
        {
            if (_entered == false)
            {
                _itemStateMachine.EnterWeaponState(_input, WeaponId.Topor, OwnerClientId, _health, SurfaceId.Player);
                _entered = true;
            }
            else
            {
                _itemStateMachine.EnterLoopState();
                _entered = false;
            }
        }
    }
}