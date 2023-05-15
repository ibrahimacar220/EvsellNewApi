
using AutoMapper;
using Evsell.App.WebApi.Dto.User;
using Evsell.Business.Common.Response;
using Evsell.Business.SqlServer.Bo.User;
using Evsell.Business.SqlServer.Business.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Evsell.App.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class UserController : ControllerBase
    {
        private readonly IMapper _mapper;
        public IUserBusiness _userBusiness;

        public UserController(IMapper mapper, IUserBusiness userBusiness)
        {
            _mapper = mapper;
            _userBusiness = userBusiness;
        }

        [HttpPost("Save")]
        public ResponseDto Save(UserDto dto)
        {
            UserBo bo = _mapper.Map<UserBo>(dto);

            return _userBusiness.Save(bo);
        }

        [HttpDelete("delete")]
        public ResponseDto Delete(UserDelDto dto)
        {
            UserDelBo bo = _mapper.Map<UserDelBo>(dto);

            return _userBusiness.Delete(bo);
        }

        [HttpPost("Get")]

        public ResponseDto<UserDto> Get(UserGetDto dto)
        {
            UserGetBo bo = _mapper.Map<UserGetBo>(dto);

            return _mapper.Map<ResponseDto<UserDto>>(_userBusiness.Get(bo));

        }

        [HttpPost("GetList")]
        public ResponseDto<List<UserGetListDto>> GetList(UserGetListCriteriaDto dto)
        {

            UserGetListCriteriaBo bo = _mapper.Map<UserGetListCriteriaBo>(dto);

            return _mapper.Map<ResponseDto<List<UserGetListDto>>>(_userBusiness.GetList(bo));

        }

        [HttpGet]
        [Route("GetByUserName")]
        public ResponseDto<UserDto> GetByUserName(string userName)
        {
            return _mapper.Map<ResponseDto<UserDto>>(_userBusiness.GetByUserName(userName));
        }
    }
}
