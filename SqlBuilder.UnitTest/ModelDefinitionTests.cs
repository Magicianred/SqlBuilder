using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlBuilder.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

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
    public void GetDatabaseColumns_does_not_include_non_database_columns()
    {
      ModelDefinition modelDefinition = new ModelDefinition(typeof(Test));

      IEnumerable<Column> columns = modelDefinition.GetDatabaseColumns();

      columns.Count().MustEqual(2);
    }

    [TestMethod]
    public void GetEditableColumns_returns_edtiable_columns()
    {
      ModelDefinition modelDefinition = new ModelDefinition(typeof(Test));

      IEnumerable<Column> columns = modelDefinition.GetEditableColumns();

      columns.Count().MustEqual(1);
      columns.First().Name.MustEqual(nameof(Test.Foo));
    }

    [TestMethod]
    public void GetEditableColumns_does_not_return_key()
    {
      ModelDefinition modelDefinition = new ModelDefinition(typeof(Test));

      IEnumerable<Column> columns = modelDefinition.GetEditableColumns();

      columns.Any(x => string.Equals(x.Name, nameof(Test.Id))).MustBeFalse();
    }

    [Table("foo", Schema = "test")]
    [SqlOptions(Options = SqlOptions.OptimiseForUnknown)]
    private class Test
    {
      [Key]
      public int Id { get; set; }

      [OrderBy]
      public string Foo { get; set; }

      [DatabaseGenerated(DatabaseGeneratedOption.None)]
      public string NotForDb { get; set; }
    }
  }
}