using System;

namespace SqlBuilder.Attributes
{
  [AttributeUsage(AttributeTargets.Class)]
  public class SqlOptionsAttribute : Attribute
  {
    public SqlOptions Options;
  }
}