using Code.StaticData.Enemy;
using Code.StaticData.Player;
using Code.StaticData.Weapon;
using Code.StaticData.Windows;

namespace Code.Services.StaticData
{
    public interface IStaticDataService : IService
    {
        WindowsStaticData WindowsData { get; }
        PlayerConfig ForPlayer(PlayerId id);
        WeaponConfig ForWeapon(WeaponId id);
        EnemyConfig ForEnemy(EnemyId id);
    }
}