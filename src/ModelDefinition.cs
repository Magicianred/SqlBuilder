using SqlBuilder.Attributes;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;

namespace SqlBuilder
{
  public struct ModelDefinition
  {
    static ModelDefinition()
    {
      _definitionCache = new ConcurrentDictionary<Type, ModelDefinition>();
    }
    
    /// <summary>
    /// This ctor does not make use of caching
    /// </summary>
    /// <param name="type"></param>
    public ModelDefinition(Type type)
    {
      TableAttribute tableAttribute = type.GetCustomAttribute<TableAttribute>();
      SqlOptionsAttribute optionsAttribute = type.GetCustomAttribute<SqlOptionsAttribute>();

      _columns = type.GetPublicMembers().Select(x => new Column(x));
      Schema = "dbo";
      _tableName = type.Name;

      Column keyColumn = _columns.FirstOrDefault(x => x.IsKey);

      OrderBy = string.Join(",", _columns.Where(x => !string.IsNullOrEmpty(x.OrderBy)).Select(x => x.OrderBy));
      Key = keyColumn?.Name;
      Options = optionsAttribute != null ? optionsAttribute.Options : SqlOptions.None;

      if (tableAttribute != null)
      {
        Schema = tableAttribute.Schema ?? Schema;
        _tableName = tableAttribute.Name ?? _tableName;
      }
    }

    /// <summary>
    /// Returns <see cref="ModelDefinition"/> for the given type (uses cache).
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static ModelDefinition GetDefinition<T>()
    {
      Type type = typeof(T);
      return _definitionCache.GetOrAdd(type, t => new ModelDefinition(type));
    }

    public string GetTableName()
    {
      return TableName;
    }

    public IEnumerable<Column> GetDatabaseColumns()
    {
      return _columns.Where(x => !x.GetMember().AttributeHas<DatabaseGeneratedAttribute>(attr => attr.DatabaseGeneratedOption == DatabaseGeneratedOption.None));
    }

    public IEnumerable<Column> GetEditableColumns()
    {
      return _columns.Where(x => x.IsEditable);
    }

    public readonly string OrderBy;

    public readonly string Key;

    public readonly SqlOptions Options;

    public IEnumerable<string> EditableColumnNames()
    {
      return GetEditableColumns().Select(x => x.Name);
    }

    public IEnumerable<Column> Columns
    {
      get
      {
        return _columns;
      }
    }

    /// <summary>
    /// This needs to be internal so it doesn't get serialised back to the ui
    /// </summary>
    internal string TableName
    {
      get
      {
        const string period = ".";
        return string.Concat(Schema, period, _tableName);
      }
    }

    /// <summary>
    /// This needs to be internal so it doesn't get serialised back to the ui
    /// </summary>
    internal string Schema { get; }

    private readonly string _tableName;

    private readonly IEnumerable<Column> _columns;

    private readonly static ConcurrentDictionary<Type, ModelDefinition> _definitionCache;
  }
}