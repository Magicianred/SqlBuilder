using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace SqlBuilder.UnitTest
{
  [TestClass]
  public class SelectExtensionsTests
  {
    [TestMethod]
    public void Where_adds_equals_clause()
    {
      Select select = new Select();
      SelectExtensions.Where(select, "foo", 2);

      select.Parameters.Count().MustEqual(1);
      select.Parameters["@p0"].MustEqual(2);
      select.Where().Sql().MustEqual("where foo=@p0");
    }

    [TestMethod]
    public void Where_adds_clause()
    {
      Select select = new Select();
      SelectExtensions.Where(select, "foo", SqlOperator.LessThan, 2);

      select.Parameters.Count().MustEqual(1);
      select.Parameters["@p0"].MustEqual(2);
      select.Where().Sql().MustEqual("where foo<@p0");
    }

    [TestMethod]
    public void Where_adds_clause_when_clauses_already_exist()
    {
      Select<TestClass> select = new Select<TestClass>();
      select.Where(x => x.Id, 7);
      SelectExtensions.Where(select, "foo", SqlOperator.LessThan, 2);

      select.Parameters.Count().MustEqual(2);
      select.Parameters["@p0"].MustEqual(7);
      select.Parameters["@p1"].MustEqual(2);
      select.Where().Sql().MustEqual("where (Id=@p0 And foo<@p1)");
    }

    private class TestClass
    {
      public int Id { get; set; }
    }
  }
}