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
      ColumnAttribute column = member.GetCustomAttribute<ColumnAttribute>();
      OrderByAttribute orderBy = member.GetCustomAttribute<OrderByAttribute>();
      DescriptionAttribute description = member.GetCustomAttribute<DescriptionAttribute>();

      string name = column != null ? column.Name : member.Name;
      bool isKey = key != null;
      string order = null;

      if (orderBy != null)
      {
        order = orderBy.Ascending ? name : string.Concat(name, " desc");
      }

      Title = description != null ? description.Description : name;
      Member = member;
      Name = name;
      IsKey = isKey;
      IsReadOnly = !IsMutable(member);
      OrderBy = order;

      Type returnType = member.ReturnType();
      IsNullable = returnType.IsNullable(out Type nullableType);
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
    /// Returns whether the <paramref name="member"/> can be updated.
    /// </summary>
    /// <param name="member"></param>
    /// <returns></returns>
    private static bool IsMutable(MemberInfo member)
    {
      ReadOnlyAttribute readOnly = member.GetCustomAttribute<ReadOnlyAttribute>();

      // if there is an attribute respect the value
      if (readOnly?.IsReadOnly == true)
      {
        return false;
      }

      // otherwise, check the member type - if field or property, check readonly/private setter.
      switch (member.MemberType)
      {
        case MemberTypes.Field:
          {
            return !((FieldInfo)member).IsInitOnly;
          }
        case MemberTypes.Property:
          {
            PropertyInfo property = (PropertyInfo)member;

            if (!property.CanWrite)
            {
              return false;
            }

            return property.SetMethod.IsPublic;
          }
      }

      // default
      return true;
    }

    /// <summary>
    /// This needs to be internal so it doesn't get serialised back to the ui
    /// </summary>
    internal readonly MemberInfo Member;
  }
}