using Microsoft.Extensions.DependencyInjection;
using System;
using Unity;

namespace CaseManagement.AspNetWebApi
{
    public class UnityServiceProvider : IServiceProvider, ISupportRequiredService, IServiceScopeFactory, IServiceScope, IDisposable
    {
        private IUnityContainer _container;

        internal UnityServiceProvider(IUnityContainer container)
        {
            _container = container;
        }
        
        public object GetService(Type serviceType)
        {
            try
            {
                return _container.Resolve(serviceType, null);
            }
            catch { /* Ignore */ }
            return null;
        }

        public object GetRequiredService(Type serviceType)
        {
            return _container.Resolve(serviceType, null);
        }


        public IServiceScope CreateScope()
        {
            return new UnityServiceProvider(_container.CreateChildContainer());
        }

        IServiceProvider IServiceScope.ServiceProvider => this;

        public static explicit operator UnityContainer(UnityServiceProvider c)
        {
            return (UnityContainer)c._container;
        }

        protected virtual void Dispose(bool disposing)
        {
            IDisposable disposable = _container;
            _container = null;
            disposable?.Dispose();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}