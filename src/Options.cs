namespace SqlBuilder
{
  public class Options : SqlText
  {
    public Options(SqlOptions sqlOptions)
    {
      _sqlOptions = sqlOptions;
    }

    public override string Sql()
    {
      if (_sqlOptions != SqlOptions.None)
      {
        string sql = null;

        if (_sqlOptions.HasFlag(SqlOptions.Recompile))
        {
          sql = "recompile";
        }

        if (_sqlOptions.HasFlag(SqlOptions.OptimiseForUnknown))
        {
          if (!string.IsNullOrEmpty(sql))
          {
            sql = string.Concat(sql, ListSeparator);
          }

          sql = string.Concat(sql, "optimize for unknown");
        }

        if (!string.IsNullOrEmpty(sql))
        {
          return string.Concat("option (", sql, ")");
        }
      }

      return null;
    }

    private readonly SqlOptions _sqlOptions;
  }
}