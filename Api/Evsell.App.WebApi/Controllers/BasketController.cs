using Evsell.Business.Common.Response;
using Evsell.Business.SqlServer.Bo.Basket;
using Evsell.Business.SqlServer.Business.Interface;
using Evsell.Business.SqlServer.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Evsell.App.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BasketController : ControllerBase
    {
        public BasketController(IBasketBusiness basketBusiness)
        {
            _basketBusiness = basketBusiness;
        }
        public IBasketBusiness _basketBusiness;

        [HttpPost("Save")]
        public ResponseDto Save(int buyerId, [FromBody] List<InvoiceProductDto> productProperties)
        {
            return _basketBusiness.Save(buyerId, productProperties);
        }

        [HttpGet("GetList")]
        public ResponseDto<List<BasketBo>> GetBaskets(int userId)
        {
            return _basketBusiness.GetBaskets(userId);
        }
        [HttpDelete("Delete")]
        public ResponseDto Delete(int userId, int qty, int productId)
        {
            return _basketBusiness.Delete(userId, qty, productId);
        }

    }
}
