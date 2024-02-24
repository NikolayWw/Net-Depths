using Code.Logic;
using Code.Logic.Despawn;
using Code.Logic.FollowTransform;
using Code.Services;
using Code.Services.StaticData;
using Code.UI.Windows.Health;
using Unity.Netcode;

namespace Code.UI.Services.Factories.HealthWindowFactory
{
    public class UIHealthWindowFactory : NetworkBehaviour, IUIHealthWindowFactory
    {
        private AllServices _services;
        public NetworkList<NetworkObjectReference> WindowsNetworkList { get; } = new();

        public void Construct(AllServices services)
        {
            _services = services;
        }

        public void CreateHealthWindow(ulong ownId, IHealth characterHealth, HealthWindowFollowTarget followKeepTarget, DespawnReporter despawnReporter = null)
        {
            NetworkBehaviourReference reporter = despawnReporter != null ? new NetworkBehaviourReference(despawnReporter) : new NetworkBehaviourReference();
            CreateHealthWindowServerRpc(ownId, characterHealth.NetworkObject, followKeepTarget.NetworkObject, reporter);
        }

        [ServerRpc(RequireOwnership = false)]
        private void CreateHealthWindowServerRpc(ulong ownId, NetworkObjectReference takeDamageReference, NetworkObjectReference followTargetReference, NetworkBehaviourReference despawnReporterReference)
        {
            IStaticDataService staticDataService = GetService<IStaticDataService>();
            NetworkObject prefab = staticDataService.WindowsData.HealthWindowPrefab;
            NetworkObject instance = Instantiate(prefab);
            instance.SpawnWithOwnership(ownId);
            WindowsNetworkList.Add(instance);

            HealthWindow healthWindow = instance.GetComponent<HealthWindow>();
            healthWindow.TakeDamageVariable.Value = takeDamageReference;
            instance.GetComponent<TransformFollow>().TargetNetworkVariable.Value = followTargetReference;

            InvokeWindowClientRpc(takeDamageReference, followTargetReference, instance, despawnReporterReference);
        }

        [ClientRpc]
        private void InvokeWindowClientRpc(NetworkObjectReference healthReference, NetworkObjectReference targetReference, NetworkObjectReference healthWindowReference, NetworkBehaviourReference despawnReporterReference)
        {
            healthReference.TryGet(out NetworkObject healthNetwork);
            healthWindowReference.TryGet(out NetworkObject windowNetwork);
            targetReference.TryGet(out NetworkObject targetNetwork);
            HealthWindow healthWindow = windowNetwork.GetComponent<HealthWindow>();

            windowNetwork.GetComponent<TransformFollow>().SetTarget(targetNetwork.GetComponent<HealthWindowFollowTarget>().FollowTarget);
            healthWindow.Construct(healthNetwork.GetComponent<IHealth>());

            if (despawnReporterReference.TryGet(out NetworkBehaviour despawnReporterNetwork))
                healthWindow.ReadDespawn(despawnReporterNetwork as DespawnReporter);
        }

        private TService GetService<TService>() where TService : IService =>
            _services.Single<TService>();
    }
}