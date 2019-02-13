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
      StringExtensions.ToCamelCase("FooBar").MustEqual("fooBar");
      StringExtensions.ToCamelCase("fooBar").MustEqual("fooBar");
      StringExtensions.ToCamelCase("foobar").MustEqual("foobar");
    }

    [TestMethod]
    public void ToCamelCase_ignores_null_or_empty()
    {
      StringExtensions.ToCamelCase(null).MustBeNull();
      StringExtensions.ToCamelCase("").MustEqual("");
    }
  }
}