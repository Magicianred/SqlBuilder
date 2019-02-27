using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SqlBuilder.UnitTest
{
  [TestClass]
  public class GroupByTests
  {
    [TestMethod]
    public void returns_sql()
    {
      new GroupBy("foo").Sql().MustBe("group by foo");
    }
  }
}