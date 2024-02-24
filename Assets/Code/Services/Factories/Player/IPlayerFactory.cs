using Code.StaticData.Player;
using Unity.Netcode;

namespace Code.Services.Factories.Player
{
    public interface IPlayerFactory : IService
    {
        [ServerRpc(RequireOwnership = false)]
        void CreatePlayerServerRpc(ulong ownId, PlayerId id);
    }
}