namespace Massive.Unity
{
    public interface IFactory<out T>
    {
        T Create();
    }
}
