using System;
using Unity.Netcode;
using UnityEngine;

namespace Code.Services.Factories.Weapon
{
    public class WeaponCreateReporter : NetworkBehaviour
    {
        private Transform _parent;
        public NetworkObject WeaponInstance { get; private set; }
        public Action OnCreated;

        public override void OnNetworkSpawn()
        {
            if (IsServer)
                Initialize();
        }

        public void Report(NetworkObject weaponInstance)
        {
            WeaponInstance = weaponInstance;
            OnCreated?.Invoke();
        }

        private void Initialize()
        {
            _parent = transform.parent;
            NetworkObject networkObject = gameObject.AddComponent<NetworkObject>();
            networkObject.Spawn();
            transform.parent = _parent;
        }
    }
}