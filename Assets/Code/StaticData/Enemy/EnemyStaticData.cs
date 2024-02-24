using System.Collections.Generic;
using UnityEngine;

namespace Code.StaticData.Enemy
{
    [CreateAssetMenu(menuName = "Static Data/Enemy Static Data", order = 0)]
    public class EnemyStaticData : ScriptableObject
    {
        public List<EnemyConfig> Configs;

        private void OnValidate()
        {
            Configs.ForEach(x => x.OnValidate());
        }
    }
}