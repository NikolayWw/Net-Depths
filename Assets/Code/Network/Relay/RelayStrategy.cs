using System.Threading.Tasks;
using Unity.Networking.Transport.Relay;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

namespace Code.Network.Relay
{
    public class RelayStrategy
    {
        private const string DTLSKey = "dtls";
        public async Task<CreateRelayData> CreateRelay(int maxConnections)
        {
            try
            {
                return await Create();
            }
            catch (RelayServiceException e)
            {
                Debug.Log(e);
            }

            return null;

            async Task<CreateRelayData> Create()
            {
                Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxConnections);
                string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
                RelayServerData serverData = new(allocation, DTLSKey);
                CreateRelayData data = new()
                {
                    RelayCode = joinCode,
                    RelayServerData = serverData
                };
                return data;
            }
        }
        public async Task<CreateRelayData> JoinRelay(string joinCode)
        {
            try
            {
                JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
                RelayServerData serverData = new(joinAllocation, DTLSKey);
                CreateRelayData data = new() { RelayServerData = serverData };

                return data;
            }
            catch (RelayServiceException e)
            {
                Debug.Log(e);
            }

            return null;
        }
    }
}