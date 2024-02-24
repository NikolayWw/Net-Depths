using Code.Logic;
using Code.Logic.Despawn;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.Windows.Health
{
    public class HealthWindow : NetworkBehaviour
    {
        [SerializeField] private Image _healthFillImage;

        private IHealth _characterHealth;
        public readonly NetworkVariable<NetworkObjectReference> TakeDamageVariable = new();

        public void Construct(IHealth characterHealth)
        {
            if (!IsOwner)
                return;

            _characterHealth = characterHealth;
            _characterHealth.CurrentHealth.OnValueChanged += Refresh;
            Refresh(0, _characterHealth.CurrentHealth.Value);
        }

        public override void OnNetworkDespawn()
        {
            if (_characterHealth != null)
                _characterHealth.CurrentHealth.OnValueChanged -= Refresh;
        }

        public void SetFillAmount(float current, float max)
        {
            float health = current / max;
            if (float.IsNaN(health) == false)
                _healthFillImage.fillAmount = current / max;
        }

        public void ReadDespawn(DespawnReporter reporter)
        {
            reporter.OnDespawn += DespawnThis;
        }

        private void DespawnThis()
        {
            if (NetworkObject.IsSpawned)
                NetworkObject.Despawn();
        }

        private void Refresh(float _, float newValue) =>
            RefreshServerRpc(_characterHealth.CurrentHealth.Value, _characterHealth.MaxHealth.Value);

        [ServerRpc(RequireOwnership = false)]
        private void RefreshServerRpc(float current, float max) =>
            RefreshClientRpc(current, max);

        [ClientRpc]
        private void RefreshClientRpc(float current, float max) =>
            SetFillAmount(current, max);
    }
}