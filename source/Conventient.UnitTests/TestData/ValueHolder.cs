namespace Conventient.UnitTests.TestData
{
    public class ValueHolder<T>
    {
        public T Value { get; set; }

        public ValueHolder()
        {
        }

        public ValueHolder(T value)
        {
            Value = value;
        }
    }
}