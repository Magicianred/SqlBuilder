using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SqlBuilder
{
  public interface IClauseCollection<T> : IClauseCollection
  {
    IClauseCollection<T> And<TProp>(Expression<Func<T, TProp>> property, SqlOperator sqlOperator, TProp value);

    IClauseCollection<T> Or<TProp>(Expression<Func<T, TProp>> property, SqlOperator sqlOperator, TProp value);

    new IClauseCollection<T> And();

    new IClauseCollection<T> Or();
  }

  public interface IClauseCollection
  {
    IClauseCollection And();

    IClauseCollection And(string column, SqlOperator sqlOperator, object value);

    IClauseCollection Or();

    IClauseCollection Or(string column, SqlOperator sqlOperator, object value);
  }
}
