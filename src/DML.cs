using System.Linq;
using System.Reflection;

namespace SqlBuilder
{
  public abstract class DML<TDataModel> : DML
  {
    public DML(TDataModel dataModel)
      : this()
    {
      Parameters.AddRange(Definition.GetEditableColumns().ToDictionary(x => ParameterCollection.GetName(x.Name), x => x.GetMember().GetMemberValue(dataModel)));
    }

    public DML(ParameterCollection parameters = null)
      : base(parameters)
    {
      Definition = ModelDefinition.GetDefinition<TDataModel>();
    }

    public override string Table()
    {
      return Definition.GetTableName();
    }

    protected readonly ModelDefinition Definition;
  }

  public abstract class DML : Statement
  {
    public DML(ParameterCollection parameters = null)
      : base(parameters) { }

    public DML Table(string table)
    {
      _table = table;
      return this;
    }

    public virtual string Table()
    {
      return _table;
    }

    private string _table;
  }
}