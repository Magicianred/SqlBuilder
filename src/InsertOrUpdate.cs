using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlBuilder
{
  public class InsertOrUpdate<TDataModel> : DML<TDataModel>
  {
    public InsertOrUpdate(TDataModel dataModel)
    {
      _dataModel = dataModel;
    }

    public override string Sql()
    {
      throw new NotImplementedException();
    }

    private readonly TDataModel _dataModel;

  }
}
