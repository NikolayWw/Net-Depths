namespace Code.Services
{
    public class AllServices
    {
        public static AllServices _container;
        public static AllServices Container => _container ?? (_container = new AllServices());

        public void RegisterSingle<TService>(TService implementation) where TService : IService =>
            Implementation<TService>.Instance = implementation;

        public TService Single<TService>() where TService : IService =>
            Implementation<TService>.Instance;

        private class Implementation<TService> where TService : IService
        {
            public static TService Instance;
        }
    }
}