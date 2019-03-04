using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace SqlBuilder.UnitTest
{
  [TestClass]
  public class ClauseCollectionTests
  {
    [TestMethod]
    public void Add_uses_And_operator_when_more_than_one_clause()
    {
      ParameterCollection parameters = new ParameterCollection();
      ClauseCollection clauseCollection = new ClauseCollection(parameters);

      clauseCollection.Add(new Clause(parameters, "foo", "bar"));
      clauseCollection.Add(new Clause(parameters, "bar", "foo"));

      parameters.Count().MustBe(2);
      clauseCollection.Sql().MustBe("(foo=@p0 And bar=@p1)");
    }

    [TestMethod]
    public void Add_adds_clause()
    {
      ParameterCollection parameters = new ParameterCollection();
      ClauseCollection clauseCollection = new ClauseCollection(parameters);

      clauseCollection.Add("foo", SqlOperator.LessThan, 1);

      parameters.Count().MustBe(1);
      clauseCollection.Sql().MustBe("foo<@p0");
    }

    [TestMethod]
    public void Add_adds_clause_when_using_IsNot()
    {
      ParameterCollection parameters = new ParameterCollection();
      ClauseCollection clauseCollection = new ClauseCollection(parameters);

      clauseCollection.Add("foo", SqlOperator.IsNot, null);

      clauseCollection.Sql().MustBe("foo Is Not null");
    }

    [TestMethod]
    public void Add_adds_clause_when_using_Is()
    {
      ParameterCollection parameters = new ParameterCollection();
      ClauseCollection clauseCollection = new ClauseCollection(parameters);

      clauseCollection.Add("foo", SqlOperator.Is, null);

      clauseCollection.Sql().MustBe("foo Is null");
    }

    [TestMethod]
    public void Add_throws_when_when_using_IsNot_with_non_null_value()
    {
      ParameterCollection parameters = new ParameterCollection();
      ClauseCollection clauseCollection = new ClauseCollection(parameters);

      clauseCollection.Add("foo", SqlOperator.IsNot, 3);

      Action action = () => clauseCollection.Sql();

      action.MustThrow();
    }

    [TestMethod]
    public void Add_throws_when_when_using_Is_with_non_null_value()
    {
      ParameterCollection parameters = new ParameterCollection();
      ClauseCollection clauseCollection = new ClauseCollection(parameters);

      clauseCollection.Add("foo", SqlOperator.Is, 1);

      Action action = () => clauseCollection.Sql();

      action.MustThrow();
    }
  }
}
