using System.Collections;
using System.Collections.Generic;

namespace SqlBuilder
{
  public class ParameterCollection : IEnumerable<KeyValuePair<string, object>>
  {
    public ParameterCollection(Dictionary<string, object> parameters)
    {
      _parameters = parameters ?? new Dictionary<string, object>(0);
    }

    public ParameterCollection(int capacity)
      : this(new Dictionary<string, object>(capacity)) { }

    public ParameterCollection()
      : this(0) { }

    /// <summary>
    /// Adds a new parameter with a name based on the index of the new parameter i.e. @p1
    /// </summary>
    /// <param name="value"></param>
    /// <returns>The key used</returns>
    public string Add(object value)
    {
      string key = NextKey();
      Add(key, value);
      return key;
    }

    public void Add(string name, object value)
    {
      _parameters.Add(GetName(name), value);
    }

    public void AddRange(Dictionary<string, object> parameters)
    {
      foreach (KeyValuePair<string, object> parameter in parameters)
      {
        Add(parameter.Key, parameter.Value);
      }
    }

    public string NextKey()
    {
      return GetName(string.Concat(_parameterPrefix, _parameters.Count.ToString()));
    }

    public int Count
    {
      get
      {
        return _parameters.Count;
      }
    }

    public object this[string key]
    {
      get
      {
        return _parameters[GetName(key)];
      }
      set
      {
        _parameters[GetName(key)] = value;
      }
    }

    IEnumerator<KeyValuePair<string, object>> IEnumerable<KeyValuePair<string, object>>.GetEnumerator()
    {
      return _parameters.GetEnumerator();
    }

    public IEnumerator GetEnumerator()
    {
      return _parameters.GetEnumerator();
    }

    public IEnumerable<string> Names
    {
      get
      {
        return _parameters.Keys;
      }
    }

    public static string GetName(string name)
    {
      if (!name.StartsWith(_prefix))
      {
        return string.Concat(_prefix, name);
      }

      return name;
    }

    private readonly Dictionary<string, object> _parameters;

    private const string _prefix = "@";

    private const string _parameterPrefix = _prefix + "p";
  }
}