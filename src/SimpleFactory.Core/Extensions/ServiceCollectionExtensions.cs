using Microsoft.Extensions.DependencyInjection;
using SimpleFactory.Core.Abstractions;
using SimpleFactory.Core.Internal;

namespace SimpleFactory.Core.Extensions
{
    /// <summary>
    /// Extension for Simple Factory.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add a generic factory facility for all <see cref="IFactory{TService}"/>, <see cref="IFactory{T, TService}"/>.
        /// </summary>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add the factory service to.</param>
        /// <returns>Simple factory builder to configure factories.</returns>
        public static SimpleFactoryBuilder AddSimpleFactory(this IServiceCollection services)
        {
            services
                .AddSingleton(typeof(IFactory<>), typeof(Factory<>));

            return new(services);
        }
    }
}