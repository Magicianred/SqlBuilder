using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SqlBuilder.UnitTest
{
  [TestClass]
  public class JoinCollectionTests
  {
    [TestMethod]
    public void Adding_single_sql_statement_returns_sql()
    {
      const string sql = "left join table a on a.id = b.id";
      JoinCollection joinCollection = new JoinCollection();

      joinCollection.Add(sql);

      joinCollection.Sql().MustBe(sql);
    }

    [TestMethod]
    public void Supports_multiple_sql_statements()
    {
      const string joina = "left join table a on a.id = b.id";
      const string joinb = "left join table b on b.id = a.id";
      JoinCollection joinCollection = new JoinCollection();

      joinCollection.Add(joina);
      joinCollection.Add(joinb);

      joinCollection.Sql().MustBe("left join table a on a.id = b.id left join table b on b.id = a.id");
    }
  }
}
