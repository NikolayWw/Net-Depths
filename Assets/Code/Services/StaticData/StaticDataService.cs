using Code.StaticData.Enemy;
using Code.StaticData.Player;
using Code.StaticData.Weapon;
using Code.StaticData.Windows;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Code.Services.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private const string WindowsDataPath = "Windows/WindowsStaticData";
        private const string PlayerDataPath = "Player/PlayersStaticData";
        private const string WeaponDataPath = "Weapon/WeaponsStaticData";
        private const string EnemyDataPath = "Enemy/EnemyStaticData";

        public WindowsStaticData WindowsData { get; private set; }
        private Dictionary<PlayerId, PlayerConfig> _playerConfigs;
        private Dictionary<WeaponId, WeaponConfig> _weaponConfigs;
        private Dictionary<EnemyId, EnemyConfig> _enemyConfigs;

        public void Load()
        {
            WindowsData = Resources.Load<WindowsStaticData>(WindowsDataPath);
            _playerConfigs = Resources.Load<PlayersStaticData>(PlayerDataPath).Configs.ToDictionary(x => x.Id, x => x);
            _weaponConfigs = Resources.Load<WeaponsStaticData>(WeaponDataPath).Configs.ToDictionary(x => x.Id, x => x);
            _enemyConfigs = Resources.Load<EnemyStaticData>(EnemyDataPath).Configs.ToDictionary(x => x.Id, x => x);
        }

        public EnemyConfig ForEnemy(EnemyId id) =>
            _enemyConfigs.TryGetValue(id, out EnemyConfig cfg) ? cfg : null;

        public WeaponConfig ForWeapon(WeaponId id) =>
            _weaponConfigs.TryGetValue(id, out WeaponConfig cfg) ? cfg : null;

        public PlayerConfig ForPlayer(PlayerId id) =>
            _playerConfigs.TryGetValue(id, out PlayerConfig cfg) ? cfg : null;
    }
}