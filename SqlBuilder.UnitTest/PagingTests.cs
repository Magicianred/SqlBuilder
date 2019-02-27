using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SqlBuilder.UnitTest
{
  [TestClass]
  public class PagingTests
  {
    [TestMethod]
    public void RowStart_returns_value()
    {
      new Paging(1, 10).RowStart.MustBe(1);
      new Paging(2, 10).RowStart.MustBe(11);
    }

    [TestMethod]
    public void RowEnd_returns_value()
    {
      new Paging(1, 10).RowEnd.MustBe(10);
      new Paging(2, 10).RowEnd.MustBe(20);
    }

    [TestMethod]
    public void Skip_returns_value()
    {
      new Paging(1, 10).Skip.MustBe(0);
      new Paging(2, 10).Skip.MustBe(10);
    }

    [TestMethod]
    public void Top_returns_value()
    {
      new Paging(1, 10).Top.MustBe(10);
    }

    [TestMethod]
    public void Sql_returns_offset()
    {
      new Paging(1, 10).Sql().MustBe("offset 0 rows fetch next 10 rows only");
    }
  }
}