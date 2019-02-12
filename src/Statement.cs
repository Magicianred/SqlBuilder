namespace SqlBuilder
{
  public abstract class Statement : SqlText
  {
    public Statement(ParameterCollection parameters = null)
      : base() { }
  }
}