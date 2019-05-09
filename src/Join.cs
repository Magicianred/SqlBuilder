using System.Text;

namespace SqlBuilder
{
  public class Join : SqlText
  {
    public Join(string sql)
    {
      _sql = sql;
    }

    public override string Sql()
    {
      return _sql;
    }

    private readonly string _sql;
  }
}