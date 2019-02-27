using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SqlBuilder.UnitTest
{
  [TestClass]
  public class ClauseTests
  {
    /// <summary>
    /// For simple clauses that just take a sql string.
    /// </summary>
    [TestMethod]
    public void Sql_returns_sql_string_when_passed()
    {
      ParameterCollection parameterCollection = new ParameterCollection();
      string sql = "foo=1";
      Clause clause = new Clause(sql, parameterCollection);

      clause.Sql().MustBe(sql);
    }

    [TestMethod]
    public void Sql_returns_generated_sql_string()
    {
      new Clause(new ParameterCollection(), "foo", SqlOperator.GreaterThan, 1).Sql().MustBe("foo>@p0");
      new Clause(new ParameterCollection(), "foo", SqlOperator.Equal, 1).Sql().MustBe("foo=@p0");
      new Clause(new ParameterCollection(), "foo", SqlOperator.GreaterThanOrEqual, 1).Sql().MustBe("foo>=@p0");
      new Clause(new ParameterCollection(), "foo", SqlOperator.LessThan, 1).Sql().MustBe("foo<@p0");
      new Clause(new ParameterCollection(), "foo", SqlOperator.LessThanOrEqual, 1).Sql().MustBe("foo<=@p0");
      new Clause(new ParameterCollection(), "foo", SqlOperator.Like, "1").Sql().MustBe("foo Like @p0");
    }

    [TestMethod]
    public void Sql_returns_generated_sql_string_with_operator()
    {
      new Clause(new ParameterCollection(), "foo", SqlOperator.GreaterThan, 1)
      {
        Operator = "And"
      }.Sql().MustBe("And foo>@p0");
    }
  }
}
