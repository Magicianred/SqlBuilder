using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SqlBuilder
{
  public class Update<TDataModel> : DML<TDataModel>
  {
    public Update(object key, TDataModel dataModel)
      : base(dataModel)
    {
      CheckDefinition();

      Parameters.Add(Definition.Key, key);
      _sets = Definition.GetEditableColumns().Select(x => x.Name);
    }

    /// <summary>
    /// Creates an instance of <see cref="Update{TDataModel}"/> which only updates properties that are different.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="current"></param>
    /// <param name="next"></param>
    public Update(object key, TDataModel current, TDataModel next)
      : base()
    {
      CheckDefinition();

      Parameters = GetDiffParameters(current, next);
      _sets = Parameters.Select(x => string.Concat(x.Key));

      // if we have parameters, add the key
      if (Parameters.Any())
      {
        Parameters.Add(Definition.Key, key);
      }
    }

    public override string Sql()
    {
      if (!_sets.Any())
      {
        return null;
      }

      string set = string.Join(ListSeparator, _sets.Select(x => string.Concat(x, "=", ParameterCollection.GetName(x))));
      return $"update {Table()} set {set} where {Definition.Key}={ParameterCollection.GetName(Definition.Key)}";
    }

    /// <summary>
    /// Returns a parameter collection containing only the parameters that are different between the two objects.
    /// </summary>
    /// <param name="current"></param>
    /// <param name="next"></param>
    /// <returns></returns>
    private ParameterCollection GetDiffParameters(TDataModel current, TDataModel next)
    {
      ParameterCollection parameters = new ParameterCollection();

      foreach (Column column in Definition.GetEditableColumns())
      {
        object currentValue = column.GetMember().GetMemberValue(current);
        object nextValue = column.GetMember().GetMemberValue(next);

        if (currentValue != nextValue)
        {
          parameters.Add(column.Name, nextValue);
        }
      }

      return parameters;
    }

    private void CheckDefinition()
    {
      if(string.IsNullOrEmpty(Definition.Key))
      {
        throw new InvalidOperationException($"The data model {nameof(TDataModel)} does not have a key specified. Updates require a known key.");
      }
    }

    private IEnumerable<string> _sets;
  }
}