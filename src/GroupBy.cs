namespace SqlBuilder
{
  public class GroupBy : SqlText
  {
    public GroupBy(string sql)
      : base()
    {
      _sql = sql;
    }

    public override string Sql()
    {
      if (string.IsNullOrEmpty(_sql))
      {
        return _sql;
      }

      return $"group by {_sql}";
    }

    public readonly string _sql;
  }
}