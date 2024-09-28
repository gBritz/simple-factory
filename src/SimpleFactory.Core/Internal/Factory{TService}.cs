using Microsoft.Extensions.DependencyInjection;
using SimpleFactory.Core.Abstractions;

namespace SimpleFactory.Core.Internal
{
    internal class Factory<TService> : IFactory<TService>
        where TService : class
    {
        private readonly IServiceProvider _serviceProvider;

        public Factory(IServiceProvider serviceProvider) =>
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

        public TService New() => _serviceProvider.GetRequiredService<TService>();
    }
}