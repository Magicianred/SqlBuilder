using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SqlBuilder
{
  public class ClauseCollection<T> : ClauseCollection, IClauseCollection<T>
  {
    public ClauseCollection(ParameterCollection parameterCollection)
      : base(parameterCollection) { }

    public ClauseCollection(ParameterCollection parameterCollection, string @operator)
      : base(parameterCollection, @operator) { }

    public IClauseCollection<T> And<TProp>(Expression<Func<T, TProp>> property, SqlOperator sqlOperator, TProp value)
    {
      return Add(SqlAnd, property, sqlOperator, value);
    }

    public IClauseCollection<T> Add<TProp>(string @operator, Expression<Func<T, TProp>> property, SqlOperator sqlOperator, TProp value)
    {
      Add(@operator, property.MemberName(), sqlOperator, value);
      return this;
    }

    public IClauseCollection<T> Or<TProp>(Expression<Func<T, TProp>> property, SqlOperator sqlOperator, TProp value)
    {
      return Add(SqlOr, property, sqlOperator, value);
    }

    public new IClauseCollection<T> And()
    {
      return Add(SqlAnd);
    }

    public new IClauseCollection<T> Or()
    {
      return Add(SqlOr);
    }

    private ClauseCollection<T> Add(string @operator)
    {
      ClauseCollection<T> clauseCollection = new ClauseCollection<T>(Parameters, @operator);
      Add(clauseCollection);
      return clauseCollection;
    }
  }

  public class ClauseCollection : Clause, IClauseCollection
  {
    public ClauseCollection(ParameterCollection parameterCollection)
      : base(parameterCollection)
    {
      Clauses = new List<Clause>(0);
    }

    public ClauseCollection(ParameterCollection parameterCollection, string @operator)
      : this(parameterCollection)
    {
      Operator = @operator;
    }

    public IClauseCollection And()
    {
      return Add(SqlAnd);
    }

    public IClauseCollection And(string column, SqlOperator sqlOperator, object value)
    {
      Add(SqlAnd, column, sqlOperator, value);
      return this;
    }

    public IClauseCollection Or()
    {
      return Add(SqlOr);
    }

    public IClauseCollection Or(string column, SqlOperator sqlOperator, object value)
    {
      Add(SqlOr, column, sqlOperator, value);
      return this;
    }

    public override string Sql()
    {
      if (Clauses.Count == 0)
      {
        return null;
      }

      string sql;

      if (Clauses.Count == 1)
      {
        sql = Clauses[0].Sql();
      }
      else
      {
        string clausesSql = string.Join(Space, Clauses.Select(x => x.Sql()));
        sql = string.Concat("(", clausesSql, ")");
      }

      if (string.IsNullOrEmpty(Operator))
      {
        return sql;
      }

      return string.Concat(Operator, Space, sql);
    }

    public void Add(string @operator, string column, SqlOperator sqlOperator, object value)
    {
      Clause clause = new Clause(Parameters, column, sqlOperator, value);
      Add(@operator, clause);
    }

    protected void Add(string @operator, Clause clause)
    {
      // only add the operator when we are on the next clause
      if (Clauses.Count > 0)
      {
        if (string.IsNullOrEmpty(@operator))
        {
          throw new InvalidOperationException("Subsequent clauses must have a non null value.");
        }

        clause.Operator = @operator;
      }

      Clauses.Add(clause);
    }

    protected void Add(Clause clause)
    {
      Add(null, clause);
    }

    protected void Add(ClauseCollection clauseCollection)
    {
      Clauses.Add(clauseCollection);
    }

    private ClauseCollection Add(string @operator)
    {
      ClauseCollection clauseCollection = new ClauseCollection(Parameters, @operator);
      Add(clauseCollection);
      return clauseCollection;
    }

    public const string ParameterPrefix = "@";

    private List<Clause> Clauses;
  }
}