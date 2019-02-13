using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SqlBuilder.UnitTest.Extensions
{
  [TestClass]
  public class TypeExtensionsTests
  {
    [TestMethod]
    public void IsNullable_returns_true_when_type_is_nullable()
    {
      typeof(int?).IsNullable(out Type underlyingType).MustBeTrue();
      underlyingType.MustNotBeNull();
    }

    [TestMethod]
    public void GetPublicMembers_returns_public_members()
    {
      IEnumerable<MemberInfo> publicMembers = typeof(TestClass).GetPublicMembers();

      publicMembers.Count().MustEqual(2);
      publicMembers.Select(x => x.Name).MustContain(nameof(TestClass.PublicFooField), nameof(TestClass.PublicFooProperty));
    }

    [TestMethod]
    public void Inherits_returns_true_when_type_inherits_type()
    {
      typeof(TestClass).Inherits(typeof(BaseTestClass)).MustBeTrue();
      typeof(BaseTestClass).Inherits(typeof(TestClass)).MustBeFalse();
    }

    [TestMethod]
    public void ReturnType_returns_return_type()
    {
      typeof(TestClass).GetField(nameof(TestClass.PublicFooField)).ReturnType().Matches<string>();
    }

    private class TestClass : BaseTestClass
    {
      public string PublicFooField;

      public string PublicFooProperty { get; set; }

      protected string ProtectedFooField;

      protected string ProtectedFooProperty { get; set; }

      private string PrivateFooField;

      private string PrivateFooProperty { get; set; }
    }

    private class BaseTestClass
    {
    }
  }
}