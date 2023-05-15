using AutoMapper;
using Evsell.App.WebApi.Dto.User;
using Evsell.Business.Common.Response;
using Evsell.Business.SqlServer.Bo.User;

namespace Evsell.App.WebApi
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            //user
            CreateMap<UserDto, UserBo>().ReverseMap();

            CreateMap<ResponseDto<UserDto>, ResponseDto<UserBo>>().ReverseMap();

            CreateMap<UserGetBo, UserGetDto>().ReverseMap();

            CreateMap<UserDelDto, UserDelBo>().ReverseMap();

            CreateMap<UserGetListCriteriaDto, UserGetListCriteriaBo>().ReverseMap();

            CreateMap<UserGetListDto, UserGetListBo>().ReverseMap();

            CreateMap<ResponseDto<List<UserGetListDto>>, ResponseDto<List<UserGetListBo>>>().ReverseMap();

            CreateMap<ResponseDto<UserGetListDto>, ResponseDto<UserGetListBo>>().ReverseMap();
            //

        }
    }
}