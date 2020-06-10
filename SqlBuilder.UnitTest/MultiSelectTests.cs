using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SqlBuilder.UnitTest
{
  [TestClass]
  public class MultiSelectTests
  {
    [TestMethod]
    public void outputs_sql()
    {
      // set-up
      MultiSelect multiSelect = new MultiSelect();
      multiSelect.Add(new Select<View>());
      multiSelect.Add(new Select<View>());

      // call & assert
      Assert.AreEqual("select * from dbo.View\r\nselect * from dbo.View", multiSelect.Sql());
    }

    private class View { }
  }
}