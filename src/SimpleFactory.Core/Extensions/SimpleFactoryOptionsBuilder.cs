using Microsoft.Extensions.DependencyInjection;

namespace SimpleFactory.Core.Extensions
{
    public sealed class SimpleFactoryOptionsBuilder<TService, TParameter>
        where TService : notnull
        where TParameter : notnull
    {
        private readonly Dictionary<TParameter, Type> _mappingKeyService = [];

        internal Type? Default { get; private set; }

        internal StringComparison? Comparison { get; private set; }

        internal ServiceLifetime ServiceLifetime { get; private set; } = ServiceLifetime.Transient;

        internal IEnumerable<KeyValuePair<TParameter, Type>> Mappings => _mappingKeyService;

        public SimpleFactoryOptionsBuilder<TService, TParameter> When<TImplementation>(TParameter param)
            where TImplementation : TService
        {
            _mappingKeyService.Add(param, typeof(TImplementation));
            return this;
        }

        public SimpleFactoryOptionsBuilder<TService, TParameter> Otherwise<TImplementation>()
            where TImplementation : TService
        {
            Default = typeof(TImplementation);
            return this;
        }

        public SimpleFactoryOptionsBuilder<TService, TParameter> As(ServiceLifetime lifetime)
        {
            ServiceLifetime = lifetime;
            return this;
        }

        public SimpleFactoryOptionsBuilder<TService, TParameter> AsTransient()
        {
            ServiceLifetime = ServiceLifetime.Transient;
            return this;
        }

        public SimpleFactoryOptionsBuilder<TService, TParameter> AsSingleton()
        {
            ServiceLifetime = ServiceLifetime.Singleton;
            return this;
        }

        public SimpleFactoryOptionsBuilder<TService, TParameter> AsScoped()
        {
            ServiceLifetime = ServiceLifetime.Scoped;
            return this;
        }

        internal SimpleFactoryOptionsBuilder<TService, TParameter> AddStringComparison(StringComparison comparison)
        {
            Comparison = comparison;
            return this;
        }
    }
}
