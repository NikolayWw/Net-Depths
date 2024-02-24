using System.Collections.Generic;
using UnityEngine;

namespace Code.StaticData.Weapon
{
    [CreateAssetMenu(menuName = "Static Data/Weapon Static Data", order = 0)]
    public class WeaponsStaticData : ScriptableObject
    {
        public List<WeaponConfig> Configs;

        private void OnValidate()
        {
            Configs.ForEach(x => x.OnValidate());
        }
    }
}