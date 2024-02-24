using Code.Services;
using UnityEngine;

namespace Code.UI.Services.Factories.UIFactoryService
{
    public interface IUIFactory : IService
    {
        Transform UIRoot { get; }

        void CreateUIRoot();
    }
}