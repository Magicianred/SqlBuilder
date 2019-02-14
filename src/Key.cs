using System;
using System.Collections.Generic;

namespace SqlBuilder
{
  public struct Key
  {
    public Key(Type dataModelType)
      : this(null, dataModelType) { }

    public Key(Type viewModelType, Type dataModelType)
    {
      ViewModelType = viewModelType;
      DataModelType = dataModelType ?? throw new ArgumentNullException(nameof(dataModelType));
    }

    public readonly Type ViewModelType;

    public readonly Type DataModelType;
  }

  public class KeyComparer : IEqualityComparer<Key>
  {
    public bool Equals(Key x, Key y)
    {
      // if it's a key without viewModelType, just compare the dataModelType
      if (x.ViewModelType == null)
      {
        return x.DataModelType == y.DataModelType;
      }

      return x.ViewModelType == y.ViewModelType && x.DataModelType == y.DataModelType;
    }

    public int GetHashCode(Key obj)
    {
      // if it's a key without viewModelType, just compare the dataModelType
      if (obj.ViewModelType == null)
      {
        return obj.GetHashCode();
      }

      int hash = 17;
      hash = hash * 31 + obj.ViewModelType.GetHashCode();
      hash = hash * 31 + obj.DataModelType.GetHashCode();
      return hash;
    }
  }
}