using Code.Logic;
using Code.Logic.SurfaceId;
using Code.Services;
using Code.Services.StaticData;
using Code.StaticData.Enemy;
using System;
using Unity.Netcode;
using UnityEngine;

namespace Code.Enemy
{
    public class EnemyHealth : NetworkBehaviour, IHealth
    {
        [SerializeField] private EnemyIdKeeper _id;

        public SurfaceId SurfaceId => SurfaceId.Enemy;
        public NetworkVariable<float> MaxHealth { get; } = new();
        public NetworkVariable<float> CurrentHealth { get; } = new();
        public Action OnHappened;
        private bool _happened;

        public override void OnNetworkSpawn()
        {
            if (IsServer)
            {
                EnemyConfig config = AllServices.Container.Single<IStaticDataService>().ForEnemy(_id.Id);
                MaxHealth.Value = config.MaxHealth;
                CurrentHealth.Value = config.MaxHealth;
            }
        }

        [ServerRpc(RequireOwnership = false)]
        public void TakeDamageServerRpc(float value)
        {
            if (_happened)
                return;

            if (CurrentHealth.Value - value <= 0)
            {
                CurrentHealth.Value = 0;
                _happened = true;
                OnHappened?.Invoke();
            }
            else
                CurrentHealth.Value -= value;
        }
    }
}