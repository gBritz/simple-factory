namespace SimpleFactory.Core.Abstractions
{
    public interface IFactory<out TService>
    {
        TService New();
    }
}