using System;

namespace SqlBuilder
{
  public class Clause : SqlText
  {
    public Clause(string sql, ParameterCollection parameters)
      : this(parameters)
    {
      _sql = sql;
    }

    public Clause(ParameterCollection parameters)
      : base(parameters) { }

    public Clause(ParameterCollection parameters, string column, SqlOperator sqlOperator, object value)
      : this(parameters)
    {
      Column = column;
      SqlOperator = sqlOperator;
      ParameterName = Parameters.Add(value);
    }

    public Clause(ParameterCollection parameters, string column, object value)
      : this(parameters, column, SqlOperator.Equal, value) { }

    public override string Sql()
    {
      if (!string.IsNullOrEmpty(_sql))
      {
        return _sql;
      }

      string sql;
      string sqlOperator = GetSqlOperator(SqlOperator);

      if (SqlOperator == SqlOperator.Like)
      {
        sql = string.Concat(Column, Space, sqlOperator, Space, ParameterName);
      }
      else
      {
        sql = string.Concat(Column, sqlOperator, ParameterName);
      }

      if (string.IsNullOrEmpty(Operator))
      {
        return sql;
      }

      return string.Concat(Operator, Space, sql);
    }

    public const string SqlAnd = "And";

    public const string SqlOr = "Or";

    public string Operator;

    protected readonly string ParameterName;

    private readonly string Column;

    private readonly SqlOperator SqlOperator;

    private static string GetSqlOperator(SqlOperator sqlOperator)
    {
      switch (sqlOperator)
      {
        case SqlOperator.Like:
          {
            return "Like";
          }
        case SqlOperator.Equal:
          {
            return "=";
          }
        case SqlOperator.NotEqual:
          {
            return "!=";
          }
        case SqlOperator.GreaterThan:
          {
            return ">";
          }
        case SqlOperator.GreaterThanOrEqual:
          {
            return ">=";
          }
        case SqlOperator.LessThan:
          {
            return "<";
          }
        case SqlOperator.LessThanOrEqual:
          {
            return "<=";
          }
        default:
          {
            throw new NotImplementedException($"{nameof(SqlOperator)} {sqlOperator} not recognised in {nameof(SqlText)}");
          }
      }
    }

    private readonly string _sql;
  }
}