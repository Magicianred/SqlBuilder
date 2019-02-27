using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel.DataAnnotations;

namespace SqlBuilder.UnitTest
{
  [TestClass]
  public class DeleteTests
  {
    [TestMethod]
    public void delete_creates_sql()
    {
      Delete<TestModel> delete = new Delete<TestModel>(4);

      delete.Sql().MustBe("delete from dbo.TestModel where Id=@Id");
      delete.Parameters.Count.MustBe(1);
      delete.Parameters["Id"].MustBe(4);
    }

    private class TestModel
    {
      [Key]
      public int Id { get; set; }
    }
  }
}