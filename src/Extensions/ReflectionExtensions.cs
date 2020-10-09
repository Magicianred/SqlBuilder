namespace System.Reflection
{
  internal static class ReflectionExtensions
  {
    /// <summary>
    /// Checks whether the custom attribute exists on the member and passes to the check factory.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="member"></param>
    /// <param name="checkFactory"></param>
    /// <returns></returns>
    public static bool AttributeHas<T>(this MemberInfo member, Func<T, bool> checkFactory)
        where T : Attribute
    {
      T attribute = member.GetCustomAttribute<T>();

      if (attribute == null)
      {
        return false;
      }

      return checkFactory(attribute);
    }

    /// <summary>
    /// Returns the value of the member if it's a field or property otherwise, null.
    /// </summary>
    /// <param name="member"></param>
    /// <param name="target"></param>
    /// <returns></returns>
    public static object GetMemberValue(this MemberInfo member, object target)
    {
      if (target == null)
      {
        return null;
      }

      switch (member.MemberType)
      {
        case MemberTypes.Field:
          {
            return ((FieldInfo)member).GetValue(target);
          }
        case MemberTypes.Property:
          {
            return ((PropertyInfo)member).GetValue(target);
          }
        default:
          {
            throw new ArgumentException("MemberInfo must be of type FieldInfo or PropertyInfo", nameof(member));
          }
      }
    }

    /// <summary>
    /// Sets the member's value on the target object.
    /// </summary>
    /// <param name="member"></param>
    /// <param name="target"></param>
    /// <param name="value"></param>
    public static void SetMemberValue(this MemberInfo member, object target, object value)
    {
      if (target == null)
      {
        return;
      }

      switch (member.MemberType)
      {
        case MemberTypes.Field:
          {
            ((FieldInfo)member).SetValue(target, value);
          }
          break;
        case MemberTypes.Property:
          {
            ((PropertyInfo)member).SetValue(target, value, null);
          }
          break;
        default:
          {
            throw new ArgumentException("MemberInfo must be of type FieldInfo or PropertyInfo", nameof(member));
          }
      }
    }

    /// <summary>
    /// Attempts to get the property value of the attribute.
    /// </summary>
    /// <typeparam name="TAttribute"></typeparam>
    /// <typeparam name="TProp"></typeparam>
    /// <param name="member"></param>
    /// <param name="expression"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool TryGetCustomAttributeValue<TAttribute, TProp>(this MemberInfo member, Func<TAttribute, TProp> expression, out TProp value)
      where TAttribute : Attribute
    {
      TAttribute attribute = member.GetCustomAttribute<TAttribute>();

      if (attribute != null)
      {
        value = expression(attribute);
        return true;
      }

      value = default(TProp);
      return false;
    }
  }
}