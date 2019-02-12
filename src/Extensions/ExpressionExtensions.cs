namespace System.Linq.Expressions
{
  internal static class ExpressionExtensions
  {
    public static string MemberName<T, V>(this Expression<Func<T, V>> expression)
    {
      return GetBody(expression).Member.Name;
    }

    public static MemberExpression GetBody<T, V>(this Expression<Func<T, V>> expression)
    {
      MemberExpression body = expression.Body as MemberExpression;

      if (body == null)
      {
        UnaryExpression ubody = (UnaryExpression)expression.Body;
        body = ubody.Operand as MemberExpression;
      }

      if (body == null)
        throw new InvalidOperationException("Expression must be a member expression");

      return body;
    }
  }
}