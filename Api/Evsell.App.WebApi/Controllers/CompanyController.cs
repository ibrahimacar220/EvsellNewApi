using Evsell.Business.Common.Response;
using Evsell.Business.SqlServer.Business.Interface;
using Evsell.Business.SqlServer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Evsell.App.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CompanyController : ControllerBase
    {

        public CompanyController(ICompanyBusiness companyBusiness)
        {
            _companyBusiness = companyBusiness;
        }
        public ICompanyBusiness _companyBusiness;

        [HttpPost("Save")]
        public ResponseDto Save([FromBody] int id, int userId, string name, bool isActive)
        {
            return _companyBusiness.Save(id, userId, name, isActive);
        }

        [HttpDelete("Delete")]
        public ResponseDto Delete(int id)
        {
            return _companyBusiness.Delete(id);
        }

        [HttpGet("GetList")]
        public ResponseDto<List<Company>> GetList(bool isActive)
        {
            return _companyBusiness.GetList(isActive);
        }

        [HttpGet("Get")]
        public ResponseDto<Company> Get(int id)
        {
            return _companyBusiness.Get(id);
        }

    }
}
