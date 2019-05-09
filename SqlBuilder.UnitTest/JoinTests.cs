using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SqlBuilder.UnitTest
{
  [TestClass]
  public class JoinTests
  {
    [TestMethod]
    public void supports_sql_string()
    {
      const string sql = "left join table a on a.id = b.id";

      new Join(sql).Sql().MustBe(sql);
    }
  }
}
