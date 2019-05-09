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
      string sql;

      if (!string.IsNullOrEmpty(_sql))
      {
        sql = _sql;
      }
      else
      {
        string sqlOperator = GetSqlOperator(SqlOperator);

        // special clauses
        if (SqlOperator == SqlOperator.Is || SqlOperator == SqlOperator.IsNot)
        {
          // we only support 'is' and 'is not' with null
          if (Parameters[ParameterName] != null)
          {
            throw new InvalidOperationException($"Attempting to use '{SqlOperator}' operator with non-null value.  This operator is only support with null.");
          }

          sql = string.Concat(Column, Space, sqlOperator, Space, "null");
        }
        else if (SqlOperator == SqlOperator.Like)
        {
          // special like formatting
          sql = string.Concat(Column, Space, sqlOperator, Space, ParameterName);
        }
        else
        {
          // all other operators
          sql = string.Concat(Column, sqlOperator, ParameterName);
        }
      }

      // if no operator simply return the sql
      if (string.IsNullOrEmpty(Operator))
      {
        return sql;
      }

      // return sql with operator
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
        case SqlOperator.Is:
        case SqlOperator.Between:
          {
            return sqlOperator.ToString();
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
        case SqlOperator.IsNot:
          {
            return "Is Not";
          }
        default:
          {
            throw new NotImplementedException($"{nameof(SqlOperator)} {sqlOperator} not recognised in {nameof(SqlText)}.");
          }
      }
    }

    private readonly string _sql;
  }
}