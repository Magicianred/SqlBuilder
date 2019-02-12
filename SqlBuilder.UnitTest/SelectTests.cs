﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlBuilder.Attributes;
using System;

namespace SqlBuilder.UnitTest
{
  [TestClass]
  public class SelectTests
  {
    [TestMethod]
    public void include_count_adds_count_recordset()
    {
      Select<DataModel> select = new Select<DataModel>(includeCount: true);
      select.Sql().MustEqual("select Id,Title from dbo.DataModel order by Title;select count(*) from dbo.DataModel");
    }

    [TestMethod]
    public void include_count_adds_count_recordset_with_where_clause()
    {
      Select<DataModel> select = new Select<DataModel>(includeCount: true);
      select.Where("Title='test'");
      select.Sql().MustEqual("select Id,Title from dbo.DataModel where Title='test' order by Title;select count(*) from dbo.DataModel where Title='test'");
    }

    [TestMethod]
    public void paging_applies_sql_with_where_and_orderby_clause()
    {
      Select<DataModel> select = new Select<DataModel>();
      select.Paging(1, 10);
      select.Where("Title='test'");
      select.OrderBy("Title");
      // note it uses top because we want the first page
      select.Sql().MustEqual("select top 10 Id,Title from dbo.DataModel where Title='test' order by Title");
    }

    [TestMethod]
    public void add_column_applies_sql()
    {
      Select<DataModel> select = new Select<DataModel>();
      select.Column(x => x.Id);
      select.Sql().MustEqual("select Id from dbo.DataModel order by Title");
    }

    [TestMethod]
    public void orderby_adds_column()
    {
      Select<DataModel> select = new Select<DataModel>();
      select.OrderBy(x => x.Id);
      select.Sql().MustEqual("select Id,Title from dbo.DataModel order by Id");
    }

    [TestMethod]
    public void orderby_adds_descending_column()
    {
      Select<DataModel> select = new Select<DataModel>();
      select.OrderBy(x => x.Id, false);
      select.Sql().MustEqual("select Id,Title from dbo.DataModel order by Id desc");
    }

    [TestMethod]
    public void groupby_adds_column()
    {
      Select<DataModel> select = new Select<DataModel>();
      select.GroupBy(x => x.Id);
      select.Sql().MustEqual("select Id,Title from dbo.DataModel order by Title group by Id");
    }

    [TestMethod]
    public void where_adds_clause()
    {
      Select<DataModel> select = new Select<DataModel>();
      select.Where(x => x.Title, "fooBar");
      select.Sql().MustEqual("select Id,Title from dbo.DataModel where Title=@p0 order by Title");
      select.Parameters.Count.MustEqual(1);
      select.Parameters["@p0"].MustEqual("fooBar");
    }

    [TestMethod]
    public void column_adds_and_returns_column()
    {
      Select<DataModel> select = new Select<DataModel>();
      select.Column(x => x.Title);
      select.Sql().MustEqual("select Title from dbo.DataModel order by Title");
    }

    [TestMethod]
    public void column_supports_multiple_calls()
    {
      Select<DataModel> select = new Select<DataModel>();
      // called in different order
      select.Column(x => x.Title);
      select.Column(x => x.Id);
      select.Sql().MustEqual("select Title,Id from dbo.DataModel order by Title");
    }

    [TestMethod]
    public void supports_SetArithabort()
    {
      Select<DataModel> select = new Select<DataModel>();
      select.Options(SqlOptions.SetArithabort);
      select.Sql().MustEqual("set arithabort on\r\nselect Id,Title from dbo.DataModel order by Title");
    }

    [TestMethod]
    public void supports_recompile()
    {
      Select<DataModel> select = new Select<DataModel>();
      select.Options(SqlOptions.Recompile);
      select.Sql().MustEqual("select Id,Title from dbo.DataModel order by Title option (recompile)");
    }

    [TestMethod]
    public void supports_optimize_unknown()
    {
      Select<DataModel> select = new Select<DataModel>();
      select.Options(SqlOptions.OptimiseForUnknown);
      select.Sql().MustEqual("select Id,Title from dbo.DataModel order by Title option (optimize for unknown)");
    }

    [TestMethod]
    public void supports_multiple_options()
    {
      Select<DataModel> select = new Select<DataModel>();
      select.Options(SqlOptions.Recompile | SqlOptions.OptimiseForUnknown);
      select.Sql().MustEqual("select Id,Title from dbo.DataModel order by Title option (recompile,optimize for unknown)");
    }

    [TestMethod]
    public void supports_options_on_count()
    {
      Select<DataModel> select = new Select<DataModel>();
      select.IncludeCount(true);
      select.Options(SqlOptions.Recompile);
      select.Sql().MustEqual("select Id,Title from dbo.DataModel order by Title option (recompile);select count(*) from dbo.DataModel option (recompile)");
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

      select.Sql().MustEqual("select top 1 * from bar order by foo");
    }

    [TestMethod]
    public void randomise_adds_order_by()
    {
      Select select = new Select();
      select.OrderBy("foo");
      select.From("bar");
      select.Randomise();

      select.Sql().MustEqual("select * from bar order by foo,NEWID()");
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

      select.Sql().MustEqual("select Id,Title from dbo.DataModel where (Id=@p0 And Title=@p1) order by Title");
      select.Parameters["@p0"].MustEqual(1);
      select.Parameters["@p1"].MustEqual("foo");
    }

    private class DataModel
    {
      public int Id { get; set; }

      [OrderBy]
      public string Title { get; set; }
    }
  }
}