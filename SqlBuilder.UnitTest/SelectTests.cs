using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlBuilder.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace SqlBuilder.UnitTest
{
  [TestClass]
  public class SelectTests
  {
    [TestMethod]
    public void include_count_adds_count_recordset()
    {
      Select<DataModel> select = new Select<DataModel>(includeCount: true);
      select.Sql().MustBe("select Id,Title from dbo.DataModel order by Title;select count(*) from dbo.DataModel");
    }

    [TestMethod]
    public void include_count_adds_count_recordset_with_where_clause()
    {
      Select<DataModel> select = new Select<DataModel>(includeCount: true);
      select.Where("Title='test'");
      select.Sql().MustBe("select Id,Title from dbo.DataModel where Title='test' order by Title;select count(*) from dbo.DataModel where Title='test'");
    }

    [TestMethod]
    public void paging_applies_sql_with_where_and_orderby_clause()
    {
      Select<DataModel> select = new Select<DataModel>();
      select.Paging(1, 10);
      select.Where("Title='test'");
      select.OrderBy("Title");
      // note it uses top because we want the first page
      select.Sql().MustBe("select top 10 Id,Title from dbo.DataModel where Title='test' order by Title");
    }

    [TestMethod]
    public void add_column_applies_sql()
    {
      Select<DataModel> select = new Select<DataModel>();
      select.Column(x => x.Id);
      select.Sql().MustBe("select Id from dbo.DataModel order by Title");
    }

    [TestMethod]
    public void orderby_adds_column()
    {
      Select<DataModel> select = new Select<DataModel>();
      select.OrderBy(x => x.Id);
      select.Sql().MustBe("select Id,Title from dbo.DataModel order by Id");
    }

    [TestMethod]
    public void orderby_adds_descending_column()
    {
      Select<DataModel> select = new Select<DataModel>();
      select.OrderBy(x => x.Id, false);
      select.Sql().MustBe("select Id,Title from dbo.DataModel order by Id desc");
    }

    [TestMethod]
    public void groupby_adds_column()
    {
      Select<DataModel> select = new Select<DataModel>();
      select.GroupBy(x => x.Id);
      select.Sql().MustBe("select Id,Title from dbo.DataModel group by Id order by Title");
    }

    [TestMethod]
    public void groupby_adds_column_and_allows_null_orderby()
    {
      Select<DataModel> select = new Select<DataModel>("Max(Id)");
      select.GroupBy(x => x.Id, null);
      select.Sql().MustBe("select Max(Id) from dbo.DataModel group by Id");
    }

    [TestMethod]
    public void where_adds_clause()
    {
      Select<DataModel> select = new Select<DataModel>();
      select.Where(x => x.Title, "fooBar");
      select.Sql().MustBe("select Id,Title from dbo.DataModel where Title=@p0 order by Title");
      select.Parameters.Count.MustBe(1);
      select.Parameters["@p0"].MustBe("fooBar");
    }

    [TestMethod]
    public void where_adds_clause_with_operator()
    {
      Select<DataModel> select = new Select<DataModel>();
      select.Where(x => x.Title, SqlOperator.LessThan, "fooBar");
      select.Sql().MustBe("select Id,Title from dbo.DataModel where Title<@p0 order by Title");
      select.Parameters.Count.MustBe(1);
      select.Parameters["@p0"].MustBe("fooBar");
    }

    [TestMethod]
    public void column_adds_and_returns_column()
    {
      Select<DataModel> select = new Select<DataModel>();
      select.Column(x => x.Title);
      select.Sql().MustBe("select Title from dbo.DataModel order by Title");
    }

    [TestMethod]
    public void column_supports_multiple_calls()
    {
      Select<DataModel> select = new Select<DataModel>();
      // called in different order
      select.Column(x => x.Title);
      select.Column(x => x.Id);
      select.Sql().MustBe("select Title,Id from dbo.DataModel order by Title");
    }

    [TestMethod]
    public void supports_SetArithabort()
    {
      Select<DataModel> select = new Select<DataModel>();
      select.Options(SqlOptions.SetArithabort);
      select.Sql().MustBe("set arithabort on\r\nselect Id,Title from dbo.DataModel order by Title");
    }

    [TestMethod]
    public void supports_recompile()
    {
      Select<DataModel> select = new Select<DataModel>();
      select.Options(SqlOptions.Recompile);
      select.Sql().MustBe("select Id,Title from dbo.DataModel order by Title option (recompile)");
    }

    [TestMethod]
    public void supports_optimize_unknown()
    {
      Select<DataModel> select = new Select<DataModel>();
      select.Options(SqlOptions.OptimiseForUnknown);
      select.Sql().MustBe("select Id,Title from dbo.DataModel order by Title option (optimize for unknown)");
    }

    [TestMethod]
    public void supports_multiple_options()
    {
      Select<DataModel> select = new Select<DataModel>();
      select.Options(SqlOptions.Recompile | SqlOptions.OptimiseForUnknown);
      select.Sql().MustBe("select Id,Title from dbo.DataModel order by Title option (recompile,optimize for unknown)");
    }

    [TestMethod]
    public void supports_options_on_count()
    {
      Select<DataModel> select = new Select<DataModel>();
      select.IncludeCount(true);
      select.Options(SqlOptions.Recompile);
      select.Sql().MustBe("select Id,Title from dbo.DataModel order by Title option (recompile);select count(*) from dbo.DataModel option (recompile)");
    }

    /// <summary>
    /// Fixes issue in paging where we were checking _orderby and not inherited order by which would have returned (if set) the orderby attribute value.
    /// </summary>
    [TestMethod]
    public void ensure_paging_checks_inherited_order_by_before_asserting_it_can_page()
    {
      Select<DataModel> select = new Select<DataModel>();
      select.Paging(1, 1); // this surfaced the bug

      Action action = () => select.Paging();

      action.MustNotThrow();
    }

    [TestMethod]
    public void selecting_items_from_the_first_page_uses_top()
    {
      Select select = new Select();
      select.Paging(1, 1);
      select.OrderBy("foo");
      select.From("bar");

      select.Sql().MustBe("select top 1 * from bar order by foo");
    }

    /// <summary>
    /// This fixes a data bug where randomising with an order by will break the random data
    /// </summary>
    [TestMethod]
    public void randomise_adds_order_by_and_ignores_all_other_order_by()
    {
      Select select = new Select();
      select.OrderBy("foo");
      select.From("bar");
      select.Randomise();

      select.Sql().MustBe("select * from bar order by NEWID(),foo");
    }

    /// <summary>
    /// This is basically ensuring that the where with clauses works correctly.  Where has extensive clause testing already.
    /// </summary>
    [TestMethod]
    public void strongly_typed_select_with_where_sets_parameters_and_returns_sql()
    {
      Select<DataModel> select = new Select<DataModel>();

      select
        .Where(x => x.Id, 1)
        .And(x => x.Title, "foo");

      select.Sql().MustBe("select Id,Title from dbo.DataModel where (Id=@p0 And Title=@p1) order by Title");
      select.Parameters["@p0"].MustBe(1);
      select.Parameters["@p1"].MustBe("foo");
    }

    /// <summary>
    /// Bug found where calling select.Where().And("col", "foo") would throw because Where returns null.
    /// </summary>
    [TestMethod]
    public void where_returning_null_when_chaining()
    {
      Select<DataModel> select = new Select<DataModel>();

      select
        .Where(x => x.Id, 1)
        .And(x => x.Title, "foo");

      select.Sql().MustBe("select Id,Title from dbo.DataModel where (Id=@p0 And Title=@p1) order by Title");
      select.Parameters["@p0"].MustBe(1);
      select.Parameters["@p1"].MustBe("foo");
    }

    [TestMethod]
    public void supports_join()
    {
      Select select = new Select("foo");
      select
        .From("dbo.bar b")
        .Join("left join dbo.fooBar fb on fb.Id = b.id");

      select.Sql().MustBe("select foo from dbo.bar b left join dbo.fooBar fb on fb.Id = b.id");
    }

    [TestMethod]
    public void supports_multiple_joins()
    {
      Select select = new Select("foo");
      select
        .From("dbo.bar b")
        .Join("left join dbo.fooBar fb on fb.Id = b.id")
        .Join("left join dbo.foo f on f.Id = fb.id");

      select.Sql().MustBe("select foo from dbo.bar b left join dbo.fooBar fb on fb.Id = b.id left join dbo.foo f on f.Id = fb.id");
    }

    [TestMethod]
    public void complex_select_with_joins_and_adhoc_where_clauses()
    {
      Select<VProperty> select = new Select<VProperty>();
      select
        .Column(x => x.Title)
        .Column(x => x.Latitude)
        .Column(x => x.Longitude)
        .LeftJoin("dbo.TProperty P", "P.PropertyId = @propertyId")
        .LeftJoin("dbo.TAddress PA", "PA.AddressId = P.AddressId")
        .OrderBy("RP.Cost")
        .Where("RP.PropertyId != P.PropertyId")
        .And("RP.PostCodeFirstPart = PA.PostCodeFirstPart")
        .And("RP.BedroomCount = P.BedroomCount")
        .And("RP.Cost between ROUND(P.Cost - ((P.Cost/100) * 5), -5) AND ROUND(P.Cost + ((P.Cost/100) * 5), +5)")
        .And("RP.[Status] != 4");

      select.Sql().MustBe("select Title,Latitude,Longitude from dbo.VProperty left join dbo.TProperty P on P.PropertyId = @propertyId left join dbo.TAddress PA on PA.AddressId = P.AddressId where (RP.PropertyId != P.PropertyId And RP.PostCodeFirstPart = PA.PostCodeFirstPart And RP.BedroomCount = P.BedroomCount And RP.Cost between ROUND(P.Cost - ((P.Cost/100) * 5), -5) AND ROUND(P.Cost + ((P.Cost/100) * 5), +5) And RP.[Status] != 4) order by RP.Cost");
    }

    [TestMethod]
    public void simplified_select_with_adhoc_where_clauses()
    {
      Select<VProperty> select = new Select<VProperty>();
      select
        .Column(x => x.Title)
        .Where("PropertyId != PropertyId")
        .And("PostCodeFirstPart = PostCodeFirstPart");

      select.Sql().MustBe("select Title from dbo.VProperty where (PropertyId != PropertyId And PostCodeFirstPart = PostCodeFirstPart)");
    }

    [TestMethod]
    public void simplified_select_with_parameter()
    {
      Select<VProperty> select = new Select<VProperty>();
      select
        .Column(x => x.Title)
        .Where("PropertyId != PropertyId")
        .And("PostCodeFirstPart = PostCodeFirstPart")
        .And("PropertyId", 1);

      string sql = select.Sql();
    }

    [TestMethod]
    public void creating_instance_with_modeldefinition_and_emptyorderby_does_not_add_empty_orderby()
    {
      new Select<VProperty>(new ModelDefinition(typeof(VProperty))).OrderBy().MustBeNull();
    }

    [TestMethod]
    public void uses_alias()
    {
      new Select<VProperty>(new ModelDefinition(typeof(VProperty)), alias: "RP").Sql().MustBe("select RP.Key,RP.Title,RP.Latitude,RP.Longitude from dbo.VProperty RP");

      // fixes bug where I wasn't passing down the arg to the overloaded ctor (!)
      new Select<VProperty>(alias: "RP").Sql().MustBe("select RP.Key,RP.Title,RP.Latitude,RP.Longitude from dbo.VProperty RP");
    }

    [TestMethod]
    public void set_column_uses_alias()
    {
      Select<VProperty> select = new Select<VProperty>(alias: "RP");

      select.Column(x => x.Title);

      select.Sql().MustBe("select RP.Title from dbo.VProperty RP");
    }

    /// <summary>
    /// Make sure we don't include properties without public setters and readonly fields.
    /// </summary>
    [TestMethod]
    public void ignores_readonly_members()
    {
      Select<VProperty> select = new Select<VProperty>();

      select.Sql().MustBe("select Key,Title,Latitude,Longitude from dbo.VProperty");
    }

    private class VProperty
    {
      [Key]
      public int Key { get; set; }

      public string Title { get; set; }

      public double? Latitude { get; set; }

      public double? Longitude { get; set; }

      public bool IsReadOnlyProp { get; private set; }

      public readonly bool IsReadOnlyField = true;
    }

    private class DataModel
    {
      public int Id { get; set; }

      [OrderBy]
      public string Title { get; set; }
    }

    private class NestedDataModel
    {
      public int Id { get; set; }

      public string Title { get; set; }
    }
  }
}