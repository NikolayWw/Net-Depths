#if UNITY_EDITOR

using UnityEditor;

#endif

using UnityEngine;

namespace Code.StaticData
{
    public abstract class BaseConfig<TId, TIdKeeper, TPrefab> where TIdKeeper : IdKeeper<TId>
                                                              where TPrefab : MonoBehaviour
    {
        [SerializeField] private string _inspectorName;
        [field: SerializeField] public TId Id { get; private set; }
        [field: SerializeField] public TPrefab Prefab { get; private set; }

        public virtual void OnValidate()
        {
            _inspectorName = Id.ToString();
#if UNITY_EDITOR

            if (Prefab != null)
            {
                if (Prefab.TryGetComponent(out TIdKeeper idKeeper))
                {
                    idKeeper.SetId(Id);
                    EditorUtility.SetDirty(idKeeper);
                }
                else
                {
                    Debug.LogError("Id Keeper is missing");
                }
            }
#endif
        }
    }
}