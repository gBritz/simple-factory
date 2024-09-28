using Microsoft.Extensions.DependencyInjection;
using SimpleFactory.Core.Abstractions;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.Emit;

namespace SimpleFactory.Core.Extensions
{
    public sealed class SimpleFactoryBuilder
    {
        private readonly IServiceCollection _services;
        private readonly MemberAccessor _memberAccessor = new();
        private readonly FactoryTypeBuilder _factoryTypeBuilder;
        private readonly SimpleFactoryAssemblyInfo _simpleFactoryAssemblyInfo = new();

        public SimpleFactoryBuilder(IServiceCollection services)
        {
            _services = services ?? throw new ArgumentNullException(nameof(services));
            _factoryTypeBuilder = new FactoryTypeBuilder(_memberAccessor, _simpleFactoryAssemblyInfo);
        }

        public SimpleFactoryBuilder Of<TService, TParameter>(Action<SimpleFactoryOptionsBuilder<TService, TParameter>>? configuration = null)
            where TService : notnull
            where TParameter : notnull
        {
            var builder = new SimpleFactoryOptionsBuilder<TService, TParameter>();
            configuration?.Invoke(builder);

            Type factoryType = typeof(IFactory<TParameter, TService>);
            Type resultServiceType = typeof(TService);
            Type parameterType = typeof(TParameter);

            KeyValuePair<NumericEmittingILGenerator, Type>[] numericalMappings = null;
            KeyValuePair<string, Type>[] stringMappings = null;

            if (parameterType.IsEnum)
            {
                var enumUnderlineType = Enum.GetUnderlyingType(parameterType);
                if (enumUnderlineType == _memberAccessor.Int32Type || enumUnderlineType == typeof(short) || enumUnderlineType == typeof(ushort) || enumUnderlineType == typeof(uint))
                {
                    numericalMappings = builder.Mappings
                        .Select(i => new KeyValuePair<NumericEmittingILGenerator, Type>(
                            NumericEmittingILGenerator.OfInt32(Convert.ToInt32(i.Key)),
                            i.Value))
                        .OrderBy(i => i.Key)
                        .ToArray();
                }
                else
                {
                    numericalMappings = builder.Mappings
                        .Select(i => new KeyValuePair<NumericEmittingILGenerator, Type>(
                            NumericEmittingILGenerator.OfInt64(Convert.ToInt64(i.Key)),
                            i.Value))
                        .OrderBy(i => i.Key)
                        .ToArray();
                }
            }
            else if (parameterType == _memberAccessor.Int32Type)
            {
                numericalMappings = builder.Mappings
                    .Select(i => new KeyValuePair<NumericEmittingILGenerator, Type>(
                        NumericEmittingILGenerator.OfInt32(Convert.ToInt32(i.Key)),
                        i.Value))
                    .OrderBy(i => i.Key)
                    .ToArray();
            }
            else if (parameterType == _memberAccessor.Int64Type)
            {
                numericalMappings = builder.Mappings
                    .Select(i => new KeyValuePair<NumericEmittingILGenerator, Type>(
                        NumericEmittingILGenerator.OfInt64(Convert.ToInt64(i.Key)),
                        i.Value))
                    .OrderBy(i => i.Key)
                    .ToArray();
            }
            else if (parameterType == _memberAccessor.StringType)
            {
                stringMappings = builder.Mappings.Cast<KeyValuePair<string, Type>>().ToArray();
            }


            var typeBuilder = _factoryTypeBuilder.CreateType(parameterType, resultServiceType, factoryType);
            var serviceProviderField = _factoryTypeBuilder.CreateConstructor(typeBuilder, resultServiceType);
            var methodBuilder = _factoryTypeBuilder.CreateNewMethod(typeBuilder, factoryType);

            var dynamicMethod = new DynamicILGenerator(_memberAccessor, serviceProviderField, resultServiceType);

            if (builder.Comparison.HasValue)
            {
                dynamicMethod.Use(builder.Comparison.Value);
            }

            if (numericalMappings is not null)
            {
                dynamicMethod.GenerateSwitchImplementation(methodBuilder, numericalMappings, builder.Default);
            }
            else if (stringMappings is not null)
            {
                dynamicMethod.GenerateStringEqualsImplementation(methodBuilder, stringMappings, builder.Default);
            }
            else
            {
                throw new InvalidOperationException($"Not identify mapping for type {parameterType.Name}");
            }

            var dynamicImplementationType = typeBuilder.CreateType();



            //var ilcode = string.Join(Environment.NewLine, dynamicImplementationType.GetMethod("New").GetInstructions());
            //File.WriteAllText(@$"C:\Users\guigu\Desktop\teste\{_simpleFactoryAssemblyInfo.AssemblyBuilder.FullName}.cil", ilcode);



            _services.Add(new ServiceDescriptor(factoryType, dynamicImplementationType, builder.ServiceLifetime));
            //_services.AddTransient<ServiceProviderProxy>();

            return this;
        }
    }



    public static class SimpleFactoryOptionsBuilderExtension
    {
        public static SimpleFactoryOptionsBuilder<TService, string> Comparison<TService>(
            this SimpleFactoryOptionsBuilder<TService, string> builder,
            StringComparison comparison)
            where TService : notnull =>
            builder.AddStringComparison(comparison);
    }

    [DebuggerDisplay("{LongValue}")]
    internal abstract class NumericEmittingILGenerator : IComparable<NumericEmittingILGenerator>
    {
        public abstract void EmitLdc(ILGenerator il);

        public abstract long LongValue { get; }

        public static NumericEmittingILGenerator OfInt32(int value) =>
            new Int32EmittingILGenerator(value);

        public static NumericEmittingILGenerator OfInt64(long value) =>
            new Int64EmittingILGenerator(value);

        public int CompareTo(NumericEmittingILGenerator? other) =>
            other.LongValue < LongValue ? 1 : other.LongValue > LongValue ? -1 : 0;
    }

    internal class Int32EmittingILGenerator : NumericEmittingILGenerator
    {
        private readonly int _value;

        public override long LongValue => _value;

        internal Int32EmittingILGenerator(int value) => _value = value;

        public override void EmitLdc(ILGenerator il)
        {
            //il.Emit(IsValueLdcShortForm(index) ? OpCodes.Ldc_I4_S : OpCodes.Ldc_I4, index);
            il.Emit(OpCodes.Ldc_I4, _value);
        }

        private static bool IsValueLdcShortForm(int num) => num > -127 && num < 126;
    }

    internal class Int64EmittingILGenerator : NumericEmittingILGenerator
    {
        private readonly long _value;

        public override long LongValue => _value;

        internal Int64EmittingILGenerator(long value) => _value = value;

        public override void EmitLdc(ILGenerator il)
        {
            il.Emit(OpCodes.Ldc_I8, _value);
        }
    }

    public class ServiceProviderProxy(IServiceProvider serviceProvider) : IServiceProvider
    {
        public object? GetService(Type serviceType) => serviceProvider.GetService(serviceType);
    }

    internal record SimpleFactoryAssemblyInfo
    {
        public AssemblyBuilder AssemblyBuilder { get; init; }
        public ModuleBuilder ModuleBuilder { get; init; }

        public SimpleFactoryAssemblyInfo()
        {
            var factoryType = typeof(IFactory<object, object>);

            string guid = Guid.NewGuid().ToString();
            var assemblyName = new AssemblyName($"{factoryType.Namespace}.{factoryType.Name}_{guid}");

            AssemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            ModuleBuilder = AssemblyBuilder.DefineDynamicModule(assemblyName.Name);
        }
    }

    internal class FactoryTypeBuilder
    {
        private readonly MemberAccessor _memberAccessor;
        private readonly SimpleFactoryAssemblyInfo _assemblyInfo;

        public FactoryTypeBuilder(
            MemberAccessor memberAccessor,
            SimpleFactoryAssemblyInfo assemblyInfo)
        {
            _memberAccessor = memberAccessor ?? throw new ArgumentNullException(nameof(memberAccessor));
            _assemblyInfo = assemblyInfo ?? throw new ArgumentNullException(nameof(assemblyInfo));
        }

        public TypeBuilder CreateType(Type parameterType, Type resultServiceType, Type implementationType)
        {
            ArgumentNullException.ThrowIfNull(parameterType, nameof(parameterType));
            ArgumentNullException.ThrowIfNull(resultServiceType, nameof(resultServiceType));
            ArgumentNullException.ThrowIfNull(implementationType, nameof(implementationType));

            return _assemblyInfo.ModuleBuilder.DefineType(
                $"SimpleFactoryOf{resultServiceType.Name}From{parameterType.Name}",
                TypeAttributes.Public |
                TypeAttributes.Class |
                TypeAttributes.AutoClass |
                TypeAttributes.AnsiClass |
                TypeAttributes.BeforeFieldInit |
                TypeAttributes.AutoLayout,
                _memberAccessor.ParentType,
                [implementationType]);
        }

        public FieldBuilder CreateConstructor(TypeBuilder typeBuilder, Type serviceType)
        {
            // https://stackoverflow.com/questions/46750814/assign-fields-in-constructor-via-reflection-emit#46760697
            // https://www.codeproject.com/Articles/121568/Dynamic-Type-Using-Reflection-Emit

            ArgumentNullException.ThrowIfNull(typeBuilder, nameof(typeBuilder));
            ArgumentNullException.ThrowIfNull(serviceType, nameof(serviceType));

            ConstructorBuilder constructor = typeBuilder.DefineConstructor(
                MethodAttributes.Public |
                MethodAttributes.HideBySig |
                MethodAttributes.SpecialName |
                MethodAttributes.RTSpecialName,
                CallingConventions.Standard,
                [_memberAccessor.ServiceProviderType]);
            constructor.SetImplementationFlags(MethodImplAttributes.Managed);

            FieldBuilder serviceProviderField = typeBuilder.DefineField(
                "_serviceProvider",
                _memberAccessor.ServiceProviderType,
                FieldAttributes.Private | FieldAttributes.InitOnly);

            ILGenerator il = constructor.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Call, _memberAccessor.ConstructorBase);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Stfld, serviceProviderField);
            il.Emit(OpCodes.Ret);

            return serviceProviderField;
        }

        public MethodBuilder CreateNewMethod(TypeBuilder typeBuilder, Type genericFactoryType)
        {
            ArgumentNullException.ThrowIfNull(typeBuilder, nameof(typeBuilder));
            ArgumentNullException.ThrowIfNull(genericFactoryType, nameof(genericFactoryType));

            MethodInfo newOfGenericFactory = genericFactoryType.GetMethod(nameof(IFactory<object, object>.New), BindingFlags.Public | BindingFlags.Instance)
                ?? throw new MissingMethodException(genericFactoryType.Name, nameof(IFactory<object, object>.New));

            MethodBuilder method = typeBuilder.DefineMethod(
                newOfGenericFactory.Name,
                MethodAttributes.Public |
                MethodAttributes.HideBySig |
                MethodAttributes.NewSlot |
                MethodAttributes.Virtual |
                MethodAttributes.Final,
                newOfGenericFactory.ReturnType,
                newOfGenericFactory.GetParameters().Select(p => p.ParameterType).ToArray());

            method.SetImplementationFlags(MethodImplAttributes.Managed);

            return method;
        }
    }

    internal class DynamicILGenerator
    {
        // https://vibhuaggarwal.wordpress.com/about/dynamic-type-using-reflection-emit
        // https://www.codeproject.com/Articles/13337/Introduction-to-Creating-Dynamic-Types-with-Reflec
        // https://www.codeproject.com/Articles/13969/Introduction-to-Creating-Dynamic-Types-with-Refl-2
        // https://www.filipekberg.se/2011/10/18/creating-a-dynamic-method-that-uses-a-switch
        // https://stackoverflow.com/questions/18467235/creating-dynamic-type-from-typebuilder-with-a-base-class-and-additional-fields-g#18468107
        // https://www.codeproject.com/Articles/742788/Dynamic-Interface-Implementation
        // https://github.com/ekonbenefits/impromptu-interface/blob/master/ImpromptuInterface/src/EmitProxy/ActLikeMaker.cs#L1772
        // https://dotnetfiddle.net

        private readonly MemberAccessor _memberAccessor;
        private readonly FieldBuilder _serviceProviderField;
        private readonly Type _returnType;
        private StringComparison? _stringComparison;

        public DynamicILGenerator(
            MemberAccessor memberAccessor,
            FieldBuilder serviceProviderField,
            Type returnType)
        {
            _memberAccessor = memberAccessor ?? throw new ArgumentNullException(nameof(memberAccessor));
            _serviceProviderField = serviceProviderField ?? throw new ArgumentNullException(nameof(serviceProviderField));
            _returnType = returnType ?? throw new ArgumentNullException(nameof(returnType));
        }

        public void Use(StringComparison comparison) =>
            _stringComparison = comparison;

        public void GenerateStringEqualsImplementation(MethodBuilder methodBuilder, KeyValuePair<string, Type>[] mappings, Type? typeOnDefaultCase)
        {
            ArgumentNullException.ThrowIfNull(methodBuilder, nameof(methodBuilder));
            ArgumentNullException.ThrowIfNull(mappings, nameof(mappings));

            ILGenerator il = methodBuilder.GetILGenerator();

            var defaultCase = il.DefineLabel();

            if (mappings.Length == 0)
            {
                GenerateNullReturn(il);
            }
            else if (mappings.Length == 1)
            {
                GenerateSingleCondition(il, mappings[0], defaultCase);
            }
            else
            {
                GenerateManyConditions(mappings, il, defaultCase);
            }

            GenerateDefaultCase(il, defaultCase, typeOnDefaultCase);
        }

        public void GenerateSwitchImplementation(MethodBuilder methodBuilder, KeyValuePair<NumericEmittingILGenerator, Type>[] mappings, Type? typeOnDefaultCase)
        {
            // https://stackoverflow.com/questions/70768899/il-switch-instruction

            ArgumentNullException.ThrowIfNull(methodBuilder, nameof(methodBuilder));
            ArgumentNullException.ThrowIfNull(mappings, nameof(mappings));

            ILGenerator il = methodBuilder.GetILGenerator();

            var defaultCase = il.DefineLabel();

            if (mappings.Length == 0)
            {
                GenerateNullReturn(il);
            }
            else if (mappings.Length == 1)
            {
                GenerateSingleCondition(il, mappings[0], defaultCase);
            }
            else if (mappings.Length == 2)
            {
                GeneratePairCondition(il, mappings[0], mappings[1], defaultCase);
            }
            else
            {
                GenerateSwitchOrCondition(il, mappings, defaultCase);
            }

            GenerateDefaultCase(il, defaultCase, typeOnDefaultCase);
        }

        private void GenerateSwitchOrCondition(ILGenerator il, KeyValuePair<NumericEmittingILGenerator, Type>[] mappings, Label defaultCase)
        {
            List<(int start, int end)> groups = [];
            int start = 0;

            for (var i = 1; i < mappings.Length; i++)
            {
                if (mappings[i - 1].Key.LongValue + 1 < mappings[i].Key.LongValue)
                {
                    groups.Add((start, i - 1));
                    start = i;
                }
            }

            groups.Add((start, start < mappings.Length - 1 ? mappings.Length - 1 : start));
            List<(Label, Type)> labels = new(mappings.Length);

            for (var i = 0; i < groups.Count; i++)
            {
                var len = groups[i].end - groups[i].start + 1;
                var itemCount = labels.Count + 1;

                if (len == 1)
                {
                    var label = DefineConditionalLabel(il, mappings[groups[i].start].Key, itemCount);
                    labels.Add((label, mappings[groups[i].start].Value));
                }
                else if (len == 2)
                {
                    var label1 = DefineConditionalLabel(il, mappings[groups[i].start].Key, itemCount);
                    var label2 = DefineConditionalLabel(il, mappings[groups[i].end].Key, itemCount + 1);
                    labels.Add((label1, mappings[groups[i].start].Value));
                    labels.Add((label2, mappings[groups[i].end].Value));
                }
                else
                {
                    var items = new KeyValuePair<NumericEmittingILGenerator, Type>[len];
                    Array.Copy(mappings, groups[i].start, items, 0, len);
                    var lbs = DefineSwitchLabels(il, items.Select(i => i.Key).ToArray());
                    for (var l = 0; l < lbs.Length; l++)
                    {
                        labels.Add((lbs[l], items[l].Value));
                    }
                }
            }

            il.Emit(labels.Count >= 5 ? OpCodes.Br : OpCodes.Br_S, defaultCase);

            for (var i = 0; i < labels.Count; i++)
            {
                GenerateCallByLabel(il, labels[i].Item1, labels[i].Item2);
            }
        }

        private void GenerateSingleCondition(ILGenerator il, KeyValuePair<string, Type> first, Label defaultCase)
        {
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Ldstr, first.Key);
            EmitCallStringEquals(il);
            il.Emit(OpCodes.Brfalse_S, defaultCase);

            GenerateCallGetService(il, first.Value);
        }

        private void GenerateManyConditions(KeyValuePair<string, Type>[] mappings, ILGenerator il, Label defaultCase)
        {
            var labels = new Label[mappings.Length];
            for (var i = 0; i < mappings.Length; i++)
            {
                labels[i] = DefineConditionalLabel(il, mappings[i].Key, i + 1);
            }

            il.Emit(labels.Length >= 5 ? OpCodes.Br : OpCodes.Br_S, defaultCase);

            for (var i = 0; i < mappings.Length; i++)
            {
                GenerateCallByLabel(il, labels[i], mappings[i].Value);
            }
        }

        private void GenerateSingleCondition(ILGenerator il, KeyValuePair<NumericEmittingILGenerator, Type> first, Label defaultCase)
        {
            il.Emit(OpCodes.Ldarg_1);

            if (first.Key.LongValue == 0)
            {
                il.Emit(OpCodes.Brtrue_S, defaultCase);
            }
            else
            {
                first.Key.EmitLdc(il);
                il.Emit(OpCodes.Bne_Un_S, defaultCase);
            }

            GenerateCallGetService(il, first.Value);
        }

        private void GeneratePairCondition(ILGenerator il, KeyValuePair<NumericEmittingILGenerator, Type> first, KeyValuePair<NumericEmittingILGenerator, Type> second, Label defaultCase)
        {
            var firstCase = DefineConditionalLabel(il, first.Key, 1);
            var secondCase = DefineConditionalLabel(il, second.Key, 2);

            il.Emit(OpCodes.Br_S, defaultCase);

            il.MarkLabel(firstCase);
            GenerateCallGetService(il, first.Value);

            il.MarkLabel(secondCase);
            GenerateCallGetService(il, second.Value);
        }

        private Label DefineConditionalLabel(ILGenerator il, string input, int itemCount)
        {
            var label = il.DefineLabel();

            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Ldstr, input);
            EmitCallStringEquals(il);
            il.Emit(itemCount >= 5 ? OpCodes.Brtrue : OpCodes.Brtrue_S, label);

            return label;
        }

        private Label DefineConditionalLabel(ILGenerator il, NumericEmittingILGenerator input, int itemCount)
        {
            var labelCase = il.DefineLabel();

            il.Emit(OpCodes.Ldarg_1);
            if (input.LongValue == 0)
            {
                il.Emit(OpCodes.Brfalse_S, labelCase);
            }
            else
            {
                input.EmitLdc(il);
                il.Emit(itemCount >= 5 ? OpCodes.Beq : OpCodes.Beq_S, labelCase);
            }

            return labelCase;
        }

        private Label[] DefineSwitchLabels(ILGenerator il, NumericEmittingILGenerator[] inputs)
        {
            // var labels = inputs.Select(_ => il.DefineLabel()).ToArray();
            var labels = new Label[inputs.Length];
            for (int i = 0; i < labels.Length; i++)
            {
                labels[i] = il.DefineLabel();
            }

            il.Emit(OpCodes.Ldarg_1);

            if (inputs[0].LongValue != 0)
            {
                inputs[0].EmitLdc(il);
                il.Emit(OpCodes.Sub);
            }

            il.Emit(OpCodes.Switch, labels);

            return labels;
        }

        private void GenerateCallByLabel(ILGenerator il, Label labelCase, Type serviceType)
        {
            il.MarkLabel(labelCase);
            GenerateCallGetService(il, serviceType);
        }

        private void GenerateDefaultCase(ILGenerator il, Label defaultCase, Type? type)
        {
            il.MarkLabel(defaultCase);

            if (type is null)
            {
                GenerateNullReturn(il);
            }
            else
            {
                GenerateCallGetService(il, type);
            }
        }

        private void EmitCallStringEquals(ILGenerator il)
        {
            if (_stringComparison is null)
            {
                il.Emit(OpCodes.Call, _memberAccessor.EqualsOfString);
            }
            else
            {
                il.Emit(OpCodes.Ldc_I4, (int)_stringComparison.Value);
                il.Emit(OpCodes.Call, _memberAccessor.EqualsComparisonOfString);
            }
        }

        private void GenerateCallGetService(ILGenerator il, Type serviceType)
        {
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldfld, _serviceProviderField);
            il.Emit(OpCodes.Ldtoken, serviceType);
            il.Emit(OpCodes.Call, _memberAccessor.GetTypeFromHandleOfType);
            il.EmitCall(OpCodes.Callvirt, _memberAccessor.GetServiceOfServiceProvider, null);
            il.Emit(OpCodes.Castclass, _returnType);
            il.Emit(OpCodes.Ret);
        }

        private void GenerateNullReturn(ILGenerator il)
        {
            il.Emit(OpCodes.Ldnull);
            il.Emit(OpCodes.Ret);
        }
    }

    internal record MemberAccessor
    {
        public readonly Type NativeType = typeof(Type);
        public readonly Type Int32Type = typeof(int);
        public readonly Type Int64Type = typeof(long);
        public readonly Type StringType = typeof(string);
        public readonly Type ParentType = typeof(object);
        public readonly Type ServiceProviderType = typeof(IServiceProvider); //typeof(ServiceProviderProxy);
        public readonly MethodInfo GetServiceOfServiceProvider;
        public readonly MethodInfo GetTypeFromHandleOfType;
        public readonly MethodInfo EqualsOfString;
        public readonly MethodInfo EqualsComparisonOfString;
        public readonly ConstructorInfo ConstructorBase = typeof(object).GetConstructor([])
            ?? throw new MissingMemberException(typeof(object).Name, "ctor");

        public MemberAccessor()
        {
            GetServiceOfServiceProvider = ServiceProviderType.GetMethod(nameof(IServiceProvider.GetService), BindingFlags.Public | BindingFlags.Instance, [NativeType])
                ?? throw new MissingMemberException(ServiceProviderType.Name, nameof(IServiceProvider.GetService));
            GetTypeFromHandleOfType = NativeType.GetMethod(nameof(NativeType.GetTypeFromHandle), BindingFlags.Public | BindingFlags.Static, [typeof(RuntimeTypeHandle)])
                ?? throw new MissingMemberException(NativeType.Name, nameof(NativeType.GetTypeFromHandle));
            EqualsOfString = StringType.GetMethod(nameof(string.Equals), BindingFlags.Public | BindingFlags.Static, [StringType, StringType])
                ?? throw new MissingMemberException(StringType.Name, nameof(string.Equals));
            EqualsComparisonOfString = StringType.GetMethod(nameof(string.Equals), BindingFlags.Public | BindingFlags.Static, [StringType, StringType, typeof(StringComparison)])
                ?? throw new MissingMemberException(StringType.Name, nameof(string.Equals));
        }
    }
}