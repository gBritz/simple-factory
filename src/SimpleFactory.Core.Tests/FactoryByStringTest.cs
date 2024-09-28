using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using SimpleFactory.Core.Abstractions;
using SimpleFactory.Core.Extensions;
using SimpleFactory.Core.Tests.Domain;

namespace SimpleFactory.Core.Tests
{
    public class FactoryByStringTest
    {
        [Theory]
        [InlineData("banana", typeof(Banana))]
        [InlineData("apple", typeof(Apple))]
        public void WhenCreateFruitByType_ResultShouldNewInstanceFromTypeImplementation(
            string namedFruitType,
            Type typeImplementation)
        {
            var fruitFactory = CreateFruitFactory(ConfigureFruitFactoryWithOtherwise);

            var fruit = fruitFactory.New(namedFruitType);

            fruit.Should().NotBeNull();
            fruit.Should().BeOfType(typeImplementation);
        }

        [Theory]
        [InlineData("BANANA", typeof(Banana))]
        [InlineData("APPLE", typeof(Apple))]
        [InlineData("banana", typeof(Banana))]
        [InlineData("apple", typeof(Apple))]
        public void GivenOrdinalIgnoreCaseComparison_WhenCreateFruitByType_ResultShouldNewInstanceFromTypeImplementation(
            string namedFruitType,
            Type typeImplementation)
        {
            var fruitFactory = CreateFruitFactory(ConfigureOrdinalIgnoreCaseComparison);

            var fruit = fruitFactory.New(namedFruitType);

            fruit.Should().NotBeNull();
            fruit.Should().BeOfType(typeImplementation);
        }

        [Theory]
        [InlineData("", typeof(Pineapple))]
        [InlineData(null, typeof(Pineapple))]
        [InlineData("Pineapple", typeof(Pineapple))]
        [InlineData("Orange", typeof(Pineapple))]
        public void WhenTypeIsNotSpecified_ResultShouldBeOtherwise(
            string? namedFruitType,
            Type typeImplementation)
        {
            var fruitFactory = CreateFruitFactory(ConfigureFruitFactoryWithOtherwise);

            var fruit = fruitFactory.New(namedFruitType);

            fruit.Should().NotBeNull();
            fruit.Should().BeOfType(typeImplementation);
        }

        [Fact]
        public void GivenSimpleFactoryWithoutOtherwiseConfigurated_WhenPassTypeNotSpecified_ResultShouldBeNull()
        {
            var fruitFactory = CreateFruitFactory(ConfigureFruitFactoryWithoutOtherwise);

            var fruit = fruitFactory.New("Orange");

            fruit.Should().BeNull();
        }

        [Theory]
        [InlineData("Banana", typeof(Banana))]
        [InlineData("Orange", typeof(Orange))]
        [InlineData("Pineapple", typeof(Pineapple))]
        [InlineData("Apple", typeof(Apple))]
        [InlineData("Bmw", typeof(Bmw))]
        [InlineData("Citroen", typeof(Citroen))]
        [InlineData("Dodge", typeof(Dodge))]
        [InlineData("Jeep", typeof(Jeep))]
        public void GivenFruitsAndCars_WhenCreateByType_ResultShouldBeNotNull(
            string type,
            Type typeImplementation)
        {
            object instanceResult;
            var isFruitType = typeof(IFruit).IsAssignableFrom(typeImplementation);
            var serviceProvider = BuildServiceProvider(ConfigureFruitsAndCarsFactory);

            if (isFruitType)
            {
                var fruitFactory = serviceProvider.GetRequiredService<IFactory<string, IFruit>>();
                instanceResult = fruitFactory.New(type);
            }
            else
            {
                var carFactory = serviceProvider.GetRequiredService<IFactory<string, ICar>>();
                instanceResult = carFactory.New(type);
            }

            instanceResult.Should().NotBeNull();
            instanceResult.Should().BeOfType(typeImplementation);
        }

        private static IFactory<string, IFruit> CreateFruitFactory(Action<IServiceCollection> configureServices) =>
            BuildServiceProvider(configureServices).GetRequiredService<IFactory<string, IFruit>>();

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
            services.AddTransient<Orange>();

            services.AddSimpleFactory()
                .Of<IFruit, string>(_ => _
                    .When<Banana>("banana")
                    .When<Apple>("apple")
                    .Otherwise<Pineapple>());
        }

        private static void ConfigureFruitFactoryWithoutOtherwise(IServiceCollection services)
        {
            services.AddTransient<Banana>();
            services.AddTransient<Orange>();

            services.AddSimpleFactory()
                .Of<IFruit, string>(_ => _
                    .When<Banana>("banana"));
        }

        private static void ConfigureFruitsAndCarsFactory(IServiceCollection services)
        {
            services.AddTransient<Banana>();
            services.AddTransient<Orange>();
            services.AddTransient<Pineapple>();
            services.AddTransient<Apple>();
            services.AddTransient<Bmw>();
            services.AddTransient<Citroen>();
            services.AddTransient<Dodge>();
            services.AddTransient<Jeep>();

            services.AddSimpleFactory()
                .Of<IFruit, string>(_ => _
                    .When<Banana>("Banana")
                    .When<Orange>("Orange")
                    .When<Pineapple>("Pineapple")
                    .When<Apple>("Apple"))
                .Of<ICar, string>(_ => _
                    .When<Bmw>("Bmw")
                    .When<Citroen>("Citroen")
                    .When<Dodge>("Dodge")
                    .When<Jeep>("Jeep"));
        }

        private static void ConfigureOrdinalIgnoreCaseComparison(IServiceCollection services)
        {
            services.AddTransient<Banana>();
            services.AddTransient<Apple>();

            services.AddSimpleFactory()
                .Of<IFruit, string>(_ => _
                    .Comparison(StringComparison.OrdinalIgnoreCase)
                    .When<Banana>("banana")
                    .When<Apple>("apple"));
        }
    }
}