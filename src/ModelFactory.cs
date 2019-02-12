using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SqlBuilder
{
  public class ModelFactory<TDataModel, TViewModel> : ModelFactory, IModelFactory<TDataModel, TViewModel>
    where TDataModel : DataModel, new()
    where TViewModel : new()
  {
    public ModelFactory()
    {
      _key = new Key(typeof(TViewModel), typeof(TDataModel));
    }

    public TViewModel CreateViewModel()
    {
      return new TViewModel();
    }

    public TViewModel CreateViewModel(TDataModel dataModel)
    {
      TViewModel model = CreateViewModel();
      PopulateViewModel(dataModel, model);
      return model;
    }

    public TDataModel CreateDataModel(TViewModel viewModel)
    {
      TDataModel dataModel = new TDataModel();
      PopulateDataModel(dataModel, viewModel);
      return dataModel;
    }

    /// <summary>
    /// Finds the data model member name for the given model member name.
    /// </summary>
    /// <param name="viewModelMemberName"></param>
    /// <returns></returns>
    public string FindDataModelMember(string viewModelMemberName)
    {
      IEnumerable<ModelColumn> columns;

      if (!Cache.TryGetValue(_key, out columns))
      {
        throw new InvalidOperationException("The DataModel does not exist in the cache. Has it been registered?");
      }

      ModelColumn column = columns.FirstOrDefault(x => string.Equals(x.ViewModelMember.Name, viewModelMemberName, StringComparison.OrdinalIgnoreCase));

      if (column != null)
      {
        return column.GetMember().Name;
      }

      return viewModelMemberName;
    }

    public void PopulateViewModel(TDataModel dataModel, TViewModel viewModel)
    {
      foreach (ModelColumn column in GetColumns(_key))
      {
        object value = column.GetMember().GetMemberValue(dataModel);
        column.ViewModelMember.SetMemberValue(viewModel, value);
      }

      dataModel.Bind(viewModel);
    }

    public void PopulateDataModel(TDataModel dataModel, TViewModel viewModel)
    {
      foreach (ModelColumn column in GetColumns(_key))
      {
        object value = column.ViewModelMember.GetMemberValue(viewModel);
        column.GetMember().SetMemberValue(dataModel, value);
      }
    }

    private readonly Key _key;
  }

  public class ModelFactory<TDataModel> : ModelFactory
      where TDataModel : DataModel, new()
  {
    public ModelFactory()
    {
      _key = new Key(typeof(TDataModel));
    }

    public void PopulateDataModel(TDataModel from, TDataModel to)
    {
      foreach (ModelColumn column in GetColumns(_key))
      {
        object value = column.GetMember().GetMemberValue(from);
        column.GetMember().SetMemberValue(to, value);
      }
    }

    private readonly Key _key;
  }

  public class ModelFactory
  {
    static ModelFactory()
    {
      Cache = new Dictionary<Key, IEnumerable<ModelColumn>>();
    }

    public static void RegisterTypes(params Assembly[] assemblies)
    {
      foreach (Assembly assembly in assemblies)
      {
        foreach (Type type in assembly.GetTypesThatInherit<DataModel>())
        {
          RegisterType(type);
        }
      }
    }

    public static void RegisterType<T>()
      where T : DataModel
    {
      RegisterType(typeof(T));
    }

    public static void RegisterType(Type dataModelType)
    {
      if (!dataModelType.Inherits(typeof(DataModel)))
      {
        throw new ArgumentException($"{dataModelType.FullName} does not inherit {nameof(DataModel)}", nameof(dataModelType));
      }

      Type[] genericTypes = dataModelType.BaseType.GetGenericArguments();
      Key key;
      IEnumerable<ModelColumn> columns;

      switch (genericTypes.Length)
      {
        case 0:
          {
            key = new Key(dataModelType);
            columns = FindColumns(dataModelType);
            break;
          }
        case 2:
          {
            // need to invoke the model instance to get the mappings
            DataModel dataModel = (DataModel)dataModelType.GetConstructor(Type.EmptyTypes).Invoke(null);
            Type viewModelType = genericTypes[1];
            key = new Key(viewModelType, dataModelType);
            columns = FindColumns(viewModelType, dataModelType, dataModel.GetMappings());
            break;
          }
        default:
          {
            throw new ArgumentException($"{dataModelType.FullName} does not have the correct number of generic arguments.", nameof(dataModelType));
          }
      }

      if (Cache.ContainsKey(key))
      {
        throw new InvalidOperationException($"There is already a data model type registered in the model factory for {dataModelType.FullName}.");
      }

      Cache.Add(key, columns);
    }

    public static IEnumerable<ModelColumn> FindColumns(Type dataModelType)
    {
      return new List<ModelColumn>(dataModelType.GetPublicMembers().Select(x => new ModelColumn(x)));
    }

    /// <summary>
    /// Returns an enumerable of <see cref="ModelColumn"/> mapped for the model and data model.
    /// </summary>
    /// <param name="viewModelType"></param>
    /// <param name="dataModelType"></param>
    /// <param name="customMappings"></param>
    /// <returns></returns>
    public static IEnumerable<ModelColumn> FindColumns(Type viewModelType, Type dataModelType, Dictionary<string, string> customMappings = null)
    {
      List<ModelColumn> columns = new List<ModelColumn>();

      foreach (MemberInfo viewModelMember in viewModelType.GetPublicMembers())
      {
        MemberInfo dataModelMember = FindDataModelMember(dataModelType, viewModelMember, customMappings);

        if (dataModelMember == null)
          continue;

        columns.Add(new ModelColumn(viewModelMember, dataModelMember));
      }

      return columns;
    }

    public static IEnumerable<Key> CacheKeys
    {
      get
      {
        return Cache.Keys;
      }
    }

    protected static IEnumerable<ModelColumn> GetColumns(Key key)
    {
      if (!Cache.ContainsKey(key))
      {
        throw new InvalidOperationException($"The type has not been registered.  Ensure you call {nameof(ModelFactory.RegisterType)} with the datamodel type.");
      }

      return Cache[key];
    }

    private static MemberInfo FindDataModelMember(Type dataModelType, MemberInfo viewModelMember, Dictionary<string, string> customMappings = null)
    {
      string name = viewModelMember.Name;

      if (customMappings != null && customMappings.ContainsKey(viewModelMember.Name))
      {
        name = customMappings[viewModelMember.Name];
      }

      return dataModelType.GetPublicMembers().FirstOrDefault(x => string.Equals(x.Name, name, StringComparison.OrdinalIgnoreCase));
    }

    protected static Dictionary<Key, IEnumerable<ModelColumn>> Cache;
  }
}