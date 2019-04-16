using System.Collections.Generic;
using System.Linq;

namespace SqlBuilder
{
  public class OrderBy : SqlText
  {
    private OrderBy()
      : base()
    {
      _collection = new List<string>(0);
    }

    public OrderBy(string sql)
      :this()
    {
      Add(sql);
    }

    public void Add(string sql)
    {
      _collection.Add(sql);
    }

    public void Add(int index, string sql)
    {
      _collection.Insert(index, sql);
    }

    public override string Sql()
    {
      switch (_collection.Count)
      {
        case 0:
          {
            return null;
          }
        case 1:
          {
            return $"order by {_collection.First()}";
          }
        default:
          {
            return $"order by {string.Join(ListSeparator, _collection)}";
          }

      }
    }

    private readonly List<string> _collection;
  }
}