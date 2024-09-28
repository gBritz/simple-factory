using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using SimpleFactory.Core.Abstractions;
using SimpleFactory.Core.Extensions;
using SimpleFactory.Core.Tests.Domain;

namespace SimpleFactory.Core.Tests
{
    public class FactoryByEnumTest
    {
        [Theory]
        [InlineData(FruitType.Banana, typeof(Banana))]
        [InlineData(FruitType.Apple, typeof(Apple))]
        public void WhenCreateFruitByType_ResultShouldNewInstanceFromTypeImplementation(
            FruitType type,
            Type typeImplementation)
        {
            var fruitFactory = CreateFruitFactory(ConfigureFruitFactoryWithOtherwise);

            var fruit = fruitFactory.New(type);

            fruit.Should().NotBeNull();
            fruit.Should().BeOfType(typeImplementation);
        }

        [Theory]
        [InlineData(FruitType.Pineapple, typeof(Pineapple))]
        [InlineData(FruitType.Orange, typeof(Pineapple))]
        public void WhenTypeIsNotSpecified_ResultShouldBeOtherwise(
            FruitType type,
            Type typeImplementation)
        {
            var fruitFactory = CreateFruitFactory(ConfigureFruitFactoryWithOtherwise);

            var fruit = fruitFactory.New(type);

            fruit.Should().NotBeNull();
            fruit.Should().BeOfType(typeImplementation);
        }

        [Theory]
        [InlineData(FruitType.Banana, typeof(Banana))]
        [InlineData(FruitType.Orange, typeof(Orange))]
        [InlineData(FruitType.Pineapple, typeof(Pineapple))]
        [InlineData(FruitType.Apple, typeof(Apple))]
        [InlineData(FruitType.Strawberry, typeof(Strawberry))]
        [InlineData(FruitType.Mango, typeof(Mango))]
        [InlineData(FruitType.Blueberry, typeof(Blueberry))]
        public void GivenManyFruitsServices_WhenCreateFruitByType_ResultShouldNewInstanceFromTypeImplementation(
            FruitType type,
            Type typeImplementation)
        {
            var fruitFactory = CreateFruitFactory(ConfigureFruitFactoryWithManyServices);

            var fruit = fruitFactory.New(type);

            fruit.Should().NotBeNull();
            fruit.Should().BeOfType(typeImplementation);
        }

        [Theory]
        [InlineData(FruitType.Banana, typeof(Banana))]
        [InlineData(FruitType.Orange, typeof(Orange))]
        [InlineData(FruitType.Pineapple, typeof(Pineapple))]
        [InlineData(FruitType.Apple, typeof(Apple))]
        [InlineData(CarType.Bmw, typeof(Bmw))]
        [InlineData(CarType.Citroen, typeof(Citroen))]
        [InlineData(CarType.Dodge, typeof(Dodge))]
        [InlineData(CarType.Jeep, typeof(Jeep))]
        public void GivenFruitsAndCars_WhenCreateByType_ResultShouldBeNotNull(
            Enum enumType,
            Type typeImplementation)
        {
            object instanceResult;
            var isFruitType = typeof(IFruit).IsAssignableFrom(typeImplementation);
            var serviceProvider = BuildServiceProvider(ConfigureFruitsAndCarsFactory);

            if (isFruitType)
            {
                var fruitFactory = serviceProvider.GetRequiredService<IFactory<FruitType, IFruit>>();
                instanceResult = fruitFactory.New((FruitType)enumType);
            }
            else
            {
                var carFactory = serviceProvider.GetRequiredService<IFactory<CarType, ICar>>();
                instanceResult = carFactory.New((CarType)enumType);
            }

            instanceResult.Should().NotBeNull();
            instanceResult.Should().BeOfType(typeImplementation);
        }

        [Fact]
        public void GivenSimpleFactoryWithoutOtherwiseConfigurated_WhenPassTypeNotSpecified_ResultShouldBeNull()
        {
            var fruitFactory = CreateFruitFactory(ConfigureFruitFactoryWithoutOtherwise);

            var fruit = fruitFactory.New(FruitType.Orange);

            fruit.Should().BeNull();
        }

        [Theory]
        [InlineData(FruitCharType.Banana, typeof(Banana))]
        [InlineData(FruitCharType.Apple, typeof(Apple))]
        public void GivenEnumFruitCharType_WhenCreateFruitByType_ResultShouldNewInstanceFromTypeImplementation(
            FruitCharType type,
            Type typeImplementation)
        {
            var fruitFactory = BuildServiceProvider(ConfigureFruitCharFactory).GetRequiredService<IFactory<FruitCharType, IFruit>>();

            var fruit = fruitFactory.New(type);

            fruit.Should().NotBeNull();
            fruit.Should().BeOfType(typeImplementation);
        }

        [Theory]
        [InlineData(FruitInt64Type.Banana, typeof(Banana))]
        [InlineData(FruitInt64Type.Orange, typeof(Orange))]
        [InlineData(FruitInt64Type.Avocado, typeof(Avocado))]
        public void GivenEnumFruitInt64Type_WhenCreateFruitByType_ResultShouldNewInstanceFromTypeImplementation(
            FruitInt64Type type,
            Type typeImplementation)
        {
            var fruitFactory = BuildServiceProvider(ConfigureFruitInt64Factory).GetRequiredService<IFactory<FruitInt64Type, IFruit>>();

            var fruit = fruitFactory.New(type);

            fruit.Should().NotBeNull();
            fruit.Should().BeOfType(typeImplementation);
        }

        [Theory]
        [InlineData(FruitInt16Type.Banana, typeof(Banana))]
        [InlineData(FruitInt16Type.Orange, typeof(Orange))]
        [InlineData(FruitInt16Type.Pineapple, typeof(Pineapple))]
        [InlineData(FruitInt16Type.Blueberry, typeof(Blueberry))]
        [InlineData(FruitInt16Type.Strawberry, typeof(Strawberry))]
        [InlineData(FruitInt16Type.Apple, typeof(Apple))]
        [InlineData(FruitInt16Type.Mango, typeof(Mango))]
        [InlineData(FruitInt16Type.Avocado, typeof(Avocado))]
        public void GivenEnumFruitInt16Type_WhenCreateFruitByType_ResultShouldNewInstanceFromTypeImplementation(
            FruitInt16Type type,
            Type typeImplementation)
        {
            var fruitFactory = BuildServiceProvider(ConfigureFruitInt16Factory).GetRequiredService<IFactory<FruitInt16Type, IFruit>>();

            var fruit = fruitFactory.New(type);

            fruit.Should().NotBeNull();
            fruit.Should().BeOfType(typeImplementation);
        }

        private static IFactory<FruitType, IFruit> CreateFruitFactory(Action<IServiceCollection> configureServices) =>
            BuildServiceProvider(configureServices).GetRequiredService<IFactory<FruitType, IFruit>>();

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
                .Of<IFruit, FruitType>(_ => _
                    .When<Banana>(FruitType.Banana)
                    .When<Apple>(FruitType.Apple)
                    .Otherwise<Pineapple>());
        }

        private static void ConfigureFruitFactoryWithoutOtherwise(IServiceCollection services)
        {
            services.AddTransient<Banana>();
            services.AddTransient<Orange>();

            services.AddSimpleFactory()
                .Of<IFruit, FruitType>(_ => _
                    .When<Banana>(FruitType.Banana));
        }

        private static void ConfigureFruitFactoryWithManyServices(IServiceCollection services)
        {
            services.AddTransient<Banana>();
            services.AddTransient<Orange>();
            services.AddTransient<Pineapple>();
            services.AddTransient<Apple>();
            services.AddTransient<Strawberry>();
            services.AddTransient<Mango>();
            services.AddTransient<Blueberry>();

            services.AddSimpleFactory()
                .Of<IFruit, FruitType>(_ => _
                    .When<Banana>(FruitType.Banana)
                    .When<Orange>(FruitType.Orange)
                    .When<Pineapple>(FruitType.Pineapple)
                    .When<Apple>(FruitType.Apple)
                    .When<Strawberry>(FruitType.Strawberry)
                    .When<Mango>(FruitType.Mango)
                    .When<Blueberry>(FruitType.Blueberry)
                );
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
                .Of<IFruit, FruitType>(_ => _
                    .When<Banana>(FruitType.Banana)
                    .When<Orange>(FruitType.Orange)
                    .When<Pineapple>(FruitType.Pineapple)
                    .When<Apple>(FruitType.Apple))
                .Of<ICar, CarType>(_ => _
                    .When<Bmw>(CarType.Bmw)
                    .When<Citroen>(CarType.Citroen)
                    .When<Dodge>(CarType.Dodge)
                    .When<Jeep>(CarType.Jeep));
        }

        private static void ConfigureFruitCharFactory(IServiceCollection services)
        {
            services.AddTransient<Banana>();
            services.AddTransient<Apple>();

            services.AddSimpleFactory()
                .Of<IFruit, FruitCharType>(_ => _
                    .When<Banana>(FruitCharType.Banana)
                    .When<Apple>(FruitCharType.Apple));
        }

        private static void ConfigureFruitInt64Factory(IServiceCollection services)
        {
            services.AddTransient<Banana>();
            services.AddTransient<Orange>();
            services.AddTransient<Avocado>();

            services.AddSimpleFactory()
                .Of<IFruit, FruitInt64Type>(_ => _
                    .When<Banana>(FruitInt64Type.Banana)
                    .When<Orange>(FruitInt64Type.Orange)
                    .When<Avocado>(FruitInt64Type.Avocado));
        }

        private static void ConfigureFruitInt16Factory(IServiceCollection services)
        {
            services.AddTransient<Banana>();
            services.AddTransient<Orange>();
            services.AddTransient<Pineapple>();
            services.AddTransient<Blueberry>();
            services.AddTransient<Strawberry>();
            services.AddTransient<Apple>();
            services.AddTransient<Mango>();
            services.AddTransient<Avocado>();

            services.AddSimpleFactory()
                .Of<IFruit, FruitInt16Type>(_ => _
                    .When<Banana>(FruitInt16Type.Banana)
                    .When<Orange>(FruitInt16Type.Orange)
                    .When<Pineapple>(FruitInt16Type.Pineapple)
                    .When<Blueberry>(FruitInt16Type.Blueberry)
                    .When<Strawberry>(FruitInt16Type.Strawberry)
                    .When<Apple>(FruitInt16Type.Apple)
                    .When<Mango>(FruitInt16Type.Mango)
                    .When<Avocado>(FruitInt16Type.Avocado));
        }
    }
}