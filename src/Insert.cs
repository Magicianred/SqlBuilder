using System.Linq;
using System.Reflection;

namespace SqlBuilder
{
  public class Insert<TDataModel> : DML<TDataModel>
  {
    public Insert(TDataModel dataModel, bool retrieveKey = false)
      : base()
    {
      _retrieveKey = retrieveKey;
      Parameters.AddRange(Definition.GetEditableColumns().ToDictionary(x => ParameterCollection.GetName(x.Name), x => x.GetMember().GetMemberValue(dataModel)));
    }

    public override string Sql()
    {
      string insert = $"insert into {Table()}({string.Join(ListSeparator, Definition.EditableColumnNames())})";
      string values = $"values({string.Join(ListSeparator, Parameters.Names)});";

      if (_retrieveKey)
      {
        return string.Concat($"declare @output table(_key int);", insert, $" output inserted.{Definition.Key} into @output ", values, "select _key from @output;");
      }

      return string.Concat(insert, " ", values);
    }

    private readonly bool _retrieveKey;
  }
}