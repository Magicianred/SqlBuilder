using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SqlBuilder
{
  public class Where<T> : ClauseCollection<T>, IWhere
  {
    public Where(string sql, ParameterCollection parameters)
      : this(parameters)
    {
      Add(new Clause(sql, parameters));
    }

    public Where(ParameterCollection parameters)
      : base(parameters) { }

    public override string Sql()
    {
      string sql = base.Sql();

      if (string.IsNullOrEmpty(sql))
      {
        return null;
      }

      return string.Concat("where ", sql);
    }
  }

  public class Where : ClauseCollection, IWhere
  {
    public Where(string sql, ParameterCollection parameters)
      : this(parameters)
    {
      Add(new Clause(sql, parameters));
    }

    public Where(ParameterCollection parameters)
      : base(parameters) { }

    public override string Sql()
    {
      string sql = base.Sql();

      if (string.IsNullOrEmpty(sql))
      {
        return null;
      }

      return string.Concat("where ", sql);
    }
  }
}