namespace SimpleFactory.Core.Abstractions
{
    public interface IFactory<in T, out TService>
    {
        TService New(T param);

        //TService NewRequired(T param);
    }
}