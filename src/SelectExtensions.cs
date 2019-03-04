using System;
using System.Linq.Expressions;

namespace SqlBuilder
{
  public static class SelectExtensions
  {
    public static Select Where(this Select select, string column, SqlOperator sqlOperator, object value)
    {
      Clause clause = new Clause(select.Parameters, column, sqlOperator, value);
      select.Where(clause);
      return select;
    }

    public static Select Where(this Select select, string column, object value)
    {
      return Where(select, column, SqlOperator.Equal, value);
    }
  }
}