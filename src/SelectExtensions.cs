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

    public static Select LeftJoin(this Select select, string joinOn, string predicate)
    {
      return Join(select, JoinType.Left, joinOn, predicate);
    }

    public static Select RightJoin(this Select select, string joinOn, string predicate)
    {
      return Join(select, JoinType.Right, joinOn, predicate);
    }

    public static Select Join(this Select select, JoinType joinType, string joinTo, string predicate)
    {
      return select.Join($"{joinType.ToString().ToLowerInvariant()} join {joinTo} on {predicate}");
    }
  }
}