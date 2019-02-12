using System;

namespace SqlBuilder
{
  [Flags]
  public enum SqlOptions
  {
    None = 0,
    Recompile = 1,
    SetArithabort = 2,

    /// <summary>
    /// Adds OPTION (OPTIMIZE FOR UNKNOWN).  We could improve this by adding the hint at property/field level but need to change <see cref="Models.ModelDefinition"/>.
    /// </summary>
    OptimiseForUnknown = 4,
  }
}