using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace System
{
  internal static class TypeExtensions
  {
    /// <summary>
    /// Returns the public instance properties and fields for the given type.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static IEnumerable<MemberInfo> GetPublicMembers(this Type type)
    {
      const BindingFlags _flags = BindingFlags.Instance | BindingFlags.Public;
      return type.GetFields(_flags)
        .Cast<MemberInfo>()
        .Concat(type.GetProperties(_flags));
    }
    
    /// <summary>
    /// If true, the given type inherits <see cref="baseType"/>.
    /// </summary>
    /// <param name="type"></param>
    /// <param name="baseType"></param>
    /// <returns></returns>
    public static bool Inherits(this Type type, Type baseType)
    {
      if (type.BaseType == null)
      {
        return false;
      }

      if (type.BaseType == baseType)
      {
        return true;
      }

      if (baseType.IsAssignableFrom(type.BaseType) || baseType.IsAssignableFrom(type))
      {
        return true;
      }

      if (type.BaseType.IsGenericType)
      {
        return type.BaseType.GetGenericTypeDefinition() == baseType;
      }

      return type.BaseType == baseType;
    }

    /// <summary>
    /// Returns the return type of the field or property.
    /// </summary>
    /// <param name="member"></param>
    /// <returns></returns>
    public static Type ReturnType(this MemberInfo member)
    {
      switch (member.MemberType)
      {
        case MemberTypes.Field:
          {
            return ((FieldInfo)member).FieldType;
          }
        case MemberTypes.Property:
          {
            return ((PropertyInfo)member).PropertyType;
          }
        default:
          {
            throw new ArgumentException("MemberInfo must be of type FieldInfo or PropertyInfo", "member");
          }
      }
    }

    /// <summary>
    /// If true, the type is nullable.
    /// </summary>
    /// <param name="type"></param>
    /// <param name="underlyingType"></param>
    /// <returns></returns>
    public static bool IsNullable(this Type type, out Type underlyingType)
    {
      underlyingType = Nullable.GetUnderlyingType(type);
      return underlyingType != null;
    }

    /// <summary>
    /// Returns the types in the assembly that inherit the given type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="assembly"></param>
    /// <returns></returns>
    public static IEnumerable<Type> GetTypesThatInherit<T>(this Assembly assembly)
    {
      return GetTypesThatInherit(assembly, typeof(T));
    }

    /// <summary>
    /// Returns the types in the assembly that inherit the given type.
    /// </summary>
    /// <param name="assembly"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public static IEnumerable<Type> GetTypesThatInherit(this Assembly assembly, Type type)
    {
      return assembly.GetTypes().Where(x => x.Inherits(type));
    }
  }
}