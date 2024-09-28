using Microsoft.Extensions.DependencyInjection;
using SimpleFactory.Core.Abstractions;
using SimpleFactory.Core.Extensions;

namespace SimpleFactory.App
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            var serviceProvider = serviceCollection.BuildServiceProvider();

            var fruitFactory = serviceProvider.GetRequiredService<IFactory<int, IFruit>>();
            var fruit = fruitFactory.New(0);

            Console.WriteLine($"You chose fruit name: {fruit?.Name}");

            Console.WriteLine("Press any key to exit!");
            Console.ReadKey();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<Banana>();
            services.AddTransient<Apple>();
            services.AddTransient<Pineapple>();
            services.AddTransient<Orange>();

            services.AddSimpleFactory()
                .Of<IFruit, int>(_ => _
                    .When<Banana>((int)FruitType.Banana)
                    .When<Apple>((int)FruitType.Apple)
                    .When<Orange>((int)FruitType.Orange)
                    .Otherwise<Pineapple>());
        }
    }
}
