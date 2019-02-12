# SqlBuilder
[![Image of Yaktocat](https://ci.appveyor.com/api/projects/status/rpfycctymokj7t6v/branch/master?svg=true
)](https://ci.appveyor.com/project/restlessmedia/sqlbuilder)

Colleciton of classes to aid building SQL queries.
```
private class DataModel
{
      public int Id { get; set; }
      
      [OrderBy]
      public string Title { get; set; }
}

Select<DataModel> select = new Select<DataModel>();
      select.Where(x => x.Title, "fooBar");
      select.Sql().MustEqual("select Id,Title from dbo.DataModel where Title=@p0 order by Title");
      select.Parameters.Count.MustEqual(1);
      select.Parameters["@p0"].MustEqual("fooBar");
```
