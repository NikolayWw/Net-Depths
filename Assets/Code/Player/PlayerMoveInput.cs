using Code.Character;
using Code.Services;
using Code.Services.InputService;
using Unity.Netcode;
using UnityEngine;

namespace Code.Player
{
    public class PlayerMoveInput : NetworkBehaviour
    {
        [SerializeField] private CharacterMove _move;
        private IInput _input;

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

            _move.UpdateMove(_input.MoveAxis);
        }
    }
}