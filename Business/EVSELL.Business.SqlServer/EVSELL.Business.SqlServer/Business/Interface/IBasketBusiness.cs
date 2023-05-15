using Evsell.Business.Common.Response;
using Evsell.Business.SqlServer.Bo.Basket;
using Evsell.Business.SqlServer.Dtos;
using Evsell.Business.SqlServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evsell.Business.SqlServer.Business.Interface
{
    public interface IBasketBusiness
    {
        public ResponseDto Save(int buyyerId, List<InvoiceProductDto> productProperties);
        public ResponseDto<List<BasketBo>> GetBaskets(int userId);
        public ResponseDto Delete(int userId, int qty, int productId);
    }

}
