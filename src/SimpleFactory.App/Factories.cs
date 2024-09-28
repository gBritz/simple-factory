namespace SimpleFactory.App.Factories
{
    public class FactoryOfInt32(IServiceProvider serviceProvider)
    {
        public IFruit? New_1(int type)
        {
            return type switch
            {
                0 => (IFruit)serviceProvider.GetService(typeof(Banana)),
                _ => null
            };
        }

        public IFruit? New_2(int type)
        {
            return type switch
            {
                0 => (IFruit)serviceProvider.GetService(typeof(Banana)),
                1 => (IFruit)serviceProvider.GetService(typeof(Apple)),
                _ => null
            };
        }

        public IFruit? New_3(int type)
        {
            return type switch
            {
                0 => (IFruit)serviceProvider.GetService(typeof(Banana)),
                1 => (IFruit)serviceProvider.GetService(typeof(Apple)),
                2 => (IFruit)serviceProvider.GetService(typeof(Orange)),
                _ => null
            };
        }

        public IFruit? New_1_With_5(int type)
        {
            return type switch
            {
                5 => (IFruit)serviceProvider.GetService(typeof(Banana)),
                _ => null
            };
        }

        public IFruit? New_4_OutOfInterval(int type)
        {
            switch (type)
            {
                case 0:
                    return (IFruit)serviceProvider.GetService(typeof(Banana));
                case 1:
                    return (IFruit)serviceProvider.GetService(typeof(Apple));
                case 2:
                    return (IFruit)serviceProvider.GetService(typeof(Orange));
                case 12:
                    return (IFruit)serviceProvider.GetService(typeof(Banana));
                default:
                    // code block
                    return null;
            }
        }

        public IFruit? New_4_OutOfInterval_NonStartWithZero(int type)
        {
            switch (type)
            {
                case 1:
                    return (IFruit)serviceProvider.GetService(typeof(Apple));
                case 2:
                    return (IFruit)serviceProvider.GetService(typeof(Orange));
                case 12:
                    return (IFruit)serviceProvider.GetService(typeof(Banana));
                case 13:
                    return (IFruit)serviceProvider.GetService(typeof(Banana));
                default:
                    // code block
                    return null;
            }
        }

        public IFruit? New_5_OutOfInterval(int type)
        {
            switch (type)
            {
                case 0:
                    return (IFruit)serviceProvider.GetService(typeof(Banana));
                case 1:
                    return (IFruit)serviceProvider.GetService(typeof(Apple));
                case 2:
                    return (IFruit)serviceProvider.GetService(typeof(Orange));
                case 12:
                    return (IFruit)serviceProvider.GetService(typeof(Banana));
                case 13:
                    return (IFruit)serviceProvider.GetService(typeof(Banana));
                default:
                    // code block
                    return null;
            }
        }

        public IFruit? New_5_OutOfInterval_NonStartWithZero(int type)
        {
            switch (type)
            {
                case 1:
                    return (IFruit)serviceProvider.GetService(typeof(Apple));
                case 2:
                    return (IFruit)serviceProvider.GetService(typeof(Orange));
                case 3:
                    return (IFruit)serviceProvider.GetService(typeof(Banana));
                case 12:
                    return (IFruit)serviceProvider.GetService(typeof(Banana));
                case 13:
                    return (IFruit)serviceProvider.GetService(typeof(Banana));
                default:
                    // code block
                    return null;
            }
        }

        public IFruit? New_6_OutOfInterval(int type)
        {
            switch (type)
            {
                case 0:
                    return (IFruit)serviceProvider.GetService(typeof(Banana));
                case 1:
                    return (IFruit)serviceProvider.GetService(typeof(Apple));
                case 2:
                    return (IFruit)serviceProvider.GetService(typeof(Orange));
                case 12:
                    return (IFruit)serviceProvider.GetService(typeof(Banana));
                case 13:
                    return (IFruit)serviceProvider.GetService(typeof(Banana));
                case 14:
                    return (IFruit)serviceProvider.GetService(typeof(Banana));
                default:
                    // code block
                    return null;
            }
        }

        public IFruit? New_9_OutOfInterval(int type)
        {
            switch (type)
            {
                case 0:
                    return (IFruit)serviceProvider.GetService(typeof(Banana));
                case 1:
                    return (IFruit)serviceProvider.GetService(typeof(Apple));
                case 2:
                    return (IFruit)serviceProvider.GetService(typeof(Orange));
                case 12:
                    return (IFruit)serviceProvider.GetService(typeof(Banana));
                case 13:
                    return (IFruit)serviceProvider.GetService(typeof(Banana));
                case 14:
                    return (IFruit)serviceProvider.GetService(typeof(Banana));
                case 23:
                    return (IFruit)serviceProvider.GetService(typeof(Banana));
                case 25:
                    return (IFruit)serviceProvider.GetService(typeof(Banana));
                case 28:
                    return (IFruit)serviceProvider.GetService(typeof(Banana));
                default:
                    // code block
                    return null;
            }
        }

        public IFruit? New_16(int type)
        {
            switch (type)
            {
                case 0:
                    return (IFruit)serviceProvider.GetService(typeof(Banana));
                case 1:
                    return (IFruit)serviceProvider.GetService(typeof(Apple));
                case 2:
                    return (IFruit)serviceProvider.GetService(typeof(Orange));
                case 3:
                    return (IFruit)serviceProvider.GetService(typeof(Banana));
                case 4:
                    return (IFruit)serviceProvider.GetService(typeof(Apple));
                case 5:
                    return (IFruit)serviceProvider.GetService(typeof(Orange));
                case 6:
                    return (IFruit)serviceProvider.GetService(typeof(Banana));
                case 7:
                    return (IFruit)serviceProvider.GetService(typeof(Apple));
                case 8:
                    return (IFruit)serviceProvider.GetService(typeof(Orange));
                case 9:
                    return (IFruit)serviceProvider.GetService(typeof(Banana));
                case 10:
                    return (IFruit)serviceProvider.GetService(typeof(Apple));
                case 11:
                    return (IFruit)serviceProvider.GetService(typeof(Orange));
                case 12:
                    return (IFruit)serviceProvider.GetService(typeof(Banana));
                case 13:
                    return (IFruit)serviceProvider.GetService(typeof(Apple));
                case 14:
                    return (IFruit)serviceProvider.GetService(typeof(Orange));
                case 15:
                    return (IFruit)serviceProvider.GetService(typeof(Banana));
                case 16:
                    return (IFruit)serviceProvider.GetService(typeof(Apple));
                default:
                    // code block
                    return null;
            }
        }

        public IFruit? New_DebugError(int type)
        {
            switch (type)
            {
                case 0:
                    return (IFruit)serviceProvider.GetService(typeof(Banana));
                case 1:
                    return (IFruit)serviceProvider.GetService(typeof(Orange));
                case 2:
                    return (IFruit)serviceProvider.GetService(typeof(Pineapple));
                case 12:
                    return (IFruit)serviceProvider.GetService(typeof(Apple));
                case 13:
                    return (IFruit)serviceProvider.GetService(typeof(Orange));
                case 26:
                    return (IFruit)serviceProvider.GetService(typeof(Orange));
                case 51:
                    return (IFruit)serviceProvider.GetService(typeof(Banana));
                default:
                    // code block
                    return null;
            }
        }

        public IFruit? New_DebugError2(int type)
        {
            switch (type)
            {
                case 12:
                    return (IFruit)serviceProvider.GetService(typeof(Orange));
                case 13:
                    return (IFruit)serviceProvider.GetService(typeof(Banana));
                case 0:
                    return (IFruit)serviceProvider.GetService(typeof(Apple));
                case 1:
                    return (IFruit)serviceProvider.GetService(typeof(Orange));
                case 24:
                    return (IFruit)serviceProvider.GetService(typeof(Banana));
                case 25:
                    return (IFruit)serviceProvider.GetService(typeof(Orange));
                case 26:
                    return (IFruit)serviceProvider.GetService(typeof(Pineapple));

                default:
                    // code block
                    return null;
            }
        }
    }

    public class FactoryOfInt64
    {
        private readonly IServiceProvider _serviceProvider;

        public FactoryOfInt64(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IFruit? New_1(long type)
        {
            return type switch
            {
                long.MaxValue => (IFruit)_serviceProvider.GetService(typeof(Banana)),
                _ => null
            };
        }

        public IFruit? New_1(short type)
        {
            return type switch
            {
                short.MaxValue => (IFruit)_serviceProvider.GetService(typeof(Banana)),
                _ => null
            };
        }

        public IFruit? New_2(long type)
        {
            return type switch
            {
                0 => (IFruit)_serviceProvider.GetService(typeof(Banana)),
                1 => (IFruit)_serviceProvider.GetService(typeof(Orange)),
                _ => null
            };
        }
    }

    public class FactoryOfFruitType(IServiceProvider serviceProvider)
    {
        public IFruit? New_1(FruitType type)
        {
            return type switch
            {
                FruitType.Banana => (IFruit)serviceProvider.GetService(typeof(Banana)),
                _ => null
            };
        }

        public IFruit? New_2(FruitType type)
        {
            return type switch
            {
                FruitType.Banana => (IFruit)serviceProvider.GetService(typeof(Banana)),
                FruitType.Orange => (IFruit)serviceProvider.GetService(typeof(Orange)),
                _ => null
            };
        }

        public IFruit? New_3(FruitType type)
        {
            return type switch
            {
                FruitType.Banana => (IFruit)serviceProvider.GetService(typeof(Banana)),
                FruitType.Orange => (IFruit)serviceProvider.GetService(typeof(Orange)),
                FruitType.Pineapple => (IFruit)serviceProvider.GetService(typeof(Pineapple)),
                _ => null
            };
        }

        public IFruit? New_4_OutOfInterval(FruitType type)
        {
            switch (type)
            {
                case FruitType.Banana:
                    return (IFruit)serviceProvider.GetService(typeof(Banana));
                case FruitType.Orange:
                    return (IFruit)serviceProvider.GetService(typeof(Orange));
                case FruitType.Pineapple:
                    return (IFruit)serviceProvider.GetService(typeof(Pineapple));
                case FruitType.Apple:
                    return (IFruit)serviceProvider.GetService(typeof(Apple));
                default:
                    // code block
                    return null;
            }
        }

        public IFruit? New_4_OutOfInterval_NonStartWithZero(FruitType type)
        {
            switch (type)
            {
                case FruitType.Orange:
                    return (IFruit)serviceProvider.GetService(typeof(Orange));
                case FruitType.Pineapple:
                    return (IFruit)serviceProvider.GetService(typeof(Pineapple));
                case FruitType.Blueberry:
                    return (IFruit)serviceProvider.GetService(typeof(Blueberry));
                case FruitType.Avocado:
                    return (IFruit)serviceProvider.GetService(typeof(Avocado));
                default:
                    // code block
                    return null;
            }
        }

        public IFruit? New_5_OutOfInterval(FruitType type)
        {
            switch (type)
            {
                case FruitType.Banana:
                    return (IFruit)serviceProvider.GetService(typeof(Banana));
                case FruitType.Orange:
                    return (IFruit)serviceProvider.GetService(typeof(Orange));
                case FruitType.Pineapple:
                    return (IFruit)serviceProvider.GetService(typeof(Pineapple));
                case FruitType.Mango:
                    return (IFruit)serviceProvider.GetService(typeof(Mango));
                case FruitType.Blueberry:
                    return (IFruit)serviceProvider.GetService(typeof(Blueberry));
                default:
                    // code block
                    return null;
            }
        }

        public IFruit? New_5_OutOfInterval_NonStartWithZero(FruitType type)
        {
            switch (type)
            {
                case FruitType.Orange:
                    return (IFruit)serviceProvider.GetService(typeof(Orange));
                case FruitType.Pineapple:
                    return (IFruit)serviceProvider.GetService(typeof(Pineapple));
                case FruitType.Apple:
                    return (IFruit)serviceProvider.GetService(typeof(Apple));
                case FruitType.Blueberry:
                    return (IFruit)serviceProvider.GetService(typeof(Blueberry));
                default:
                    // code block
                    return null;
            }
        }

        public IFruit? New_6_OutOfInterval(FruitType type)
        {
            switch (type)
            {
                case FruitType.Banana:
                    return (IFruit)serviceProvider.GetService(typeof(Banana));
                case FruitType.Orange:
                    return (IFruit)serviceProvider.GetService(typeof(Orange));
                case FruitType.Pineapple:
                    return (IFruit)serviceProvider.GetService(typeof(Pineapple));
                case FruitType.Mango:
                    return (IFruit)serviceProvider.GetService(typeof(Mango));
                case FruitType.Blueberry:
                    return (IFruit)serviceProvider.GetService(typeof(Blueberry));
                case FruitType.Avocado:
                    return (IFruit)serviceProvider.GetService(typeof(Avocado));
                default:
                    // code block
                    return null;
            }
        }

        public IFruit? New_DebugError2(FruitType type)
        {
            switch (type)
            {
                case FruitType.Orange:
                    return (IFruit)serviceProvider.GetService(typeof(Orange));
                case FruitType.Pineapple:
                    return (IFruit)serviceProvider.GetService(typeof(Pineapple));
                case FruitType.Apple:
                    return (IFruit)serviceProvider.GetService(typeof(Apple));
                //case FruitType.Banana:
                //    return (IFruit)serviceProvider.GetService(typeof(Banana));
                //case FruitType.Orange:
                //    return (IFruit)serviceProvider.GetService(typeof(Orange));
                case FruitType.Avocado:
                    return (IFruit)serviceProvider.GetService(typeof(Avocado));
                case FruitType.Blueberry:
                    return (IFruit)serviceProvider.GetService(typeof(Blueberry));
                case FruitType.Mango:
                    return (IFruit)serviceProvider.GetService(typeof(Mango));
                default:
                    // code block
                    return null;
            }
        }

        public IFruit? New_Int16(FruitInt16Type type)
        {
            switch (type)
            {
                /*
                case FruitInt16Type.Banana:
                    return (IFruit)serviceProvider.GetService(typeof(Banana));
                case FruitInt16Type.Orange:
                    return (IFruit)serviceProvider.GetService(typeof(Orange));
                case FruitInt16Type.Blueberry:
                    return (IFruit)serviceProvider.GetService(typeof(Blueberry));
                case FruitInt16Type.Apple:
                    return (IFruit)serviceProvider.GetService(typeof(Apple));
                case FruitInt16Type.Mango:
                    return (IFruit)serviceProvider.GetService(typeof(Mango));
                case FruitInt16Type.Avocado:
                    return (IFruit)serviceProvider.GetService(typeof(Avocado));
                */
                case FruitInt16Type.Banana:
                    return (IFruit)serviceProvider.GetService(typeof(Banana));
                case FruitInt16Type.Orange:
                    return (IFruit)serviceProvider.GetService(typeof(Orange));
                case FruitInt16Type.Blueberry:
                    return (IFruit)serviceProvider.GetService(typeof(Blueberry));
                case FruitInt16Type.Blueberry2:
                    return (IFruit)serviceProvider.GetService(typeof(Blueberry));
                case FruitInt16Type.Mango:
                    return (IFruit)serviceProvider.GetService(typeof(Mango));
                case FruitInt16Type.Apple:
                    return (IFruit)serviceProvider.GetService(typeof(Apple));
                case FruitInt16Type.Apple2:
                    return (IFruit)serviceProvider.GetService(typeof(Apple));
                default:
                    // code block
                    return null;
            }
        }
    }

    public class FactoryOfChar
    {
        private readonly IServiceProvider _serviceProvider;

        public FactoryOfChar(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IFruit? New_1(char type)
        {
            return type switch
            {
                'B' => (IFruit)_serviceProvider.GetService(typeof(Banana)),
                _ => null
            };
        }

        public IFruit? New_2(char type)
        {
            return type switch
            {
                'B' => (IFruit)_serviceProvider.GetService(typeof(Banana)),
                'O' => (IFruit)_serviceProvider.GetService(typeof(Orange)),
                'P' => (IFruit)_serviceProvider.GetService(typeof(Pineapple)),
                'A' => (IFruit)_serviceProvider.GetService(typeof(Apple)),
                'S' => (IFruit)_serviceProvider.GetService(typeof(Strawberry)),
                'M' => (IFruit)_serviceProvider.GetService(typeof(Mango)),
                'C' => (IFruit)_serviceProvider.GetService(typeof(Blueberry)),
                'D' => (IFruit)_serviceProvider.GetService(typeof(Avocado)),
                _ => null
            };
        }
    }

    public class FactoryOfString
    {
        private readonly IServiceProvider _serviceProvider;

        public FactoryOfString(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IFruit? New_1(string type)
        {
            return type switch
            {
                "B" => (IFruit)_serviceProvider.GetService(typeof(Banana)),
                _ => null
            };
        }

        public IFruit? New_2(string type)
        {
            return type switch
            {
                "banana" => (IFruit)_serviceProvider.GetService(typeof(Banana)),
                "orange" => (IFruit)_serviceProvider.GetService(typeof(Orange)),
                "pineapple" => (IFruit)_serviceProvider.GetService(typeof(Pineapple)),
                "apple" => (IFruit)_serviceProvider.GetService(typeof(Apple)),
                "appl2" => (IFruit)_serviceProvider.GetService(typeof(Apple)),
                "strawberry" => (IFruit)_serviceProvider.GetService(typeof(Strawberry)),
                "mango" => (IFruit)_serviceProvider.GetService(typeof(Mango)),
                "blueberry" => (IFruit)_serviceProvider.GetService(typeof(Blueberry)),
                "avocado" => (IFruit)_serviceProvider.GetService(typeof(Avocado)),
                _ => null
            };
        }
    }
}
