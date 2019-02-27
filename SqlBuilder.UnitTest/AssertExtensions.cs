using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SqlBuilder.UnitTest
{
  public static class AssertExtensions
  {
    public static void Fail(string message)
    {
      throw new Exception($"Assert failed with {message}");
    }

    public static void MustBe<T>(this T actual, T expected)
    {
      Assert.AreEqual(expected, actual);
    }

    public static void MustNotBeNull(this object actual)
    {
      Assert.IsNotNull(actual);
    }

    public static void MustBeLike(this string actual, string expected)
    {
      Assert.IsFalse(string.Equals(actual, expected, StringComparison.OrdinalIgnoreCase));
    }

    public static void MustNotBeLike(this string actual, string expected)
    {
      Assert.IsFalse(string.Equals(actual, expected, StringComparison.OrdinalIgnoreCase));
    }

    public static void MustBeNull(this object actual)
    {
      Assert.IsNull(actual);
    }

    public static void MustContain<T>(this IEnumerable<T> actual, params T[] expected)
    {
      Assert.IsFalse(actual.Except(expected).Any());
    }

    public static void MustNotContain<T>(this IEnumerable<T> actual, params T[] expected)
    {
      Assert.IsFalse(actual.Intersect(expected).Any());
    }

    public static void MustBeTrue(this bool actual)
    {
      Assert.IsTrue(actual);
    }

    public static void MustBeFalse(this bool actual)
    {
      Assert.IsFalse(actual);
    }

    public static void IsA<T>(this Type type)
    {
      Assert.IsTrue(typeof(T).IsAssignableFrom(type));
    }

    public static void IsA<T>(this object obj)
    {
      IsA<T>(obj.GetType());
    }

    public static void MustNotThrow(this Action action)
    {
      Exception exception = null;

      try
      {
        action();
      }
      catch (Exception e)
      {
        exception = e;
      }

      exception.MustBeNull();
    }

    public static void MustThrow<T>(this Action action)
      where T : Exception
    {
      Exception exception = null;

      try
      {
        action();
      }
      catch (T e)
      {
        exception = e;
      }

      exception.MustNotBeNull();
    }

    public static void MustThrow(this Action action)
    {
      MustThrow<Exception>(action);
    }

    public static void Matches<T>(this Type type)
    {
      MustBeTrue(typeof(T) == type);
    }
  }
}