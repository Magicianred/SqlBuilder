using SqlBuilder.Attributes;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace SqlBuilder
{
  public class Column
  {
    public Column(MemberInfo member)
    {
      KeyAttribute key = member.GetCustomAttribute<KeyAttribute>();
      ReadOnlyAttribute readOnly = member.GetCustomAttribute<ReadOnlyAttribute>();
      ColumnAttribute column = member.GetCustomAttribute<ColumnAttribute>();
      OrderByAttribute orderBy = member.GetCustomAttribute<OrderByAttribute>();
      DescriptionAttribute description = member.GetCustomAttribute<DescriptionAttribute>();

      string name = column != null ? column.Name : member.Name;
      bool isKey = key != null;
      bool isReadOnly = readOnly != null && readOnly.IsReadOnly;
      string order = null;

      if (orderBy != null)
      {
        if (orderBy.Ascending)
        {
          order = name;
        }
        else
        {
          order = string.Concat(name, " desc");
        }
      }

      if (description != null)
      {
        Title = description.Description;
      }
      else
      {
        Title = name;
      }

      Member = member;
      Name = name;
      IsKey = isKey;
      IsReadOnly = isReadOnly;
      OrderBy = order;

      Type returnType = member.ReturnType();
      Type nullableType;
      IsNullable = returnType.IsNullable(out nullableType);
      Type = IsNullable ? nullableType.Name : returnType.Name;
    }

    public MemberInfo GetMember()
    {
      return Member;
    }

    public readonly string Name;

    public string CamelCaseName
    {
      get
      {
        return Name.ToCamelCase();
      }
    }

    public readonly string Title;

    public readonly string OrderBy;

    public readonly bool IsKey;

    public readonly bool IsReadOnly;

    public bool IsEditable
    {
      get
      {
        return !(IsKey || IsReadOnly);
      }
    }

    public readonly string Type;

    public readonly bool IsNullable;

    /// <summary>
    /// This needs to be internal so it doesn't get serialised back to the ui
    /// </summary>
    internal readonly MemberInfo Member;
  }
}