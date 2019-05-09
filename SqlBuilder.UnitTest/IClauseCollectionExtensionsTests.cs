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

    [TestMethod]
    public void And_calls_Add_with_arguments()
    {
      IClauseCollection clauseCollection = A.Fake<IClauseCollection>();

      IClauseCollectionExtensions.And(clauseCollection, "1=1");

      // assert the 2nd argument of Add() was called with the clause argument where the sql matches and with the correct operator
      A.CallTo(() => clauseCollection.Add("And", A<Clause>.Ignored))
        .WhenArgumentsMatch(x =>
        {
          return
            x.Get<string>(0) == "And" &&
            x.Get<Clause>(1).Sql() == "1=1";
        }).MustHaveHappened();
    }

    [TestMethod]
    public void Or_calls_Add_with_arguments()
    {
      IClauseCollection clauseCollection = A.Fake<IClauseCollection>();

      IClauseCollectionExtensions.Or(clauseCollection, "1=1");

      // assert the 2nd argument of Add() was called with the clause argument where the sql matches and with the correct operator
      A.CallTo(() => clauseCollection.Add("And", A<Clause>.Ignored))
        .WhenArgumentsMatch(x =>
        {
          return
            x.Get<string>(0) == "Or" &&
            x.Get<Clause>(1).Sql() == "1=1";
        }).MustHaveHappened();
    }

    public class TestClass
    {
      public string Title { get; set; }
    }
  }
}