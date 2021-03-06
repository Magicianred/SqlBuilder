﻿using System;
using System.Linq.Expressions;

namespace SqlBuilder
{
  public static class IClauseCollectionExtensions
  {
    public static IClauseCollection And(this IClauseCollection clauseCollection, string column, object value)
    {
      return clauseCollection.And(column, SqlOperator.Equal, value);
    }

    public static IClauseCollection And(this IClauseCollection clauseCollection, string sql)
    {
      return Add(clauseCollection, Clause.SqlAnd, sql);
    }

    public static IClauseCollection<T> And<T, TProp>(this IClauseCollection<T> clauseCollection, Expression<Func<T, TProp>> property, TProp value)
    {
      return clauseCollection.And(property, SqlOperator.Equal, value);
    }

    public static IClauseCollection<T> And<T, TProp>(this IClauseCollection<T> clauseCollection, Expression<Func<T, TProp>> property, TProp value, Func<TProp, bool> applyPredicate)
    {
      if (applyPredicate(value))
      {
        And(clauseCollection, property, value);
      }

      return clauseCollection;
    }

    public static IClauseCollection<T> And<T, TProp>(this IClauseCollection<T> clauseCollection, Expression<Func<T, TProp>> property, SqlOperator sqlOperator, TProp value, Func<TProp, bool> applyPredicate)
    {
      if (applyPredicate(value))
      {
        clauseCollection.And(property, sqlOperator, value);
      }

      return clauseCollection;
    }

    public static IClauseCollection<T> AndIsNotNull<T, TProp>(this IClauseCollection<T> clauseCollection, Expression<Func<T, TProp>> property)
    {
      clauseCollection.And(property.MemberName(), SqlOperator.IsNot, "null");
      return clauseCollection;
    }

    public static IClauseCollection<T> AndIsNull<T, TProp>(this IClauseCollection<T> clauseCollection, Expression<Func<T, TProp>> property)
    {
      clauseCollection.And(property.MemberName(), SqlOperator.Is, "null");
      return clauseCollection;
    }

    public static IClauseCollection Or(this IClauseCollection clauseCollection, string column, object value)
    {
      return clauseCollection.Or(column, SqlOperator.Equal, value);
    }

    public static IClauseCollection Or(this IClauseCollection clauseCollection, string sql)
    {
      return Add(clauseCollection, Clause.SqlOr, sql);
    }

    public static IClauseCollection<T> Or<T, TProp>(this IClauseCollection<T> clauseCollection, Expression<Func<T, TProp>> property, TProp value)
    {
      return clauseCollection.Or(property, SqlOperator.Equal, value);
    }

    public static IClauseCollection<T> Or<T, TProp>(this IClauseCollection<T> clauseCollection, Expression<Func<T, TProp>> property, TProp value, Func<TProp, bool> applyPredicate)
    {
      if (applyPredicate(value))
      {
        Or(clauseCollection, property, value);
      }

      return clauseCollection;
    }

    public static IClauseCollection<T> Or<T, TProp>(this IClauseCollection<T> clauseCollection, Expression<Func<T, TProp>> property, SqlOperator sqlOperator, TProp value, Func<TProp, bool> applyPredicate)
    {
      if (applyPredicate(value))
      {
        clauseCollection.Or(property, sqlOperator, value);
      }

      return clauseCollection;
    }

    public static IClauseCollection<T> OrIsNotNull<T, TProp>(this IClauseCollection<T> clauseCollection, Expression<Func<T, TProp>> property)
    {
      clauseCollection.Or(property.MemberName(), SqlOperator.IsNot, "null");
      return clauseCollection;
    }

    public static IClauseCollection<T> OrIsNull<T, TProp>(this IClauseCollection<T> clauseCollection, Expression<Func<T, TProp>> property)
    {
      clauseCollection.Or(property.MemberName(), SqlOperator.Is, "null");
      return clauseCollection;
    }

    public static void Add(this IClauseCollection clauseCollection, string @operator, string column, SqlOperator sqlOperator, object value)
    {
      Clause clause = new Clause(clauseCollection.Parameters, column, sqlOperator, value);
      clauseCollection.Add(@operator, clause);
    }

    public static void Add(this IClauseCollection clauseCollection, string column, SqlOperator sqlOperator, object value)
    {
      Add(clauseCollection, null, column, sqlOperator, value);
    }

    public static void Add(this IClauseCollection clauseCollection, Clause clause)
    {
      if (clauseCollection.Count > 0)
      {
        clauseCollection.Add(Clause.SqlAnd, clause);
      }
      else
      {
        clauseCollection.Add(null, clause);
      }
    }

    private static IClauseCollection Add(this IClauseCollection clauseCollection, string @operator, string sql)
    {
      Clause clause = new Clause(sql, clauseCollection.Parameters);
      clauseCollection.Add(@operator, clause);
      return clauseCollection;
    }
  }
}