using Code.Infrastructure.Logic;
using Code.Services.Factories.Enemy;
using Code.Services.Factories.Player;
using Code.StaticData.Enemy;
using Code.StaticData.Player;
using UnityEngine;

namespace Code.Infrastructure.GameStateMachine.State
{
    public class HostLoadLevelState : IPayloadState<ulong, string>
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly IPlayerFactory _playerFactory;
        private readonly IEnemyFactory _enemyFactory;
        private ulong _ownId;

        public HostLoadLevelState(IGameStateMachine gameStateMachine, SceneLoader sceneLoader, IPlayerFactory playerFactory, IEnemyFactory enemyFactory)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _playerFactory = playerFactory;
            _enemyFactory = enemyFactory;
        }

        public void Enter(ulong ownId, string sceneKey)
        {
            _ownId = ownId;
            _sceneLoader.Load(sceneKey, OnLoaded);
        }

        public void Exit()
        { }

        private void OnLoaded()
        {
            _playerFactory.CreatePlayerServerRpc(_ownId, PlayerId.Boy);
            _enemyFactory.CreateEnemy(EnemyId.Skeleton, Vector2.zero, _ownId);
            _gameStateMachine.Enter<LoopState>();
        }
    }
}