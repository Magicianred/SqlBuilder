using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace SqlBuilder.UnitTest
{
  [TestClass]
  public class ParameterCollectionTests
  {
    [TestMethod]
    public void add_adds_parameter()
    {
      ParameterCollection parameterCollection = new ParameterCollection();

      parameterCollection.Add("foo", 2);

      parameterCollection.Count.MustBe(1);
      parameterCollection["foo"].MustBe(2);
    }

    [TestMethod]
    public void add_returns_next_parameter_name_when_not_given()
    {
      ParameterCollection parameterCollection = new ParameterCollection();

      parameterCollection.Add(1).MustBe("@p0");
      parameterCollection.Add(2).MustBe("@p1");

      parameterCollection.Count.MustBe(2);
      parameterCollection["@p0"].MustBe(1);
      parameterCollection["@p1"].MustBe(2);
    }

    [TestMethod]
    public void names_returns_colleciton_of_name_strings()
    {
      ParameterCollection parameterCollection = new ParameterCollection();

      parameterCollection.Add(1);
      parameterCollection.Add(2);

      parameterCollection.Names.MustContain("@p0", "@p1");
    }

    [TestMethod]
    public void addrange_adds_multiple_parameters()
    {
      ParameterCollection parameterCollection = new ParameterCollection();

      parameterCollection.Add(1);
      parameterCollection.AddRange(new Dictionary<string, object>
      {
        { "foo", 2 },
        { "bar", 3 },
      });

      parameterCollection.Count.MustBe(3);
      parameterCollection["@foo"].MustBe(2);
      parameterCollection["@bar"].MustBe(3);
    }

    [TestMethod]
    public void NextKey_returns_next_available_parameter_key()
    {
      ParameterCollection parameterCollection = new ParameterCollection();

      parameterCollection.NextKey().MustBe("@p0");
      parameterCollection.Add(0);
      parameterCollection.NextKey().MustBe("@p1");
    }

    [TestMethod]
    public void GetName_prepends_prefix()
    {
      ParameterCollection.GetName("foo").MustBe("@foo");
      ParameterCollection.GetName("@foo").MustBe("@foo");
    }

    [TestMethod]
    public void ctor_creates_parameters_if_null()
    {
      new ParameterCollection(null).Count.MustBe(0);
    }
  }
}