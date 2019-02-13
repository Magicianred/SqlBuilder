namespace System.Linq.Expressions
{
  internal static class ExpressionExtensions
  {
    /// <summary>
    /// Returns the member name from a given expression.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="V"></typeparam>
    /// <param name="expression"></param>
    /// <returns></returns>
    public static string MemberName<T, V>(this Expression<Func<T, V>> expression)
    {
      return GetBody(expression).Member.Name;
    }

    /// <summary>
    /// Returns the member expression from an expression.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="V"></typeparam>
    /// <param name="expression"></param>
    /// <returns></returns>
    private static MemberExpression GetBody<T, V>(this Expression<Func<T, V>> expression)
    {
      MemberExpression body = expression.Body as MemberExpression;

      if (body == null)
      {
        UnaryExpression ubody = (UnaryExpression)expression.Body;
        body = ubody.Operand as MemberExpression;
      }

      if (body == null)
      {
        throw new InvalidOperationException("Expression must be a member expression");
      }

      return body;
    }
  }
}