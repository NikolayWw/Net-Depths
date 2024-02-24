using System;
using Unity.Netcode;

namespace Code.Logic.Despawn
{
    public class DespawnReporter : NetworkBehaviour
    {
        public Action OnDespawn;

        public override void OnNetworkDespawn()
        {
            OnDespawn?.Invoke();
        }
    }
}