using System.Collections.Generic;
using System.Text;

namespace SqlBuilder
{
  public class JoinCollection : SqlText
  {
    public JoinCollection()
    {
      _joins = new List<Join>();
    }

    public void Add(string sql)
    {
      _joins.Add(new Join(sql));
    }

    public override void Sql(StringBuilder builder)
    {
      if (_joins.Count > 0)
      {
        foreach (Join join in _joins)
        {
          join.Sql(builder);
        }
      }
    }

    public override string Sql()
    {
      StringBuilder stringBuilder = new StringBuilder();
      Sql(stringBuilder);
      return stringBuilder.ToString();
    }

    private readonly List<Join> _joins;
  }
}