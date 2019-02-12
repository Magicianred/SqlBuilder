using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlBuilder.Attributes;

namespace SqlBuilder.UnitTest
{
  [TestClass]
  public class ModelDefinitionTests
  {
    [TestMethod]
    public void picks_up_order_by()
    {
      ModelDefinition modelDefinition = new ModelDefinition(typeof(Test));

      modelDefinition.OrderBy.MustEqual("Foo");
    }

    private class Test
    {
      [OrderBy]
      public string Foo { get; set; }
    }
  }
}
