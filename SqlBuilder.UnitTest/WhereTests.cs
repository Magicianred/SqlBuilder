using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SqlBuilder.UnitTest
{
  [TestClass]
  public class WhereTests
  {
    [TestMethod]
    public void includes_parenthesis_for_nested_clauses()
    {
      ParameterCollection parameterCollection = new ParameterCollection();
      Where where = new Where(parameterCollection);

      where
        .And("foo", "2")
        .And("bar", "3")
        .Or()
          .And("fooBar", "5")
          .And("barFoo", SqlOperator.GreaterThan, "7");

      where.Sql().MustBe("where (foo=@p0 And bar=@p1 Or (fooBar=@p2 And barFoo>@p3))");
    }

    [TestMethod]
    public void does_not_include_parenthesis_for_single_clause()
    {
      ParameterCollection parameterCollection = new ParameterCollection();
      Where where = new Where(parameterCollection);

      where
        .And("foo", "2")
        .Or("bar", "3")
        .And()
          .Or("fooBar", "5");

      where.Sql().MustBe("where (foo=@p0 Or bar=@p1 And fooBar=@p2)");
    }
  }
}
