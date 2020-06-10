using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SqlBuilder
{
  public class MultiSelect : SqlText
  {
    public override string Sql()
    {
      return string.Join(Environment.NewLine, _selects.Select(select => select.Sql()));
    }

    public override void Sql(StringBuilder builder)
    {
      foreach (Select select in _selects)
      {
        select.Sql(builder);
      }
    }

    public void Add(Select select)
    {
      _selects.Add(select);
    }

    private readonly IList<Select> _selects = new List<Select>(0);
  }
}