using System.Collections.Generic;

namespace Conventient.UnitTests.TestData
{
    public class TestObject : IHaveString
    {
        public int Number { get; set; }
        public string StringField;
        public TestEnum TestEnum { get; set; }
        public string String { get; set; }
        public IList<InnerObject> InnerObjects { get; set; }
        public InnerObject Inner { get; set; }

        public TestObject()
        {
            Inner = new InnerObject();
            InnerObjects = new List<InnerObject>();
        }
    }
}