using Code.Logic.Despawn;
using Code.StaticData.Enemy;
using Unity.Netcode;
using UnityEngine;

namespace Code.Services.Factories.Enemy
{
    public interface IEnemyFactory : IService
    {
        void CreateEnemy(EnemyId id, Vector2 position, ulong ownId);
        NetworkList<NetworkObjectReference> EnemyNetworkList { get; }
    }
}