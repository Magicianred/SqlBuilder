using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SqlBuilder.UnitTest
{
  [TestClass]
  public class OptionsTests
  {
    [TestMethod]
    public void none_renders_null()
    {
      new Options(SqlOptions.None).Sql().MustBeNull();
    }

    [TestMethod]
    public void single_valid_option_renders_sql()
    {
      new Options(SqlOptions.Recompile).Sql().MustBe("option (recompile)");
      new Options(SqlOptions.OptimiseForUnknown).Sql().MustBe("option (optimize for unknown)");
    }

    [TestMethod]
    public void multiple_valid_options_renders_sql()
    {
      new Options(SqlOptions.Recompile | SqlOptions.OptimiseForUnknown).Sql().MustBe("option (recompile,optimize for unknown)");
      new Options(SqlOptions.OptimiseForUnknown | SqlOptions.Recompile).Sql().MustBe("option (recompile,optimize for unknown)");
    }
  }
}