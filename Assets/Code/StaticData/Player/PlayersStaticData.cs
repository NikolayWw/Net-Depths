using System.Collections.Generic;
using UnityEngine;

namespace Code.StaticData.Player
{
    [CreateAssetMenu(menuName = "Static Data/Players Static Data", order = 0)]
    public class PlayersStaticData : ScriptableObject
    {
        public List<PlayerConfig> Configs;

        private void OnValidate()
        {
            Configs.ForEach(x => x.OnValidate());
        }
    }
}