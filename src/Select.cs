using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace SqlBuilder
{
  public class Select<TDataModel> : Select
  {
    public Select(string columns = Wildcard, bool includeCount = false, string alias = null)
      : this(ModelDefinition.GetDefinition<TDataModel>(), columns, includeCount, alias) { }

    public Select(ModelDefinition definition, string columns = Wildcard, bool includeCount = false, string alias = null)
      : base(columns, includeCount)
    {
      _definition = definition;
      _alias = alias;

      if (!string.IsNullOrEmpty(_definition.OrderBy))
      {
        _orderBy = new OrderBy(_definition.OrderBy);
      }
    }

    public Select<TDataModel> Column<TProp>(Expression<Func<TDataModel, TProp>> property)
    {
      string name = property.MemberName();

      // add alias?
      if (!string.IsNullOrEmpty(_alias))
      {
        name = string.Concat(_alias, Dot, name);
      }

      // if we don't have columns set or it's the default (*) then overwrite with single column
      if (string.IsNullOrEmpty(_columns) || _columns == Wildcard)
      {
        _columns = name;
      }
      else
      {
        _columns = string.Concat(_columns, ListSeparator, name);
      }

      return this;
    }

    public Select<TDataModel> OrderBy<TProp>(Expression<Func<TDataModel, TProp>> property, bool ascending = true)
    {
      OrderBy(property.MemberName(), ascending);
      return this;
    }

    public Select<TDataModel> GroupBy<TProp>(Expression<Func<TDataModel, TProp>> property, OrderBy orderBy)
    {
      GroupBy(property);
      _orderBy = orderBy;
      return this;
    }

    public Select<TDataModel> GroupBy<TProp>(Expression<Func<TDataModel, TProp>> property)
    {
      GroupBy(property.MemberName());
      return this;
    }

    public IClauseCollection<TDataModel> Where<TProp>(Expression<Func<TDataModel, TProp>> property, TProp value, Func<bool> enabled = null)
    {
      return Where(property, SqlOperator.Equal, value);
    }

    public IClauseCollection<TDataModel> Where<TProp>(Expression<Func<TDataModel, TProp>> property, SqlOperator sqlOperator, TProp value, Func<bool> enabled = null)
    {
      if (_where == null)
      {
        _where = new Where<TDataModel>(Parameters);
      }

      if (enabled != null && !enabled())
      {
        return _where;
      }

      return _where.And(property, sqlOperator, value);
    }

    public override IWhere Where()
    {
      if(_where != null)
      {
        return _where;
      }

      return base.Where();
    }

    public override string Columns()
    {
      // if we have columns and they aren't the default (*) return them, otherwise, check the model definition
      if (!string.IsNullOrEmpty(_columns) && _columns != Wildcard)
      {
        return _columns;
      }

      // if we have an alias, provide it as part of the name
      bool hasAlias = !string.IsNullOrEmpty(_alias);

      // column list
      string columns = string.Join(ListSeparator, _definition.GetDatabaseColumns().Select(x => hasAlias ? string.Concat(_alias, Dot, x.Name) : x.Name));

      if (!string.IsNullOrEmpty(columns))
      {
        return columns;
      }

      return Wildcard;
    }

    public Select Columns(string columns)
    {
      _columns = columns;
      return this;
    }

    public override OrderBy OrderBy()
    {
      return base.OrderBy() ?? _orderBy;
    }

    public override string From()
    {
      string tableName = _definition.GetTableName();

      if (string.IsNullOrEmpty(_alias))
      {
        return tableName;
      }

      return string.Concat(tableName, Space, _alias);
    }

    public override SqlOptions Options()
    {
      SqlOptions options = base.Options();

      if (options == SqlOptions.None)
      {
        options = _definition.Options;
      }

      return options;
    }

    private readonly ModelDefinition _definition;

    private Where<TDataModel> _where;

    private OrderBy _orderBy;

    private readonly string _alias;
  }

  public class Select : SqlText
  {
    public Select(string columns = Wildcard, bool includeCount = false)
      : base()
    {
      _columns = columns;
      _includeCount = includeCount;
    }

    public virtual string Columns()
    {
      return _columns;
    }

    public Select From(string from)
    {
      _from = from;
      return this;
    }

    public virtual string From()
    {
      return _from;
    }

    public JoinCollection Join()
    {
      return _joins;
    }

    public Select Join(string sql)
    {
      if (_joins == null)
      {
        _joins = new JoinCollection();
      }

      _joins.Add(sql);

      return this;
    }

    public Select OrderBy(string column, bool ascending = true)
    {
      string sql = ascending ? column : string.Concat(column, " desc");
      _orderBy = new OrderBy(sql);
      return this;
    }

    public virtual OrderBy OrderBy()
    {
      return _orderBy;
    }

    public Select GroupBy(string sql)
    {
      _groupBy = new GroupBy(sql);
      return this;
    }

    public virtual GroupBy GroupBy()
    {
      return _groupBy;
    }

    public Select Paging(int page, int pageSize)
    {
      _paging = new Paging(page, pageSize);
      return this;
    }

    public virtual Paging Paging()
    {
      if (_paging != null && OrderBy() == null)
      {
        throw new InvalidOperationException("If using paging, an order by must be specified.");
      }

      return _paging;
    }

    public IClauseCollection Where(string sql)
    {
      if (_where == null)
      {
        _where = new Where(Parameters);
      }

      return _where.And(sql);
    }

    public virtual IWhere Where()
    {
      return _where;
    }

    public IWhere Where(Clause clause)
    {
      IWhere where = Where(); // in case it's been overriden

      if (where == null)
      {
        _where = new Where(clause);
        return _where;
      }

      where.Add(clause);
      return where;
    }

    public virtual bool IncludeCount()
    {
      return _includeCount;
    }

    public virtual Select IncludeCount(bool includeCount)
    {
      _includeCount = includeCount;
      return this;
    }

    public Select Options(SqlOptions options)
    {
      _options = options;
      return this;
    }

    public virtual SqlOptions Options()
    {
      return _options;
    }

    public Select Randomise()
    {
      // we do this in case of any overrides
      OrderBy orderBy = OrderBy();
      const string sql = "NEWID()";

      if (orderBy == null)
      {
        OrderBy(sql);
      }
      else
      {
        orderBy.Add(0, sql);
      }

      return this;
    }

    public override string Sql()
    {
      StringBuilder builder = new StringBuilder();
      string columns = Columns();
      OrderBy orderBy = OrderBy();
      GroupBy groupBy = GroupBy();
      Paging paging = Paging();
      string from = From();
      IWhere where = Where();
      SqlOptions sqlOptions = Options();
      Options options = new Options(sqlOptions);

      // options
      if (sqlOptions.HasFlag(SqlOptions.SetArithabort))
      {
        builder.AppendLine($"set arithabort on");
      }

      // select from
      if (paging != null && paging.IsTop)
      {
        builder.Append($"select top {paging.PageSize} {columns} from {from}");
      }
      else
      {
        builder.Append($"select {columns} from {from}");
      }

      // joins
      if (_joins != null)
      {
        _joins.Sql(builder);
      }

      // where
      if (where != null)
      {
        where.Sql(builder);
      }

      // group by
      if (groupBy != null)
      {
        groupBy.Sql(builder);
      }

      // order by
      if (orderBy != null)
      {
        orderBy.Sql(builder);
      }

      // pagination
      if (paging != null && !paging.IsTop)
      {
        paging.Sql(builder);
      }

      // query options
      options.Sql(builder);

      // count recordset
      if (IncludeCount())
      {
        builder.Append($";select count(*) from {from}");

        if (where != null)
        {
          where.Sql(builder);
        }

        // query options
        options.Sql(builder);
      }

      return builder.ToString();
    }

    public const string Wildcard = "*";

    protected string _columns;

    private string _from;

    private IWhere _where;

    private Paging _paging;

    private OrderBy _orderBy;

    private GroupBy _groupBy;

    private JoinCollection _joins;

    private bool _includeCount;

    private SqlOptions _options = SqlOptions.None;
  }
}