using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SqlBuilder.UnitTest
{
  [TestClass]
  public class SqlTextTests
  {
    [TestMethod]
    public void call_ctor_creates_empty_parameters_if_argument_null()
    {
      TestClass testClass = new TestClass();

      new TestClass().Parameters.MustNotBeNull();
      new TestClass().Parameters.Count.MustBe(0);
    }

    public void sql_appends_to_builder()
    {
      TestClass testClass = new TestClass();
      StringBuilder stringBuilder = new StringBuilder();
      testClass.Sql(stringBuilder);

      stringBuilder.ToString().MustBe("foo bar");
    }

    private class TestClass : SqlText
    {
      public override string Sql()
      {
        return "foo bar";
      }
    }
  }
}
