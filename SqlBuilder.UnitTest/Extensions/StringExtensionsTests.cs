using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SqlBuilder.UnitTest.Extensions
{
  [TestClass]
  public class StringExtensionsTests
  {
    [TestMethod]
    public void ToCamelCase_converts()
    {
      StringExtensions.ToCamelCase("FooBar").MustBe("fooBar");
      StringExtensions.ToCamelCase("fooBar").MustBe("fooBar");
      StringExtensions.ToCamelCase("foobar").MustBe("foobar");
    }

    [TestMethod]
    public void ToCamelCase_ignores_null_or_empty()
    {
      StringExtensions.ToCamelCase(null).MustBeNull();
      StringExtensions.ToCamelCase("").MustBe("");
    }
  }
}