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
    public class ProductCommentController : ControllerBase
    {

        public ProductCommentController(IProductCommentBusiness productCommentBusiness)
        {
            _productCommentBusiness = productCommentBusiness;
        }
        public IProductCommentBusiness _productCommentBusiness;

        [HttpPost("Save")]
        public ResponseDto Save([FromBody] int id, int userId, bool isActive, int productId, string comments, string header, int productRate)
        {
            return _productCommentBusiness.Save(id, userId, isActive, productId, comments, header, productRate);
        }

        [HttpDelete("Delete")]
        public ResponseDto Delete(int id)
        {
            return _productCommentBusiness.Delete(id);
        }

        [HttpGet("GetList")]
        public ResponseDto<List<ProductComment>> GetList(bool isActive)
        {
            return _productCommentBusiness.GetList(isActive);
        }

        [HttpGet("Get")]
        public ResponseDto<ProductComment> Get(int id)
        {
            return _productCommentBusiness.Get(id);
        }
    }
}
