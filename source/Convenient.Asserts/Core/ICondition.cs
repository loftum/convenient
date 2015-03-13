namespace Convenient.Asserts.Core
{
    public interface ICondition<in T>
    {
        void Verify(T item);
    }
}