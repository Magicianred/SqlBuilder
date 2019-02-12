using System;

namespace SqlBuilder.Attributes
{
  [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
  public class OrderByAttribute : Attribute
  {
    public OrderByAttribute(bool ascending = true)
    {
      Ascending = ascending;
    }

    public int Order = 1;

    public readonly bool Ascending;
  }
}