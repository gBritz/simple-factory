using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using SimpleFactory.Core.Abstractions;
using SimpleFactory.Core.Extensions;
using SimpleFactory.Core.Tests.Domain;

namespace SimpleFactory.Core.Tests
{
    public class FactoryByInt32Test
    {
        [Fact]
        public void GivenNoFruitServicesRegistered_WhenCreateFruitByType_ResultShouldBeNull()
        {
            var fruitFactory = CreateFruitFactory(ConfigureFruitFactoryWithWithoutServices);

            var fruit = fruitFactory.New((int)FruitType.Banana);

            fruit.Should().BeNull();
        }

        [Fact]
        public void GivenOneFruitServiceRegistered_WhenCreateFruitByType_ResultShouldNotBeNull()
        {
            var fruitFactory = CreateFruitFactory(ConfigureFruitFactoryWithOneService);

            var fruit = fruitFactory.New((int)FruitType.Banana);

            fruit.Should().NotBeNull();
            fruit.Should().BeOfType(typeof(Banana));
        }

        [Theory]
        [InlineData((int)FruitType.Banana, typeof(Banana))]
        [InlineData((int)FruitType.Orange, typeof(Orange))]
        public void GivenTwoFruitsServicesRegistered_WhenCreateFruitByType_ResultShouldNotBeNull(
            int numberOfFruitType,
            Type typeImplementation)
        {
            var fruitFactory = CreateFruitFactory(ConfigureFruitFactoryWithTwoServices);

            var fruit = fruitFactory.New(numberOfFruitType);

            fruit.Should().NotBeNull();
            fruit.Should().BeOfType(typeImplementation);
        }

        [Theory]
        [InlineData((int)FruitType.Banana, typeof(Banana))]
        [InlineData((int)FruitType.Orange, typeof(Orange))]
        [InlineData((int)FruitType.Pineapple, typeof(Pineapple))]
        public void GivenThreeFruitsServicesRegistered_WhenCreateFruitByType_ResultShouldNotBeNull(
            int numberOfFruitType,
            Type typeImplementation)
        {
            var fruitFactory = CreateFruitFactory(ConfigureFruitFactoryWithThreeServices);

            var fruit = fruitFactory.New(numberOfFruitType);

            fruit.Should().NotBeNull();
            fruit.Should().BeOfType(typeImplementation);
        }

        [Theory]
        [InlineData((int)FruitType.Banana, typeof(Banana))]
        [InlineData((int)FruitType.Orange, typeof(Orange))]
        [InlineData((int)FruitType.Pineapple, typeof(Pineapple))]
        [InlineData((int)FruitType.Apple, typeof(Apple))]
        [InlineData((int)FruitType.Strawberry, typeof(Strawberry))]
        [InlineData((int)FruitType.Mango, typeof(Mango))]
        public void GivenManyFruitsServicesRegistered_WhenCreateFruitByType_ResultShouldNotBeNull(
            int numberOfFruitType,
            Type typeImplementation)
        {
            var fruitFactory = CreateFruitFactory(ConfigureFruitFactoryWithManyServices);

            var fruit = fruitFactory.New(numberOfFruitType);

            fruit.Should().NotBeNull();
            fruit.Should().BeOfType(typeImplementation);
        }

        [Theory]
        [InlineData((int)FruitType.Banana, typeof(Banana))]
        [InlineData((int)FruitType.Orange, typeof(Orange))]
        [InlineData((int)FruitType.Pineapple, typeof(Pineapple))]
        public void GivenThreeFruitsServicesRegisteredWithDescendingOrder_WhenCreateFruitByType_ResultShouldNotBeNull(
            int numberOfFruitType,
            Type typeImplementation)
        {
            var fruitFactory = CreateFruitFactory(ConfigureFruitFactoryWithThreeServicesDescendingOrder);

            var fruit = fruitFactory.New(numberOfFruitType);

            fruit.Should().NotBeNull();
            fruit.Should().BeOfType(typeImplementation);
        }

        [Theory]
        [InlineData((int)FruitType.Orange, typeof(Orange))]
        [InlineData((int)FruitType.Apple, typeof(Apple))]
        public void GivenTwoFruitsServicesRegisteredOutOfInterval_WhenCreateFruitByType_ResultShouldNotBeNull(
            int numberOfFruitType,
            Type typeImplementation)
        {
            var fruitFactory = CreateFruitFactory(ConfigureFruitFactoryWithTwoServicesOutOfInterval);

            var fruit = fruitFactory.New(numberOfFruitType);

            fruit.Should().NotBeNull();
            fruit.Should().BeOfType(typeImplementation);
        }

        [Theory]
        [InlineData((int)FruitType.Banana, typeof(Banana))]
        [InlineData((int)FruitType.Pineapple, typeof(Pineapple))]
        [InlineData((int)FruitType.Mango, typeof(Mango))]
        public void GivenThreeFruitsServicesRegisteredOutOfInterval_WhenCreateFruitByType_ResultShouldNotBeNull(
            int numberOfFruitType,
            Type typeImplementation)
        {
            var fruitFactory = CreateFruitFactory(ConfigureFruitFactoryWithThreeServicesOutOfInterval);

            var fruit = fruitFactory.New(numberOfFruitType);

            fruit.Should().NotBeNull();
            fruit.Should().BeOfType(typeImplementation);
        }

        [Theory]
        [InlineData(0, typeof(Banana))]
        [InlineData(1, typeof(Orange))]
        [InlineData(2, typeof(Pineapple))]
        [InlineData(12, typeof(Apple))]
        [InlineData(13, typeof(Strawberry))]
        [InlineData(14, typeof(Mango))]
        public void GivenTwoGroupsOfFruitsServicesRegistered_WhenCreateFruitByType_ResultShouldNotBeNull(
            int numberOfFruitType,
            Type typeImplementation)
        {
            var fruitFactory = CreateFruitFactory(ConfigureTwoGroupsOfFruits);

            var fruit = fruitFactory.New(numberOfFruitType);

            fruit.Should().NotBeNull();
            fruit.Should().BeOfType(typeImplementation);
        }

        [Theory]
        [InlineData(2, typeof(Banana))]
        [InlineData(13, typeof(Pineapple))]
        [InlineData(66, typeof(Apple))]
        public void GivenThreeGroupsOfFruitsServicesRegistered_WhenCreateFruitByType_ResultShouldNotBeNull(
            int numberOfFruitType,
            Type typeImplementation)
        {
            var fruitFactory = CreateFruitFactory(ConfigureThreeGroupsOfFruits);

            var fruit = fruitFactory.New(numberOfFruitType);

            fruit.Should().NotBeNull();
            fruit.Should().BeOfType(typeImplementation);
        }

        [Theory]
        [InlineData(0, typeof(Banana))]
        [InlineData(1, typeof(Orange))]
        [InlineData(2, typeof(Pineapple))]
        [InlineData(12, typeof(Apple))]
        [InlineData(13, typeof(Strawberry))]
        [InlineData(26, typeof(Mango))]
        [InlineData(51, typeof(Blueberry))]
        public void GivenFourGroupsOfFruitsServicesRegistered_WhenCreateFruitByType_ResultShouldNotBeNull(
            int numberOfFruitType,
            Type typeImplementation)
        {
            var fruitFactory = CreateFruitFactory(ConfigureFourGroupsOfFruits);

            var fruit = fruitFactory.New(numberOfFruitType);

            fruit.Should().NotBeNull();
            fruit.Should().BeOfType(typeImplementation);
        }

        [Theory]
        [InlineData(0, typeof(Banana))]
        [InlineData(1, typeof(Orange))]
        [InlineData(12, typeof(Pineapple))]
        [InlineData(13, typeof(Apple))]
        [InlineData(14, typeof(Strawberry))]
        [InlineData(26, typeof(Mango))]
        [InlineData(51, typeof(Blueberry))]
        public void GivenFourGroupsOfFruits2ServicesRegistered_WhenCreateFruitByType_ResultShouldNotBeNull(
            int numberOfFruitType,
            Type typeImplementation)
        {
            var fruitFactory = CreateFruitFactory(ConfigureFourGroupsOfFruits2);

            var fruit = fruitFactory.New(numberOfFruitType);

            fruit.Should().NotBeNull();
            fruit.Should().BeOfType(typeImplementation);
        }

        [Theory]
        [InlineData(12, typeof(Banana))]
        [InlineData(13, typeof(Orange))]
        [InlineData(0, typeof(Pineapple))]
        [InlineData(1, typeof(Apple))]
        [InlineData(24, typeof(Strawberry))]
        [InlineData(25, typeof(Mango))]
        [InlineData(26, typeof(Blueberry))]
        public void GivenFourGroupsOfFruits3ServicesRegistered_WhenCreateFruitByType_ResultShouldNotBeNull(
            int numberOfFruitType,
            Type typeImplementation)
        {
            var fruitFactory = CreateFruitFactory(ConfigureFourGroupsOfFruits3);

            var fruit = fruitFactory.New(numberOfFruitType);

            fruit.Should().NotBeNull();
            fruit.Should().BeOfType(typeImplementation);
        }

        [Theory]
        [InlineData((int)FruitType.Pineapple, typeof(Pineapple))]
        [InlineData((int)FruitType.Mango, typeof(Pineapple))]
        [InlineData((int)FruitType.Strawberry, typeof(Pineapple))]
        public void GivenSequentialsFruits_WhenTypeIsNotSpecified_ResultShouldBeOtherwise(
            int numberOfFruitType,
            Type typeImplementation)
        {
            var fruitFactory = CreateFruitFactory(ConfigureSequentialFruitsWithOtherwise);

            var fruit = fruitFactory.New(numberOfFruitType);

            fruit.Should().NotBeNull();
            fruit.Should().BeOfType(typeImplementation);
        }

        [Theory]
        [InlineData((int)FruitType.Apple, typeof(Pineapple))]
        [InlineData((int)FruitType.Orange, typeof(Pineapple))]
        [InlineData((int)FruitType.Pineapple, typeof(Pineapple))]
        public void GivenTwoNonSequentialsFruits_WhenTypeIsNotSpecified_ResultShouldBeOtherwise(
            int numberOfFruitType,
            Type typeImplementation)
        {
            var fruitFactory = CreateFruitFactory(ConfigureTwoNonSequentialFruitsWithOtherwise);

            var fruit = fruitFactory.New(numberOfFruitType);

            fruit.Should().NotBeNull();
            fruit.Should().BeOfType(typeImplementation);
        }

        [Theory]
        [InlineData((int)FruitType.Apple, typeof(Pineapple))]
        [InlineData((int)FruitType.Orange, typeof(Pineapple))]
        [InlineData((int)FruitType.Pineapple, typeof(Pineapple))]
        public void GivenThreeNonSequentialsFruits_WhenTypeIsNotSpecified_ResultShouldBeOtherwise(
            int numberOfFruitType,
            Type typeImplementation)
        {
            var fruitFactory = CreateFruitFactory(ConfigureThreeNonSequentialFruitsWithOtherwise);

            var fruit = fruitFactory.New(numberOfFruitType);

            fruit.Should().NotBeNull();
            fruit.Should().BeOfType(typeImplementation);
        }

        [Fact]
        public void GivenSimpleFactoryWithoutOtherwiseConfigurated_WhenPassNotSpecifiedType_ResultShouldBeNull()
        {
            var fruitFactory = CreateFruitFactory(ConfigureFruitFactoryWithoutOtherwise);

            var fruit = fruitFactory.New((int)FruitType.Pineapple);

            fruit.Should().BeNull();
        }

        [Theory]
        [InlineData((int)FruitType.Banana, typeof(Banana))]
        [InlineData((int)FruitType.Orange, typeof(Orange))]
        [InlineData((int)FruitType.Pineapple, typeof(Pineapple))]
        [InlineData((int)FruitType.Apple, typeof(Apple))]
        [InlineData((int)CarType.Bmw, typeof(Bmw))]
        [InlineData((int)CarType.Citroen, typeof(Citroen))]
        [InlineData((int)CarType.Dodge, typeof(Dodge))]
        [InlineData((int)CarType.Jeep, typeof(Jeep))]
        public void GivenFruitsAndCars_WhenCreateByType_ResultShouldBeNotNull(
            int numberOfImplementationType,
            Type typeImplementation)
        {
            object instanceResult;
            var isFruitType = typeof(IFruit).IsAssignableFrom(typeImplementation);
            var serviceProvider = BuildServiceProvider(ConfigureFruitsAndCarsFactory);

            if (isFruitType)
            {
                var fruitFactory = serviceProvider.GetRequiredService<IFactory<int, IFruit>>();
                instanceResult = fruitFactory.New(numberOfImplementationType);
            }
            else
            {
                var carFactory = serviceProvider.GetRequiredService<IFactory<int, ICar>>();
                instanceResult = carFactory.New(numberOfImplementationType);
            }

            instanceResult.Should().NotBeNull();
            instanceResult.Should().BeOfType(typeImplementation);
        }

        /*
        [Fact]
        public void GivenSimpleFactoryWithoutOtherwiseConfigurated_WhenTryNewRequiredAndPassNotSpecifiedType_ShouldBeThrowInvalidOperationException()
        {
            var fruitFactory = CreateFruitFactory(ConfigureFruitFactoryWithoutOtherwise);

            Action call = () => fruitFactory.NewRequired((int)FruitType.Orange);

            call.Should().Throw<InvalidOperationException>()
                .WithMessage("Not found configurated service of FruitType as Orange.");
        }
        */

        private static IFactory<int, IFruit> CreateFruitFactory(Action<IServiceCollection> configureServices) =>
            BuildServiceProvider(configureServices).GetRequiredService<IFactory<int, IFruit>>();

        private static ServiceProvider BuildServiceProvider(Action<IServiceCollection> configureServices)
        {
            ArgumentNullException.ThrowIfNull(configureServices, nameof(configureServices));

            var serviceCollection = new ServiceCollection();
            configureServices(serviceCollection);
            return serviceCollection.BuildServiceProvider();
        }

        private static void ConfigureFruitFactoryWithWithoutServices(IServiceCollection services)
        {
            services.AddSimpleFactory()
                .Of<IFruit, int>();
        }

        private static void ConfigureFruitFactoryWithOneService(IServiceCollection services)
        {
            services.AddTransient<Banana>();

            services.AddSimpleFactory()
                .Of<IFruit, int>(_ => _
                    .When<Banana>((int)FruitType.Banana));
        }

        private static void ConfigureFruitFactoryWithTwoServices(IServiceCollection services)
        {
            services.AddTransient<Banana>();
            services.AddTransient<Orange>();

            services.AddSimpleFactory()
                .Of<IFruit, int>(_ => _
                    .When<Banana>((int)FruitType.Banana)
                    .When<Orange>((int)FruitType.Orange));
        }

        private static void ConfigureFruitFactoryWithThreeServices(IServiceCollection services)
        {
            services.AddTransient<Banana>();
            services.AddTransient<Orange>();
            services.AddTransient<Pineapple>();

            services.AddSimpleFactory()
                .Of<IFruit, int>(_ => _
                    .When<Banana>((int)FruitType.Banana)
                    .When<Orange>((int)FruitType.Orange)
                    .When<Pineapple>((int)FruitType.Pineapple));
        }

        private static void ConfigureFruitFactoryWithThreeServicesDescendingOrder(IServiceCollection services)
        {
            services.AddTransient<Banana>();
            services.AddTransient<Orange>();
            services.AddTransient<Pineapple>();

            services.AddSimpleFactory()
                .Of<IFruit, int>(_ => _
                    .When<Pineapple>((int)FruitType.Pineapple)
                    .When<Orange>((int)FruitType.Orange)
                    .When<Banana>((int)FruitType.Banana));
        }

        private static void ConfigureFruitFactoryWithTwoServicesOutOfInterval(IServiceCollection services)
        {
            services.AddTransient<Orange>();
            services.AddTransient<Apple>();

            services.AddSimpleFactory()
                .Of<IFruit, int>(_ => _
                    .When<Orange>((int)FruitType.Orange)
                    .When<Apple>((int)FruitType.Apple));
        }

        private static void ConfigureFruitFactoryWithThreeServicesOutOfInterval(IServiceCollection services)
        {
            services.AddTransient<Banana>();
            services.AddTransient<Pineapple>();
            services.AddTransient<Mango>();

            services.AddSimpleFactory()
                .Of<IFruit, int>(_ => _
                    .When<Pineapple>((int)FruitType.Pineapple)
                    .When<Banana>((int)FruitType.Banana)
                    .When<Mango>((int)FruitType.Mango));
        }

        private static void ConfigureTwoGroupsOfFruits(IServiceCollection services)
        {
            services.AddTransient<Banana>();
            services.AddTransient<Orange>();
            services.AddTransient<Pineapple>();
            services.AddTransient<Apple>();
            services.AddTransient<Strawberry>();
            services.AddTransient<Mango>();

            services.AddSimpleFactory()
                .Of<IFruit, int>(_ => _
                    .When<Banana>(0)
                    .When<Orange>(1)
                    .When<Pineapple>(2)
                    .When<Apple>(12)
                    .When<Strawberry>(13)
                    .When<Mango>(14));
        }

        private static void ConfigureThreeGroupsOfFruits(IServiceCollection services)
        {
            services.AddTransient<Banana>();
            services.AddTransient<Pineapple>();
            services.AddTransient<Apple>();

            services.AddSimpleFactory()
                .Of<IFruit, int>(_ => _
                    .When<Banana>(2)
                    .When<Pineapple>(13)
                    .When<Apple>(66));
        }

        private static void ConfigureFourGroupsOfFruits(IServiceCollection services)
        {
            services.AddTransient<Banana>();
            services.AddTransient<Orange>();
            services.AddTransient<Pineapple>();
            services.AddTransient<Apple>();
            services.AddTransient<Strawberry>();
            services.AddTransient<Mango>();
            services.AddTransient<Blueberry>();

            services.AddSimpleFactory()
                .Of<IFruit, int>(_ => _
                    .When<Banana>(0)
                    .When<Orange>(1)
                    .When<Pineapple>(2)
                    .When<Apple>(12)
                    .When<Strawberry>(13)
                    .When<Mango>(26)
                    .When<Blueberry>(51));
        }

        private static void ConfigureFourGroupsOfFruits2(IServiceCollection services)
        {
            services.AddTransient<Banana>();
            services.AddTransient<Orange>();
            services.AddTransient<Pineapple>();
            services.AddTransient<Apple>();
            services.AddTransient<Strawberry>();
            services.AddTransient<Mango>();
            services.AddTransient<Blueberry>();

            services.AddSimpleFactory()
                .Of<IFruit, int>(_ => _
                    .When<Banana>(0)
                    .When<Orange>(1)
                    .When<Pineapple>(12)
                    .When<Apple>(13)
                    .When<Strawberry>(14)
                    .When<Mango>(26)
                    .When<Blueberry>(51));
        }

        private static void ConfigureFourGroupsOfFruits3(IServiceCollection services)
        {
            services.AddTransient<Banana>();
            services.AddTransient<Orange>();
            services.AddTransient<Pineapple>();
            services.AddTransient<Apple>();
            services.AddTransient<Strawberry>();
            services.AddTransient<Mango>();
            services.AddTransient<Blueberry>();

            services.AddSimpleFactory()
                .Of<IFruit, int>(_ => _
                    .When<Banana>(12)
                    .When<Orange>(13)
                    .When<Pineapple>(0)
                    .When<Apple>(1)
                    .When<Strawberry>(24)
                    .When<Mango>(25)
                    .When<Blueberry>(26));
        }

        private static void ConfigureFruitFactoryWithManyServices(IServiceCollection services)
        {
            services.AddTransient<Banana>();
            services.AddTransient<Orange>();
            services.AddTransient<Pineapple>();
            services.AddTransient<Apple>();
            services.AddTransient<Strawberry>();
            services.AddTransient<Mango>();

            services.AddSimpleFactory()
                .Of<IFruit, int>(_ => _
                    .When<Banana>((int)FruitType.Banana)
                    .When<Orange>((int)FruitType.Orange)
                    .When<Pineapple>((int)FruitType.Pineapple)
                    .When<Apple>((int)FruitType.Apple)
                    .When<Strawberry>((int)FruitType.Strawberry)
                    .When<Mango>((int)FruitType.Mango)
                );
        }

        private static void ConfigureSequentialFruitsWithOtherwise(IServiceCollection services)
        {
            services.AddTransient<Banana>();
            services.AddTransient<Orange>();
            services.AddTransient<Pineapple>();

            services.AddSimpleFactory()
                .Of<IFruit, int>(_ => _
                    .When<Banana>((int)FruitType.Banana)
                    .When<Orange>((int)FruitType.Orange)
                    .Otherwise<Pineapple>()); // TODO: itens fora da ordem também.
        }

        private static void ConfigureTwoNonSequentialFruitsWithOtherwise(IServiceCollection services)
        {
            services.AddTransient<Banana>();
            services.AddTransient<Strawberry>();
            services.AddTransient<Pineapple>();

            services.AddSimpleFactory()
                .Of<IFruit, int>(_ => _
                    .When<Strawberry>((int)FruitType.Strawberry)
                    .When<Banana>((int)FruitType.Banana)
                    .Otherwise<Pineapple>()); // TODO: itens fora da ordem também.
        }

        private static void ConfigureThreeNonSequentialFruitsWithOtherwise(IServiceCollection services)
        {
            services.AddTransient<Banana>();
            services.AddTransient<Strawberry>();
            services.AddTransient<Mango>();
            services.AddTransient<Pineapple>();

            services.AddSimpleFactory()
                .Of<IFruit, int>(_ => _
                    .When<Strawberry>((int)FruitType.Strawberry)
                    .When<Banana>((int)FruitType.Banana)
                    .When<Mango>((int)FruitType.Mango)
                    .Otherwise<Pineapple>());
        }

        private static void ConfigureFruitFactoryWithoutOtherwise(IServiceCollection services)
        {
            services.AddTransient<Banana>();

            services.AddSimpleFactory()
                .Of<IFruit, int>(_ => _
                    .When<Banana>((int)FruitType.Banana));
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
                .Of<IFruit, int>(_ => _
                    .When<Banana>((int)FruitType.Banana)
                    .When<Orange>((int)FruitType.Orange)
                    .When<Pineapple>((int)FruitType.Pineapple)
                    .When<Apple>((int)FruitType.Apple))
                .Of<ICar, int>(_ => _
                    .When<Bmw>((int)CarType.Bmw)
                    .When<Citroen>((int)CarType.Citroen)
                    .When<Dodge>((int)CarType.Dodge)
                    .When<Jeep>((int)CarType.Jeep));
        }
    }
}