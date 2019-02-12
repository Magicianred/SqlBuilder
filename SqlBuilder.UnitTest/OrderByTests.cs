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

      orderBy.Sql().MustEqual("order by foo asc");
    }

    [TestMethod]
    public void supports_multiple_clauses()
    {
     // TODO:
    }
  }
}