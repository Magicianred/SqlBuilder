using System;

namespace SqlBuilder
{
  public struct Key
  {
    public Key(Type dataModelType)
      : this(null, dataModelType) { }

    public Key(Type viewModelType, Type dataModelType)
    {
      ViewModelType = viewModelType;
      DataModelType = dataModelType;
    }

    public readonly Type ViewModelType;

    public readonly Type DataModelType;
  }
}