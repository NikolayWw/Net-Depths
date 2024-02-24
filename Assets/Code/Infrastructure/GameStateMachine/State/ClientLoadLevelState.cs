using Code.Logic;
using Code.Logic.FollowTransform;
using Code.Services.Factories.Player;
using Code.Services.Factories.Weapon;
using Code.StaticData.Player;
using Code.UI.Services.Factories.HealthWindowFactory;
using Code.UI.Windows.Health;
using Unity.Netcode;

namespace Code.Infrastructure.GameStateMachine.State
{
    public class ClientLoadLevelState : IPayloadState<ulong>
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly IPlayerFactory _gameFactory;
        private readonly IWeaponFactory _weaponFactory;
        private readonly IUIHealthWindowFactory _healthWindowFactory;
        private ulong _owlId;

        public ClientLoadLevelState(IGameStateMachine gameStateMachine, IPlayerFactory gameFactory, IWeaponFactory weaponFactory, IUIHealthWindowFactory healthWindowFactory)
        {
            _gameStateMachine = gameStateMachine;
            _gameFactory = gameFactory;
            _weaponFactory = weaponFactory;
            _healthWindowFactory = healthWindowFactory;
        }

        public void Enter(ulong owlId)
        {
            _owlId = owlId;
            Entered();
        }

        public void Exit()
        { }

        private void Entered()
        {
            SetTransformFollow();
            UpdateHealths();
            _gameFactory.CreatePlayerServerRpc(_owlId, PlayerId.Boy);
            _gameStateMachine.Enter<LoopState>();
        }

        private void UpdateHealths()
        {
            foreach (NetworkObjectReference reference in _healthWindowFactory.WindowsNetworkList)
            {
                if (reference.TryGet(out NetworkObject healthWindowNetwork) == false)
                    continue;

                HealthWindow healthWindow = healthWindowNetwork.GetComponent<HealthWindow>();
                if (healthWindow.TakeDamageVariable.Value.TryGet(out NetworkObject takeDamageNetwork) == false)
                    continue;

                IHealth characterHealth = takeDamageNetwork.GetComponent<IHealth>();
                healthWindow.SetFillAmount(characterHealth.CurrentHealth.Value, characterHealth.MaxHealth.Value);
            }
        }

        private void SetTransformFollow()
        {
            SetList<ItemInHandFollowTarget>(_weaponFactory.WeaponsNetworkList);
            SetList<HealthWindowFollowTarget>(_healthWindowFactory.WindowsNetworkList);

            return;
            static void SetList<TTransform>(NetworkList<NetworkObjectReference> networkList) where TTransform : BaseTransformFollowKeepTarget
            {
                foreach (NetworkObjectReference transformFollowReference in networkList)
                {
                    if (false == transformFollowReference.TryGet(out NetworkObject followNetwork))
                        continue;

                    TransformFollow transformFollow = followNetwork.GetComponent<TransformFollow>();
                    if (false == transformFollow.TargetNetworkVariable.Value.TryGet(out NetworkObject targetKeeperNetwork))
                        continue;

                    TTransform keepTarget = targetKeeperNetwork.GetComponent<TTransform>();
                    transformFollow.SetTarget(keepTarget.FollowTarget);
                }
            }
        }
    }
}