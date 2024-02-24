using Code.Infrastructure.GameStateMachine;
using Code.Infrastructure.GameStateMachine.State;
using Code.Services;
using Unity.Netcode;
using static Code.StaticData.Constants.GameConstants;

namespace Code.Network.Netcode
{
    public class HostClientEntryPoint : NetworkBehaviour
    {
        private void Start()
        {
            if (IsServer && IsOwner)
            {
                AllServices.Container.Single<IGameStateMachine>()
                    .Enter<HostLoadLevelState, ulong, string>(OwnerClientId, FirstLevelSceneKey);
            }
            else if (IsOwner)
            {
                AllServices.Container.Single<IGameStateMachine>()
                    .Enter<ClientLoadLevelState, ulong>(OwnerClientId);
            }
        }
    }
}