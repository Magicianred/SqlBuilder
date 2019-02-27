using System;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SqlBuilder.UnitTest.Extensions
{
  [TestClass]
  public class ExpressionExtensionsTests
  {
    [TestMethod]
    public void MemberName_returns_member_name()
    {
      Expression<Func<TestClass, string>> expression = x => x.Foo;

      expression.MemberName().MustBe(nameof(TestClass.Foo));
    }

    [TestMethod]
    public void MemberName_throws_exception_when_expression_does_not_contain_member()
    {
      Expression<Func<TestClass, int>> expression = null;

      Action action = () => expression.MemberName().MustBe(nameof(TestClass.FooBar));

      action.MustThrow();
    }

    private abstract class TestClass
    {
      public string Foo { get; set; }

      public abstract int FooBar();
    }
  }
}