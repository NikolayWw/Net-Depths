using Code.Services;
using Code.Services.StaticData;
using Unity.Netcode;
using UnityEngine;

namespace Code.UI.Services.Factories.UIFactoryService
{
    public class UIFactory : NetworkBehaviour, IUIFactory
    {
        private AllServices _services;
        public Transform UIRoot { get; private set; }

        public void Construct(AllServices services)
        {
            _services = services;
        }

        public void CreateUIRoot()
        {
            IStaticDataService staticDataService = GetService<IStaticDataService>();

            GameObject prefab = staticDataService.WindowsData.UIRootPrefab;
            UIRoot = Instantiate(prefab).transform;
        }

        private TService GetService<TService>() where TService : IService =>
            _services.Single<TService>();
    }
}