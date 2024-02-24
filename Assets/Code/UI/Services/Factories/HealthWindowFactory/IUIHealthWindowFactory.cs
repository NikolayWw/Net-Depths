using Code.Logic;
using Code.Logic.Despawn;
using Code.Logic.FollowTransform;
using Code.Services;
using Unity.Netcode;

namespace Code.UI.Services.Factories.HealthWindowFactory
{
    public interface IUIHealthWindowFactory : IService
    {
        NetworkList<NetworkObjectReference> WindowsNetworkList { get; }

        void CreateHealthWindow(ulong ownId, IHealth characterHealth, HealthWindowFollowTarget followKeepTarget, DespawnReporter despawnReporter = null);
    }
}