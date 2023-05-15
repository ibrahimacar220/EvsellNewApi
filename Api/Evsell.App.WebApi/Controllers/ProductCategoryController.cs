using Evsell.Business.Common.Response;
using Evsell.Business.SqlServer.Business;
using Evsell.Business.SqlServer.Business.Interface;
using Evsell.Business.SqlServer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Evsell.App.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductCategoryController : ControllerBase
    {
        public ProductCategoryController(IProductCategoryBusiness productCategoryBusiness)
        {
            _productCategoryBusiness = productCategoryBusiness;
        }
        public IProductCategoryBusiness _productCategoryBusiness;

        [HttpGet("GetListChlidren")]
        public ResponseDto<List<Productcate>> GetListChlidren()
        {
            return _productCategoryBusiness.GetListChildren();
        }

        [HttpGet("GetList")]
        public ResponseDto<List<ProductCategory>> GetList()
        {
            return _productCategoryBusiness.GetList();
        }
    }
}
