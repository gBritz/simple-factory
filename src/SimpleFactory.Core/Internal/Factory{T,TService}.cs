using Microsoft.Extensions.DependencyInjection;
using SimpleFactory.Core.Abstractions;

namespace SimpleFactory.Core.Internal
{
    internal class Factory<T, TService> :
        //FactoryBase<TService>,
        IFactory<T, TService>
        where TService : class
    {
        private readonly IServiceProvider _serviceProvider;

        public Factory(IServiceProvider serviceProvider) //, ServiceMap serviceMap
                                                         //: base(serviceProvider, serviceMap, new[] { typeof(T) })
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public TService New(T arg) =>
            //this.New(new object[] { arg });
            _serviceProvider.GetRequiredService<TService>(); //// TODO: build-in switch case in IL dynamicly
    }
}