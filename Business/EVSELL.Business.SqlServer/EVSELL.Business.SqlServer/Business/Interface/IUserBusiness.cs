using Evsell.Business.Common.Response;
using Evsell.Business.SqlServer.Bo.User;
using Evsell.Business.SqlServer.Dtos;
using Evsell.Business.SqlServer.Models;

namespace Evsell.Business.SqlServer.Business.Interface
{
    public interface IUserBusiness : IDisposable
    {
        ResponseDto Save(UserBo userBo);
        ResponseDto Delete(UserDelBo userDelBo);
        ResponseDto<UserBo> Get(UserGetBo userGetBo);
        ResponseDto<List<UserGetListBo>> GetList(UserGetListCriteriaBo userGetListCriteriaBo);

        ResponseDto<UserBo> GetByUserName(string userName);      
    }
}
