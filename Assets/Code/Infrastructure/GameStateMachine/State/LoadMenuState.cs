using Code.Infrastructure.Logic;
using Code.UI.Services.Factories.NetworkFactoryService;
using Code.UI.Services.Factories.UIFactoryService;

namespace Code.Infrastructure.GameStateMachine.State
{
    public class LoadMenuState : IState
    {
        private const string MenuSceneKey = "MainMenu";

        private readonly IGameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly IUIFactory _uiFactory;
        private readonly INetworkFactory _networkFactory;

        public LoadMenuState(IGameStateMachine gameStateMachine, SceneLoader sceneLoader, IUIFactory uiFactory, INetworkFactory networkFactory)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _uiFactory = uiFactory;
            _networkFactory = networkFactory;
        }

        public void Enter()
        {
            _sceneLoader.Load(MenuSceneKey, OnLoaded);
        }

        public void Exit()
        { }

        private void OnLoaded()
        {
            _uiFactory.CreateUIRoot();
            _networkFactory.CreateFindLobbiesWindow();
            _gameStateMachine.Enter<LoopState>();
        }
    }
}