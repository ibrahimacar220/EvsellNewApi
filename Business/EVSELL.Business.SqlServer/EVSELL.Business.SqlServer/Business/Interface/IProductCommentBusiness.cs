using Evsell.Business.Common.Response;
using Evsell.Business.SqlServer.Models;

namespace Evsell.Business.SqlServer.Business.Interface
{
    public interface IProductCommentBusiness : IDisposable
    {
        ResponseDto Save(int id, int userId, bool isActive, int productId, string comments, string header, int productRate);
        ResponseDto Delete(int id);
        ResponseDto<ProductComment> Get(int id);
        ResponseDto<List<ProductComment>> GetList(bool? isActive);
    }
}