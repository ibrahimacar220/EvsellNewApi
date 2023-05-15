using Evsell.Business.SqlServer.Bo.Basket;
using Evsell.Business.SqlServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evsell.Business.SqlServer.Bo.User
{
    public class UserGetListBo
    {
        public int Id { get; set; }

        public int UserType { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string FirtsName { get; set; }

        public string LastName { get; set; }

        public bool? IsActive { get; set; }

        public DateTime CreateDate { get; set; }

        public int CreateUserId { get; set; }

        public DateTime? UpdateDate { get; set; }

        public int? UpdateUserId { get; set; }

        public List<BasketBo> baskets { get; set; }
    }
}
