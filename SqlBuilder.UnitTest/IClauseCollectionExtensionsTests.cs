using System;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SqlBuilder.UnitTest
{
  [TestClass]
  public class IClauseCollectionExtensionsTests
  {
    [TestMethod]
    public void And_calls_And_with_arguments()
    {
      IClauseCollection clauseCollection = A.Fake<IClauseCollection>();
      IClauseCollectionExtensions.And(clauseCollection, "foo", 2);

      A.CallTo(() => clauseCollection.And("foo", SqlOperator.Equal, 2)).MustHaveHappened();
    }

    private class TestClass { }
  }
}