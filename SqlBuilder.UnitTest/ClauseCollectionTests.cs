using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
  }
}
