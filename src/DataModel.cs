using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;

namespace SqlBuilder
{
  public abstract class DataModel<TDataModel, TViewModel> : DataModel
      where TDataModel : DataModel, new()
      where TViewModel : new()
  {
    /// <summary>
    /// Maps a <see cref="TDataModel"/> member to a <see cref="TViewModel"/> member in situations where the names don't match.
    /// </summary>
    /// <typeparam name="TProp"></typeparam>
    /// <param name="dataModelMember"></param>
    /// <param name="viewModelMember"></param>
    public void Map<TProp>(Expression<Func<TDataModel, TProp>> dataModelMember, Expression<Func<TViewModel, TProp>> viewModelMember)
    {
      Map(viewModelMember.MemberName(), dataModelMember.MemberName());
    }
  }

  public abstract class DataModel : IValidatableObject
  {
    public DataModel()
    {
      _mappings = new Dictionary<string, string>(0);
    }

    public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
      return Enumerable.Empty<ValidationResult>();
    }

    public Dictionary<string, string> GetMappings()
    {
      return _mappings;
    }

    public virtual void Bind(object viewModel) { }

    protected void Map(string viewModelMemberName, string dataModelMemberName)
    {
      _mappings.Add(viewModelMemberName, dataModelMemberName);
    }

    private readonly Dictionary<string, string> _mappings;
  }
}