using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evsell.Business.SqlServer.Bo.Basket
{
    public class BasketBo
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Qty { get; set; }
        public string Image { get; set; }
        public decimal Price { get; set; }
    }
}
