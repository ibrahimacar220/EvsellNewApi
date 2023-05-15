using Evsell.Business.Common.Response;
using Evsell.Business.SqlServer.Models;

namespace Evsell.Business.SqlServer.Business.Interface
{
    public interface IProductBusiness : IDisposable
    {
        ResponseDto Save(int id, int companyId, bool isActive, string name, string image, int price, string description, int category, int Stock);
        ResponseDto Delete(int id);
        ResponseDto<Product> Get(int id);
        ResponseDto<List<Product>> GetList(bool? isActive);

        ResponseDto ProductStockChange(int id, int change, bool isInput);
        ResponseDto StockControl(int id);
        ResponseDto<List<Product>> GetListByCompanyId(int id);
        ResponseDto<List<Product>> GetListByCategoryId(int id);

    }
}
