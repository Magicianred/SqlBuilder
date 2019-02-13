using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlBuilder.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

    [TestMethod]
    public void picks_up_key()
    {
      ModelDefinition modelDefinition = new ModelDefinition(typeof(Test));

      modelDefinition.Key.MustEqual("Id");
    }

    [TestMethod]
    public void picks_up_table_name_with_schema()
    {
      ModelDefinition modelDefinition = new ModelDefinition(typeof(Test));

      modelDefinition.TableName.MustEqual("test.foo");
    }

    [TestMethod]
    public void picks_up_options()
    {
      ModelDefinition modelDefinition = new ModelDefinition(typeof(Test));

      modelDefinition.Options.MustEqual(SqlOptions.OptimiseForUnknown);
    }

    [TestMethod]
    public void fd()
    {
      ModelDefinition modelDefinition = new ModelDefinition(typeof(Test));

      modelDefinition.Options.MustEqual(SqlOptions.OptimiseForUnknown);
    }

    [Table("foo", Schema = "test")]
    [SqlOptions(Options = SqlOptions.OptimiseForUnknown)]
    private class Test
    {
      [Key]
      public int Id { get; set; }

      [OrderBy]
      public string Foo { get; set; }
    }
  }
}