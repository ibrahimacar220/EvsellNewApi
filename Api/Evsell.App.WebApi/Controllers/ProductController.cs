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
    public class ProductController : ControllerBase
    {

        public ProductController(IProductBusiness productBusiness)
        {
            _productBusiness = productBusiness;
        }
        public IProductBusiness _productBusiness;

        [HttpPost("Save")]
        public ResponseDto Save(int id, int companyId, bool isActive, string name, string image, int price, string description, int categoryId, int stock)
        {
            return _productBusiness.Save(id, companyId, isActive, name, image, price, description, categoryId, stock);
        }

        [HttpPost("ProductStockChange")]
        public ResponseDto ProductStockChange(int id, int change, bool isInput)
        {
            return _productBusiness.ProductStockChange(id, change, isInput);
        }

        [HttpDelete("Delete")]
        public void Delete(int id)
        {
            _productBusiness.Delete(id);
        }

        [HttpGet("GetList")]
        public ResponseDto<List<Product>> GetList(bool? isActive)
        {
            return _productBusiness.GetList(isActive);
        }

        [HttpGet("GetListByCategoryId")]
        public ResponseDto<List<Product>> GetListByCategoryId(int id)
        {
            return _productBusiness.GetListByCategoryId(id);
        }

        [HttpGet("GetListByCompanyId")]
        public ResponseDto<List<Product>> GetListByCompanyId(int id)
        {
            return _productBusiness.GetListByCompanyId(id);
        }

        [HttpGet("StockControl")]
        public ResponseDto StockControl(int id)
        {
            return _productBusiness.StockControl(id);
        }
    }
}
