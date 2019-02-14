using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SqlBuilder.UnitTest
{
  [TestClass]
  public class KeyTests
  {
    [TestMethod]
    public void KeyComparer_equals_identifys_matches()
    {
      KeyComparer keyComparer = new KeyComparer();

      Key key1 = new Key(typeof(ViewModel), typeof(ViewModel));
      keyComparer.Equals(key1, key1).MustBeTrue();

      Key key2 = new Key(typeof(ViewModel));
      keyComparer.Equals(key2, key2).MustBeTrue();

      keyComparer.Equals(key1, key2).MustBeFalse();
    }

    private class ViewModel { }

    private class DataModel { }
  }
}
