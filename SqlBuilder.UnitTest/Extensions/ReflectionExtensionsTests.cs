using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SqlBuilder.UnitTest.Extensions
{
  [TestClass]
  public class ReflectionExtensionsTests
  {
    [TestMethod]
    public void AttributeHas_returns_true_when_attribute_matches_and_passes_predicate()
    {
      typeof(TestClass).GetMember(nameof(TestClass.Foo), BindingFlags.Instance | BindingFlags.Public).First().AttributeHas<RequiredAttribute>(x => Equals(x.ErrorMessage, "FooBar")).MustBeTrue();
    }

    [TestMethod]
    public void AttributeHas_returns_false_when_attribute_matches_and_does_not_pass_predicate()
    {
      typeof(TestClass).GetMember(nameof(TestClass.Foo), BindingFlags.Instance | BindingFlags.Public).First().AttributeHas<RequiredAttribute>(x => Equals(x.ErrorMessage, null)).MustBeFalse();
    }

    [TestMethod]
    public void AttributeHas_returns_false_when_attribute_does_not_match_but_predicate_would()
    {
      typeof(TestClass).GetMember(nameof(TestClass.Foo), BindingFlags.Instance | BindingFlags.Public).First().AttributeHas<CreditCardAttribute>(x => Equals(x.ErrorMessage, "FooBar")).MustBeFalse();
    }

    [TestMethod]
    public void GetMemberValue_returns_property_value()
    {
      TestClass testClass = new TestClass
      {
        Foo = 9,
      };

      testClass.GetType().GetMember(nameof(TestClass.Foo), BindingFlags.Instance | BindingFlags.Public).First().GetMemberValue(testClass).MustBe(testClass.Foo);
    }

    [TestMethod]
    public void GetMemberValue_returns_field_value()
    {
      TestClass testClass = new TestClass
      {
        BarField = "1234566777",
      };

      testClass.GetType().GetMember(nameof(TestClass.BarField), BindingFlags.Instance | BindingFlags.Public).First().GetMemberValue(testClass).MustBe(testClass.BarField);
    }

    [TestMethod]
    public void SetMemberValue_sets_property_value()
    {
      TestClass testClass = new TestClass
      {
        Foo = 9,
      };

      testClass.GetType().GetMember(nameof(TestClass.Foo), BindingFlags.Instance | BindingFlags.Public).First().SetMemberValue(testClass, 999);

      testClass.Foo.MustBe(999);
    }

    [TestMethod]
    public void SetMemberValue_sets_field_value()
    {
      TestClass testClass = new TestClass
      {
        BarField = "1234566777",
      };

      testClass.GetType().GetMember(nameof(TestClass.BarField), BindingFlags.Instance | BindingFlags.Public).First().SetMemberValue(testClass, "fooBar");

      testClass.BarField.MustBe("fooBar");
    }

    [TestMethod]
    public void SetMemberValue_ignores_set_when_obj_is_null()
    {
      Action action = () => typeof(TestClass).GetType().GetField(nameof(TestClass.BarField), BindingFlags.Instance | BindingFlags.Public).SetMemberValue(null, "fooBar");

      action.MustNotThrow();
    }

    [TestMethod]
    public void GetMemberValue_returns_null_when_obj_is_null()
    {
      Action action = () => typeof(TestClass).GetType().GetField(nameof(TestClass.BarField), BindingFlags.Instance | BindingFlags.Public).GetMemberValue(null);

      action.MustNotThrow();
    }

    private class TestClass
    {
      [Required(ErrorMessage = "FooBar")]
      public int Foo { get; set; }

      public string BarField;
    }
  }
}
