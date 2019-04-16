using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SqlBuilder.UnitTest
{
  [TestClass]
  public class OrderByTests
  {
    [TestMethod]
    public void supports_clauses()
    {
      OrderBy orderBy = new OrderBy("foo asc");

      orderBy.Sql().MustBe("order by foo asc");
    }

    [TestMethod]
    public void Add_adds_orderby()
    {
      OrderBy orderBy = new OrderBy("foo asc");

      orderBy.Add("bar desc");

      orderBy.Sql().MustBe("order by foo asc,bar desc");
    }

    [TestMethod]
    public void Add_with_index_adds_orderby()
    {
      OrderBy orderBy = new OrderBy("foo asc");

      orderBy.Add("bar desc");
      orderBy.Add(0, "fooBar desc");

      orderBy.Sql().MustBe("order by fooBar desc,foo asc,bar desc");
    }
  }
}