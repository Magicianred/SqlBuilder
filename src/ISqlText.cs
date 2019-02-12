using System.Text;

namespace SqlBuilder
{
  public interface ISqlText
  {
    string Sql();

    void Sql(StringBuilder builder);

    ParameterCollection Parameters { get; }
  }
}