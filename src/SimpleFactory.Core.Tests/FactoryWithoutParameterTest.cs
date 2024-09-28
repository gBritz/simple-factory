using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using SimpleFactory.Core.Abstractions;
using SimpleFactory.Core.Extensions;
using SimpleFactory.Core.Tests.Domain;

namespace SimpleFactory.Core.Tests
{
    public class FactoryWithoutParameterTest
    {
        [Fact]
        public void WhenCreateFruitWithoutParameter_ResultShouldNewInstanceFromLatestRegistrationType()
        {
            var fruitFactory = BuildServiceProvider().GetRequiredService<IFactory<IFruit>>();

            var fruit = fruitFactory.New();

            fruit.Should().NotBeNull();
            fruit.Should().BeOfType<Orange>();
        }

        private static ServiceProvider BuildServiceProvider()
        {
            var serviceCollection = new ServiceCollection();
            ConfigureFruitFactory(serviceCollection);
            return serviceCollection.BuildServiceProvider();
        }

        private static void ConfigureFruitFactory(IServiceCollection services)
        {
            services.AddTransient<IFruit, Banana>();
            services.AddTransient<IFruit, Apple>();
            services.AddTransient<IFruit, Pineapple>();
            services.AddTransient<IFruit, Orange>();

            services.AddSimpleFactory();
                //.Of<IFruit>();
        }
    }
}