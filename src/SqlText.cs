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

    public ParameterCollection Parameters { get; protected set; }

    public static string StartsWith(string value)
    {
      if (string.IsNullOrEmpty(value))
      {
        return value;
      }
      
      return string.Concat(LikeWildcard, value);
    }

    public static string EndsWith(string value)
    {
      if (string.IsNullOrEmpty(value))
      {
        return value;
      }

      return string.Concat(value, LikeWildcard);
    }

    public static string Contains(string value)
    {
      if (string.IsNullOrEmpty(value))
      {
        return value;
      }

      return string.Concat(LikeWildcard, value, LikeWildcard);
    }

    protected const string ListSeparator = ",";

    protected const string Space = " ";

    private const string LikeWildcard = "%";

    private const string SingleQuote = "'";
  }
}