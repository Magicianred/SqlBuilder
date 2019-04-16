using restlessmedia.Module.Configuration;
using SqlBuilder;
using System.Data;
using System.Linq;

namespace restlessmedia.Data
{
  public static class SelectExtensions
  {
    public static void WithLicenseId(this Select select, IDbConnection connection, ILicenseSettings licenseSettings)
    {
      select.Where("LicenseId", LicenseHelper.GetLicenseId(connection, licenseSettings));
    }
  }
}