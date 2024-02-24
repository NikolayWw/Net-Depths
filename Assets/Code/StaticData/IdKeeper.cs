using UnityEngine;

namespace Code.StaticData
{
    public abstract class IdKeeper<TId> : MonoBehaviour
    {
        [HideInInspector] [SerializeField] private TId _id;
        public TId Id => _id;

        public void SetId(TId id) =>
            _id = id;
    }
}