using Code.Infrastructure.GameStateMachine.State;
using Code.Infrastructure.Logic;
using Code.Services;
using Code.Services.Factories.Enemy;
using Code.Services.Factories.Player;
using Code.Services.Factories.Weapon;
using Code.UI.Services.Factories.HealthWindowFactory;
using Code.UI.Services.Factories.UIFactoryService;
using System;
using System.Collections.Generic;
using Code.UI.Services.Factories.NetworkFactoryService;

namespace Code.Infrastructure.GameStateMachine
{
    public class GameStateMachine : IGameStateMachine
    {
        private readonly Dictionary<Type, IExitable> _states;
        private IExitable _activeState = new LoopState();

        public GameStateMachine(SceneLoader sceneLoader, AllServices services)
        {
            _states = new Dictionary<Type, IExitable>
            {
                [typeof(LoopState)] = new LoopState(),
                [typeof(LoadMenuState)] = new LoadMenuState(this, sceneLoader,
                    services.Single<IUIFactory>(),
                    services.Single<INetworkFactory>()),

                [typeof(ClientLoadLevelState)] = new ClientLoadLevelState(this,
                    services.Single<IPlayerFactory>(),
                    services.Single<IWeaponFactory>(),
                    services.Single<IUIHealthWindowFactory>()),

                [typeof(HostLoadLevelState)] = new HostLoadLevelState(this, sceneLoader,
                    services.Single<IPlayerFactory>(),
                    services.Single<IEnemyFactory>()),
            };
        }

        public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadState<TPayload>
        {
            TState state = ChangeState<TState>();
            state.Enter(payload);
        }

        public void Enter<TState, TPayload1, TPayload2>(TPayload1 payload1, TPayload2 payload2) where TState : class, IPayloadState<TPayload1, TPayload2>
        {
            TState state = ChangeState<TState>();
            state.Enter(payload1, payload2);
        }

        public void Enter<TState>() where TState : class, IState
        {
            TState state = ChangeState<TState>();
            state.Enter();
        }

        private TState ChangeState<TState>() where TState : class, IExitable
        {
            _activeState.Exit();
            TState state = _states[typeof(TState)] as TState;
            _activeState = state;
            return state;
        }
    }
}