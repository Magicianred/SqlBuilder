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

      select.Parameters.Count().MustBe(1);
      select.Parameters["@p0"].MustBe(2);
      select.Where().Sql().MustBe("where foo=@p0");
    }

    [TestMethod]
    public void Where_adds_clause()
    {
      Select select = new Select();
      SelectExtensions.Where(select, "foo", SqlOperator.LessThan, 2);

      select.Parameters.Count().MustBe(1);
      select.Parameters["@p0"].MustBe(2);
      select.Where().Sql().MustBe("where foo<@p0");
    }

    [TestMethod]
    public void Where_adds_clause_when_clauses_already_exist()
    {
      Select<TestClass> select = new Select<TestClass>();
      select.Where(x => x.Id, 7);
      SelectExtensions.Where(select, "foo", SqlOperator.LessThan, 2);

      select.Parameters.Count().MustBe(2);
      select.Parameters["@p0"].MustBe(7);
      select.Parameters["@p1"].MustBe(2);
      select.Where().Sql().MustBe("where (Id=@p0 And foo<@p1)");
    }

    [TestMethod]
    public void LeftJoin_adds_join()
    {
      Select<TestClass> select = new Select<TestClass>();
      SelectExtensions.LeftJoin(select, "foo", "1=1");
      select.Join().Sql().MustBe("left join foo on 1=1");
    }

    [TestMethod]
    public void RightJoin_adds_join()
    {
      Select<TestClass> select = new Select<TestClass>();
      SelectExtensions.RightJoin(select, "foo", "1=1");
      select.Join().Sql().MustBe("right join foo on 1=1");
    }

    [TestMethod]
    public void Join_adds_join()
    {
      Select<TestClass> select = new Select<TestClass>();
      SelectExtensions.Join(select, JoinType.Left, "foo", "1=1");
      select.Join().Sql().MustBe("left join foo on 1=1");
    }

    private class TestClass
    {
      public int Id { get; set; }
    }
  }
}