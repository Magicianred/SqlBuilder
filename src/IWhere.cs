namespace SqlBuilder
{
  public interface IWhere<T> : IWhere, IClauseCollection<T> { }

  public interface IWhere : IClauseCollection, ISqlText { }
}