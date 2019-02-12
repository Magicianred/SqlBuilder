using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel.DataAnnotations;

namespace SqlBuilder.UnitTest
{
  [TestClass]
  public class InsertTests
  {
    [TestMethod]
    public void insert_creates_sql()
    {
      EditableTestModel testModel = new EditableTestModel
      {
        Id = 2,
        Title = "test",
      };
      Insert<EditableTestModel> insert = new Insert<EditableTestModel>(testModel);

      insert.Sql().MustEqual("insert into dbo.EditableTestModel(Id,Title) values(@Id,@Title);");
      insert.Parameters.Count.MustEqual(2);
      insert.Parameters["Id"].MustEqual(2);
      insert.Parameters["Title"].MustEqual("test");
    }

    [TestMethod]
    public void insert_ignores_key()
    {
      TestModel testModel = new TestModel
      {
        Id = 2,
        Title = "test",
      };
      Insert<TestModel> insert = new Insert<TestModel>(testModel);

      insert.Sql().MustEqual("insert into dbo.TestModel(Title) values(@Title);");
      insert.Parameters.Count.MustEqual(1);
      insert.Parameters["Title"].MustEqual("test");
    }

    [TestMethod]
    public void insert_outputs_key()
    {
      TestModel testModel = new TestModel
      {
        Id = 2,
        Title = "test",
      };
      Insert<TestModel> insert = new Insert<TestModel>(testModel, true);

      insert.Sql().MustEqual("declare @output table(_key int);insert into dbo.TestModel(Title) output inserted.Id into @output values(@Title);select _key from @output;");
      insert.Parameters.Count.MustEqual(1);
      insert.Parameters["Title"].MustEqual("test");
    }

    private class TestModel
    {
      [Key]
      public int Id { get; set; }

      public string Title { get; set; }
    }

    private class EditableTestModel
    {
      public int Id { get; set; }

      public string Title { get; set; }
    }
  }
}