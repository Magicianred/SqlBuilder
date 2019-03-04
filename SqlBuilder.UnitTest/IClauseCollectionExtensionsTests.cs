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

    [TestMethod]
    public void AndIsNotNull_calls_And_with_arguments()
    {
      IClauseCollection<TestClass> clauseCollection = A.Fake<IClauseCollection<TestClass>>();

      IClauseCollectionExtensions.AndIsNotNull(clauseCollection, x => x.Title);

      A.CallTo(() => clauseCollection.And("Title", SqlOperator.IsNot, "null")).MustHaveHappened();
    }

    [TestMethod]
    public void AndIsNull_calls_And_with_arguments()
    {
      IClauseCollection<TestClass> clauseCollection = A.Fake<IClauseCollection<TestClass>>();

      IClauseCollectionExtensions.AndIsNull(clauseCollection, x => x.Title);

      A.CallTo(() => clauseCollection.And("Title", SqlOperator.Is, "null")).MustHaveHappened();
    }

    [TestMethod]
    public void OrIsNotNull_calls_And_with_arguments()
    {
      IClauseCollection<TestClass> clauseCollection = A.Fake<IClauseCollection<TestClass>>();

      IClauseCollectionExtensions.OrIsNotNull(clauseCollection, x => x.Title);

      A.CallTo(() => clauseCollection.Or("Title", SqlOperator.IsNot, "null")).MustHaveHappened();
    }

    [TestMethod]
    public void OrIsNull_calls_And_with_arguments()
    {
      IClauseCollection<TestClass> clauseCollection = A.Fake<IClauseCollection<TestClass>>();

      IClauseCollectionExtensions.OrIsNull(clauseCollection, x => x.Title);

      A.CallTo(() => clauseCollection.Or("Title", SqlOperator.Is, "null")).MustHaveHappened();
    }

    public class TestClass
    {
      public string Title { get; set; }
    }
  }
}