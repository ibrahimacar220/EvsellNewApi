using Evsell.Business.Common.Response;
using Evsell.Business.SqlServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evsell.Business.SqlServer.Business.Interface
{
    public interface IProductCategoryBusiness:IDisposable
    {
        ResponseDto< List<ProductCategory>> GetList();
        ResponseDto<List<Productcate>> GetListChildren();
    }
}
