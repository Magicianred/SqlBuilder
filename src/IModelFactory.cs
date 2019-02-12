namespace SqlBuilder
{
  public interface IModelFactory<TDataModel, TViewModel> : IModelFactory
    where TDataModel : DataModel, new()
    where TViewModel : new()
  {
    /// <summary>
    /// Returns a new instance of <see cref="TViewModel"/>.
    /// </summary>
    /// <returns></returns>
    TViewModel CreateViewModel();

    /// <summary>
    /// Returns a new instance of <see cref="TViewModel"/> populated from the <see cref="TDataModel"/>.
    /// </summary>
    /// <param name="dataModel"></param>
    /// <returns></returns>
    TViewModel CreateViewModel(TDataModel dataModel);

    /// <summary>
    /// Returns a new instance of <see cref="TDataModel"/> populated from the <see cref="TViewModel"/>.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    TDataModel CreateDataModel(TViewModel model);

    void PopulateDataModel(TDataModel dataModel, TViewModel model);

    void PopulateViewModel(TDataModel dataModel, TViewModel model);
  }

  public interface IModelFactory
  {
    string FindDataModelMember(string name);
  }
}