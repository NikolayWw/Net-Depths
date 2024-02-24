using Code.Infrastructure.GameStateMachine;
using Code.Infrastructure.Logic;
using Code.Services.Factories.Enemy;
using Code.Services.Factories.Player;
using Code.Services.Factories.Weapon;
using Code.Services.InputService;
using Code.Services.Network;
using Code.Services.StaticData;
using Code.UI.Services.Factories.HealthWindowFactory;
using Code.UI.Services.Factories.NetworkFactoryService;
using Code.UI.Services.Factories.UIFactoryService;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Code.Services
{
    public class RegisterServices
    {
        private const string NetworkManagerPrefabPath = "NetworkManager";

        public RegisterServices(SceneLoader sceneLoader, ICoroutineRunner coroutineRunner)
        {
            InitUnityServices();

            NetworkManager networkManagerPrefab = Resources.Load<NetworkManager>(NetworkManagerPrefabPath);
            NetworkManager networkManagerInstance = Object.Instantiate(networkManagerPrefab);
            AllServices services = AllServices.Container;

            GameObject container = new("Services Container");
            Object.DontDestroyOnLoad(container);
            container.AddComponent<NetworkObject>();

            PlayerFactory playerFactory = container.AddComponent<PlayerFactory>();
            playerFactory.Construct(services);

            WeaponFactory weaponFactory = container.AddComponent<WeaponFactory>();
            weaponFactory.Construct(services);

            UIFactory uiFactory = container.AddComponent<UIFactory>();
            uiFactory.Construct(services);

            EnemyFactory enemyFactory = container.AddComponent<EnemyFactory>();
            enemyFactory.Construct(services);

            UIHealthWindowFactory healthWindowFactory = container.AddComponent<UIHealthWindowFactory>();
            healthWindowFactory.Construct(services);

            RegisterStaticData(services);
            services.RegisterSingle<IPlayerFactory>(playerFactory);
            services.RegisterSingle<IWeaponFactory>(weaponFactory);
            services.RegisterSingle<IUIFactory>(uiFactory);
            services.RegisterSingle<IEnemyFactory>(enemyFactory);
            services.RegisterSingle<INetworkFactory>(new NetworkFactory(services, networkManagerInstance));
            services.RegisterSingle<IUIHealthWindowFactory>(healthWindowFactory);
            services.RegisterSingle<IInput>(new InputService.Input());
            services.RegisterSingle<INetworkService>(new NetworkService(coroutineRunner, services.Single<INetworkFactory>(), networkManagerInstance));
            services.RegisterSingle<IGameStateMachine>(new GameStateMachine(sceneLoader, services));
        }

        private async void InitUnityServices()
        {
            try
            {
                await UnityServices.InitializeAsync();
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            }
            catch (ServicesInitializationException e)
            {
                Debug.LogError(e);
            }
        }

        private static void RegisterStaticData(AllServices services)
        {
            StaticDataService service = new();
            service.Load();
            services.RegisterSingle<IStaticDataService>(service);
        }
    }
}