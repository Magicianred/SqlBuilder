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
      this.Add(@operator, property.MemberName(), sqlOperator, value);
      return this;
    }

    public IClauseCollection<T> Or<TProp>(Expression<Func<T, TProp>> property, SqlOperator sqlOperator, TProp value)
    {
      return Add(SqlOr, property, sqlOperator, value);
    }

    public new IClauseCollection<T> And()
    {
      return AddCollection(SqlAnd);
    }

    public new IClauseCollection<T> Or()
    {
      return AddCollection(SqlOr);
    }

    private ClauseCollection<T> AddCollection(string @operator)
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
      return AddCollection(SqlAnd);
    }

    public IClauseCollection And(string column, SqlOperator sqlOperator, object value)
    {
      this.Add(SqlAnd, column, sqlOperator, value);
      return this;
    }

    public IClauseCollection Or()
    {
      return AddCollection(SqlOr);
    }

    public IClauseCollection Or(string column, SqlOperator sqlOperator, object value)
    {
      this.Add(SqlOr, column, sqlOperator, value);
      return this;
    }

    public override string Sql()
    {
      if (Count == 0)
      {
        return null;
      }

      string sql;

      if (Count == 1)
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

    public void Add(string @operator, Clause clause)
    {
      // only add the operator when we are on the next clause
      if (Count > 0)
      {
        if (string.IsNullOrEmpty(@operator))
        {
          throw new InvalidOperationException("Subsequent clauses must supply an operator.");
        }

        clause.Operator = @operator;
      }

      Clauses.Add(clause);
    }

    public void Add(string sql)
    {
      Clause clause = new Clause(sql, Parameters);
      Clauses.Add(clause);
    }

    public int Count
    {
      get
      {
        return Clauses.Count;
      }
    }

    protected void Add(ClauseCollection clauseCollection)
    {
      Clauses.Add(clauseCollection);
    }

    private ClauseCollection AddCollection(string @operator)
    {
      ClauseCollection clauseCollection = new ClauseCollection(Parameters, @operator);
      Add(clauseCollection);
      return clauseCollection;
    }

    public const string ParameterPrefix = "@";

    private List<Clause> Clauses;
  }
}