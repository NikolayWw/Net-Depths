using Code.Logic;
using Code.Logic.SurfaceId;
using System;
using Unity.Netcode;
using UnityEngine;

namespace Code.Enemy
{
    public class EnemyFindTarget : NetworkBehaviour
    {
        public Action OnFound;
        public IHealth Health { get; private set; }
        public Transform TargetTransform { get; private set; }
        private float _timer;
        
        private void Update()
        {
            if (IsServer == false)
                return;

            _timer += Time.deltaTime;
           
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (IsServer == false)
                return;

            if (_timer < 4)
                return;

            if (other.TryGetComponent(out LinkToRootOnCollider linkToRoot) == false)
                return;

            if (linkToRoot.Root.TryGetComponent(out IHealth health) == false)
                return;

            if (health.SurfaceId == SurfaceId.Player)
            {
                Health = health;
                TargetTransform = linkToRoot.Root.transform;
                OnFound?.Invoke();
            }
        }
    }
}