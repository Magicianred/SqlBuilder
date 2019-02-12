using System.Reflection;

namespace SqlBuilder
{
  public class ModelColumn : Column
  {
    public ModelColumn(MemberInfo viewModelMember, MemberInfo dataModelMember)
      : base(dataModelMember)
    {
      ViewModelMember = viewModelMember;
    }

    public ModelColumn(MemberInfo dataModelMember)
      : base(dataModelMember) { }

    public readonly MemberInfo ViewModelMember;
  }
}