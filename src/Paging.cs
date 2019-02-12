using System.Text;

namespace SqlBuilder
{
  public class Paging : SqlText
  {
    public Paging(int page, int pageSize = DefaultPageSize)
    {
      Page = page;
      PageSize = pageSize;
    }

    public int Top
    {
      get
      {
        return PageSize;
      }
    }

    public int Skip
    {
      get
      {
        if (Page > 1)
        {
          return (Page - 1) * PageSize;
        }

        return 0;
      }
    }

    public int RowStart
    {
      get
      {
        return Skip + 1;
      }
    }

    public int RowEnd
    {
      get
      {
        if (Page > 1)
        {
          return Page * PageSize;
        }

        return PageSize;
      }
    }

    public override string Sql()
    {
      return $"offset {Skip} rows fetch next {Top} rows only";
    }

    public override void Sql(StringBuilder builder)
    {
      base.Sql(builder);
    }

    public bool IsTop
    {
      get
      {
        return Page == 1;
      }
    }

    public int Page;

    public int PageSize;

    public const int DefaultPageSize = 10;
  }
}