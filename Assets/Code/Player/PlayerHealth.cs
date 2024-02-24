using Code.Logic;
using Code.Logic.SurfaceId;
using Code.Services;
using Code.Services.StaticData;
using Code.StaticData.Player;
using Unity.Netcode;
using UnityEngine;

namespace Code.Player
{
    public class PlayerHealth : NetworkBehaviour, IHealth
    {
        [SerializeField] private PlayerIdKeeper _id;

        public SurfaceId SurfaceId => SurfaceId.Player;

        public NetworkVariable<float> MaxHealth { get; } = new();
        public NetworkVariable<float> CurrentHealth { get; } = new();

        public override void OnNetworkSpawn()
        {
            if (IsServer)
            {
                PlayerConfig config = AllServices.Container.Single<IStaticDataService>().ForPlayer(_id.Id);
                MaxHealth.Value = config.MaxHealth;
                CurrentHealth.Value = config.MaxHealth;
            }
        }

        [ServerRpc(RequireOwnership = false)]
        public void TakeDamageServerRpc(float value)
        {
            if (CurrentHealth.Value - value < 0)
                CurrentHealth.Value = 0;
            else
                CurrentHealth.Value -= value;
        }
    }
}