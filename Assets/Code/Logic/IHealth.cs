using Code.Logic.SurfaceId;
using Unity.Netcode;

namespace Code.Logic
{
    public interface IHealth : ISurfaceId
    {
        NetworkObject NetworkObject { get; }
        NetworkVariable<float> MaxHealth { get; }
        NetworkVariable<float> CurrentHealth { get; }

        [ServerRpc(RequireOwnership = false)]
        void TakeDamageServerRpc(float value);
    }
}