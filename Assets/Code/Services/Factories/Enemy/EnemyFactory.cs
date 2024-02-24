using Code.Enemy;
using Code.Logic.Despawn;
using Code.Logic.FollowTransform;
using Code.Services.Factories.Weapon;
using Code.Services.StaticData;
using Code.StaticData.Enemy;
using Code.UI.Services.Factories.HealthWindowFactory;
using Unity.Netcode;
using UnityEngine;

namespace Code.Services.Factories.Enemy
{
    public class EnemyFactory : NetworkBehaviour, IEnemyFactory
    {
        private AllServices _services;

        public NetworkList<NetworkObjectReference> EnemyNetworkList { get; } = new();

        public void Construct(AllServices services)
        {
            _services = services;
        }

        public void CreateEnemy(EnemyId id, Vector2 position, ulong ownId)
        {
            CreateEnemyServerRpc(id, position, ownId);
        }

        [ServerRpc]
        private void CreateEnemyServerRpc(EnemyId id, Vector2 position, ulong ownId)
        {
            IStaticDataService dataService = GetService<IStaticDataService>();
            IWeaponFactory weaponFactory = GetService<IWeaponFactory>();
            IUIHealthWindowFactory healthWindowFactory = GetService<IUIHealthWindowFactory>();
            EnemyConfig config = dataService.ForEnemy(id);

            NetworkObject enemyInstantiate = Instantiate(config.Prefab, position, Quaternion.identity);
            enemyInstantiate.Spawn();
            EnemyNetworkList.Add(enemyInstantiate);

            EnemyHealth enemyHealth = enemyInstantiate.GetComponent<EnemyHealth>();
            HealthWindowFollowTarget followKeepTarget = enemyInstantiate.GetComponent<HealthWindowFollowTarget>();
            healthWindowFactory.CreateHealthWindow(ownId, enemyHealth, followKeepTarget, enemyInstantiate.GetComponent<DespawnReporter>());
        }

        private TService GetService<TService>() where TService : IService =>
            _services.Single<TService>();
    }
}