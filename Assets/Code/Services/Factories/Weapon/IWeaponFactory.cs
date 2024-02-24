using Code.Character;
using Code.Logic.FollowTransform;
using Code.Logic.SurfaceId;
using Code.StaticData.Weapon;
using Unity.Netcode;

namespace Code.Services.Factories.Weapon
{
    public interface IWeaponFactory : IService
    {
        [ServerRpc(RequireOwnership = false)]
        void HideWeapon(NetworkObjectReference reference);

        NetworkList<NetworkObjectReference> WeaponsNetworkList { get; }
        NetworkObject CreateWeapon(ulong ownId, WeaponId id, ItemInHandFollowTarget followTarget,
            CharacterAnimator characterAnimator, params SurfaceId[] ignoreTargets);
    }
}