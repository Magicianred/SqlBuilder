using System.Text;

namespace SqlBuilder
{
  public abstract class SqlText : ISqlText
  {
    public SqlText(ParameterCollection parameters = null)
    {
      Parameters = parameters ?? new ParameterCollection(0);
    }

    public abstract string Sql();

    public virtual void Sql(StringBuilder builder)
    {
      string sql = Sql();

      if (!string.IsNullOrWhiteSpace(sql))
      {
        if (builder.Length > 0)
        {
          builder.Append(" ");
        }

        builder.Append(sql);
      }
    }

    public virtual ParameterCollection Parameters { get; protected set; }

    public const string ListSeparator = ",";

    protected const string Space = " ";

    protected const string Dot = ".";
  }
}