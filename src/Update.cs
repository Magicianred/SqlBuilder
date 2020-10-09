using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SqlBuilder
{
  public class Update<TDataModel> : DML<TDataModel>
  {
    public Update(object key, TDataModel dataModel)
      : base()
    {
      EnsureDefinition();

      Parameters.Add(Definition.Key, key);

      foreach (Column column in Definition.GetEditableColumns())
      {
        object value = column.GetMember().GetMemberValue(dataModel);
        string name = Parameters.Add(column.Name, value);
        _setParameters.Add(column.Name, name);
      }
    }

    /// <summary>
    /// Creates an instance of <see cref="Update{TDataModel}"/> which only updates properties that have changed.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="current"></param>
    /// <param name="next"></param>
    public Update(object key, TDataModel current, TDataModel next)
      : base()
    {
      EnsureDefinition();

      // for each column check the current value against the next to see if there has been a change
      foreach (Column column in Definition.GetEditableColumns())
      {
        object currentValue = column.GetMember().GetMemberValue(current);
        object nextValue = column.GetMember().GetMemberValue(next);

        if (currentValue != nextValue)
        {
          string name = Parameters.Add(column.Name, nextValue);
          _setParameters.Add(column.Name, name);
        }
      }

      // if we have any parameters to update from the check, add the key to the collection
      if (Parameters.Any())
      {
        Parameters.Add(Definition.Key, key);
      }
    }

    public override string Sql()
    {
      if (!_setParameters.Any())
      {
        return null;
      }

      string set = string.Join(ListSeparator, _setParameters.Select(x => $"{x.Key}={x.Value}"));
      return $"update {Table()} set {set} where {Definition.Key}={ParameterCollection.GetName(Definition.Key)}";
    }

    private void EnsureDefinition()
    {
      if (string.IsNullOrEmpty(Definition.Key))
      {
        throw new InvalidOperationException($"The data model type {typeof(TDataModel)} does not have a key specified. Update requires a known key.");
      }
    }

    private IDictionary<string, string> _setParameters = new Dictionary<string, string>(0); // init here so we don't have to in ctor
  }
}