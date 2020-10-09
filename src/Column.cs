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
      string name = member.TryGetCustomAttributeValue<ColumnAttribute, string>(attribute => attribute.Name, out string columnName) ? columnName : member.Name; 
      bool isKey = member.GetCustomAttribute<KeyAttribute>() != null;

      if(member.TryGetCustomAttributeValue<OrderByAttribute, bool>(attribute => attribute.Ascending, out bool isAscending))
      {
        OrderBy = isAscending ? name : $"{name} desc";
      }

      Title = member.TryGetCustomAttributeValue<DescriptionAttribute, string>(attribute => attribute.Description, out string description) ? description : name;
      Member = member;
      Name = name;
      IsKey = isKey;
      IsReadOnly = !CanWrite(member);
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
    private static bool CanWrite(MemberInfo member)
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