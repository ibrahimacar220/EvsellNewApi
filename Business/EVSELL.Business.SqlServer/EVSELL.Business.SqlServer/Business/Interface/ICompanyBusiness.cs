using Evsell.Business.Common.Response;
using Evsell.Business.SqlServer.Models;

namespace Evsell.Business.SqlServer.Business.Interface
{
    public interface ICompanyBusiness : IDisposable
    {
        ResponseDto Save(int id, int userId, string name, bool IsActive);
        ResponseDto Delete(int id);
        ResponseDto<Company> Get(int id);
        ResponseDto<List<Company>> GetList(bool? isActive);
    }
}