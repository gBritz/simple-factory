using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using SimpleFactory.Core.Abstractions;
using SimpleFactory.Core.Extensions;
using SimpleFactory.Core.Tests.Domain;
using System.Reflection.Metadata.Ecma335;

namespace SimpleFactory.Core.Tests
{
    public class FactoryByInt64Test
    {
        [Theory]
        [InlineData(0, typeof(Banana))]
        [InlineData(1, typeof(Apple))]
        public void WhenCreateFruitByType_ResultShouldNewInstanceFromTypeImplementation(
            long numberOfFruitType,
            Type typeImplementation)
        {
            var fruitFactory = CreateFruitFactory(ConfigureFruitFactoryWithOtherwise);

            var fruit = fruitFactory.New(numberOfFruitType);

            fruit.Should().NotBeNull();
            fruit.Should().BeOfType(typeImplementation);
        }

        [Theory]
        [InlineData((long)FruitType.Pineapple, typeof(Pineapple))]
        [InlineData((long)FruitType.Strawberry, typeof(Pineapple))]
        public void WhenTypeIsNotSpecified_ResultShouldBeOtherwise(
            int numberOfFruitType,
            Type typeImplementation)
        {
            var fruitFactory = CreateFruitFactory(ConfigureFruitFactoryWithOtherwise);

            var fruit = fruitFactory.New(numberOfFruitType);

            fruit.Should().NotBeNull();
            fruit.Should().BeOfType(typeImplementation);
        }

        [Fact]
        public void GivenSimpleFactoryWithoutOtherwiseConfigurated_WhenPassTypeNotSpecified_ResultShouldBeNull()
        {
            var fruitFactory = CreateFruitFactory(ConfigureFruitFactoryWithoutOtherwise);

            var fruit = fruitFactory.New(999);

            fruit.Should().BeNull();
        }

        private static IFactory<long, IFruit> CreateFruitFactory(Action<IServiceCollection> configureServices) =>
            BuildServiceProvider(configureServices).GetRequiredService<IFactory<long, IFruit>>();

        private static ServiceProvider BuildServiceProvider(Action<IServiceCollection> configureServices)
        {
            ArgumentNullException.ThrowIfNull(configureServices, nameof(configureServices));

            var serviceCollection = new ServiceCollection();
            configureServices(serviceCollection);
            return serviceCollection.BuildServiceProvider();
        }

        private static void ConfigureFruitFactoryWithOtherwise(IServiceCollection services)
        {
            services.AddTransient<Banana>();
            services.AddTransient<Apple>();
            services.AddTransient<Pineapple>();
            services.AddTransient<Strawberry>();

            services.AddSimpleFactory()
                .Of<IFruit, long>(_ => _
                    .When<Banana>(0)
                    .When<Apple>(1)
                    .Otherwise<Pineapple>());
        }

        private static void ConfigureFruitFactoryWithInt64MaxValue(IServiceCollection services)
        {
            services.AddTransient<Banana>();

            services.AddSimpleFactory()
                .Of<IFruit, long>(_ => _
                    .When<Banana>(long.MaxValue));
        }

        private static void ConfigureFruitFactoryWithoutOtherwise(IServiceCollection services)
        {
            services.AddTransient<Banana>();
            services.AddTransient<Orange>();

            services.AddSimpleFactory()
                .Of<IFruit, long>(_ => _
                    .When<Banana>(0));
        }
    }
}