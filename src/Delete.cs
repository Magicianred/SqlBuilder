namespace SqlBuilder
{
  public class Delete<TDataModel> : DML<TDataModel>
  {
    public Delete(object key)
    {
      _key = key;
      Parameters.Add(Definition.Key, key);
    }

    public override string Sql()
    {
      return $"delete from {Table()} where {Definition.Key}=@{Definition.Key}";
    }

    private readonly object _key;
  }
}