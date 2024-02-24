using Unity.Netcode;
using UnityEngine;

namespace Code.Logic.FollowTransform
{
    public class TransformFollow : NetworkBehaviour
    {
        [SerializeField] private bool _followRotate = true;

        private Transform _target;
        private Transform _cacheTransform;
        public readonly NetworkVariable<NetworkObjectReference> TargetNetworkVariable = new();

        public void SetTarget(Transform target)
        {
            _cacheTransform = transform;
            _target = target;
        }

        private void LateUpdate()
        {
            if (_target == null)
                return;

            if (_followRotate)
                _cacheTransform.SetPositionAndRotation(_target.position, _target.rotation);
            else
                _cacheTransform.position = _target.position;
        }
    }
}