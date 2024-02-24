using Code.Character;
using Code.Logic.FollowTransform;
using Code.Logic.SurfaceId;
using Code.Services.StaticData;
using Code.StaticData.Weapon;
using Unity.Netcode;
using UnityEngine;

namespace Code.Services.Factories.Weapon
{
    public class WeaponFactory : NetworkBehaviour, IWeaponFactory
    {
        private AllServices _services;
        public NetworkList<NetworkObjectReference> WeaponsNetworkList { get; } = new();

        public void Construct(AllServices services)
        {
            _services = services;
        }

        public NetworkObject CreateWeapon(ulong ownId, WeaponId id, ItemInHandFollowTarget followTarget, CharacterAnimator characterAnimator, params SurfaceId[] ignoreTargets)
        {
            WeaponConfig config = GetService<IStaticDataService>().ForWeapon(id);
            NetworkObject weaponHandler = Instantiate(config.Prefab);
            weaponHandler.GetComponent<WeaponIdKeeper>().SetId(id);
            weaponHandler.SpawnWithOwnership(ownId);
            WeaponsNetworkList.Add(weaponHandler);

            TransformFollow transformFollow = weaponHandler.GetComponent<TransformFollow>();
            transformFollow.TargetNetworkVariable.Value = followTarget.NetworkObject;
            InvokeTransformFollowClientRpc(followTarget.NetworkObject, weaponHandler);

            return weaponHandler;
        }

        public void HideWeapon(NetworkObjectReference reference)
        {
            WeaponsNetworkList.Remove(reference);
            if (reference.TryGet(out var networkObject))
                networkObject.Despawn();
        }

        [ClientRpc]
        private void InvokeTransformFollowClientRpc(NetworkObjectReference targetReference, NetworkObjectReference followReference)
        {
            followReference.TryGet(out NetworkObject followNetwork);
            targetReference.TryGet(out NetworkObject targetNetwork);
            followNetwork.GetComponent<TransformFollow>().SetTarget(targetNetwork.GetComponent<ItemInHandFollowTarget>().FollowTarget);
        }

        private TService GetService<TService>() where TService : IService =>
            _services.Single<TService>();
    }
}