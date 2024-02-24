using Code.Logic.FollowTransform;
using Code.Player;
using Code.Services.StaticData;
using Code.StaticData.Player;
using Code.UI.Services.Factories.HealthWindowFactory;
using Unity.Netcode;

namespace Code.Services.Factories.Player
{
    public class PlayerFactory : NetworkBehaviour, IPlayerFactory
    {
        private AllServices _services;

        public void Construct(AllServices services)
        {
            _services = services;
        }

        [ServerRpc(RequireOwnership = false)]
        public void CreatePlayerServerRpc(ulong ownId, PlayerId id)
        {
            IStaticDataService staticDataService = GetService<IStaticDataService>();
            IUIHealthWindowFactory healthWindowFactory = GetService<IUIHealthWindowFactory>();
            NetworkObject prefab = staticDataService.ForPlayer(id).Prefab;

            NetworkObject instantiate = Instantiate(prefab);
            instantiate.SpawnWithOwnership(ownId);
            HealthWindowFollowTarget transformFollowKeeper = instantiate.GetComponent<HealthWindowFollowTarget>();
            healthWindowFactory.CreateHealthWindow(ownId, instantiate.GetComponent<PlayerHealth>(), transformFollowKeeper);
        }

        private TService GetService<TService>() where TService : IService =>
            _services.Single<TService>();
    }
}