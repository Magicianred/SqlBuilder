using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace SqlBuilder.UnitTest
{
  [TestClass]
  public class ColumnTests
  {
    [TestMethod]
    public void IsReadOnly()
    {
      new Column(GetType().GetMember(nameof(IsReadOnlyField)).FirstOrDefault()).IsReadOnly.MustBeTrue();
      new Column(GetType().GetMember(nameof(IsReadOnlyProp)).FirstOrDefault()).IsReadOnly.MustBeTrue();

      new Column(GetType().GetMember(nameof(IsEditableField)).FirstOrDefault()).IsReadOnly.MustBeFalse();
      new Column(GetType().GetMember(nameof(IsEditableProp)).FirstOrDefault()).IsReadOnly.MustBeFalse();
    }

    public readonly bool IsReadOnlyField = true;

    public bool IsEditableField = true;

    public bool IsReadOnlyProp { get { return true; } }

    public bool IsEditableProp { get; set; }
  }
}
