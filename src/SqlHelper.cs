namespace SqlBuilder
{
  public static class SqlHelper
  {
    public static string ToLikeString(string value)
    {
      if (string.IsNullOrWhiteSpace(value))
        return value;

      return string.Concat(_likeOperator, value, _likeOperator);
    }

    public static string ToStartsLikeString(string value)
    {
      if (string.IsNullOrWhiteSpace(value))
        return value;

      return string.Concat(value, _likeOperator);
    }

    public static string ToEndsLikeString(string value)
    {
      if (string.IsNullOrWhiteSpace(value))
        return value;

      return string.Concat(_likeOperator, value);
    }

    public static string GetOffset(int page, int maxPerPage)
    {
      int offset = page == 1 ? 0 : (maxPerPage * (page - 1));
      return $"OFFSET {offset} ROWS FETCH NEXT {maxPerPage} ROWS ONLY";
    }

    public static string ToPagedSql(string sql, string orderBy, bool ascending, int page, int maxPerPage)
    {
      string offset = GetOffset(page, maxPerPage);

      return $@"
          ;WITH   _data
          AS (
            {sql}
            ORDER
            BY      {orderBy} {(ascending ? string.Empty : " DESC")}
            {offset}
          )
          SELECT  *
          FROM    _data
          
          ;WITH   _count
          AS (
            {sql}
          )
          SELECT  COUNT(0)
          FROM    _count
      ";
    }

    private const char _likeOperator = '%';
  }
}