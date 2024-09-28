namespace SimpleFactory.Core.Tests.Domain
{
    public interface IFruit
    {
        string Name { get; }
    }

    public class Banana : IFruit
    {
        public string Name => "Banana";
    }

    public class Orange : IFruit
    {
        public string Name => "Orange";
    }

    public class Pineapple : IFruit
    {
        public string Name => "Pineapple";
    }

    public class Apple : IFruit
    {
        public string Name => "Apple";
    }

    public class Strawberry : IFruit
    {
        public string Name => "Strawberry";
    }

    public class Mango : IFruit
    {
        public string Name => "Mango";
    }

    public class Blueberry : IFruit
    {
        public string Name => "Blueberry";
    }

    public class Avocado : IFruit
    {
        public string Name => "Avocado";
    }

    public interface ICar
    {
        string Name { get; }
    }

    public class Bmw : ICar
    {
        public string Name => "Bmw";
    }

    public class Citroen : ICar
    {
        public string Name => "Citroën";
    }

    public class Dodge : ICar
    {
        public string Name => "Dodge";
    }

    public class Jeep : ICar
    {
        public string Name => "Jeep";
    }

    public enum CarType
    {
        Bmw,

        Citroen,

        Dodge,

        Jeep,
    }

    public enum FruitType
    {
        Banana,

        Orange,

        Pineapple,

        Apple,

        Strawberry,

        Mango,

        Blueberry,

        Avocado,
    }

    public enum FruitCharType
    {
        Banana = 'B',

        Orange = 'O',

        Pineapple = 'P',

        Apple = 'A',

        Strawberry = 'S',

        Mango = 'M',

        Blueberry = 'B',

        Avocado = 'V',
    }

    public enum FruitInt64Type : long
    {
        Banana = 0,

        Orange = 1,

        Avocado = long.MaxValue,
    }

    public enum FruitInt16Type : short
    {
        Banana = 0,

        Orange = -127,

        Pineapple = -128,

        Blueberry = -129,

        Strawberry = 126,

        Apple = 127,

        Mango = 128,

        Avocado = short.MaxValue,
    }
}