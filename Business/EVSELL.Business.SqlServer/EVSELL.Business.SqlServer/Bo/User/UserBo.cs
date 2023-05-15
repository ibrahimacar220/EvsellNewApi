using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evsell.Business.SqlServer.Bo.User
{
    public class UserBo
    {
        public int Id { get; set; }

        public EnumUserTypes UserType { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string FirtsName { get; set; }

        public string LastName { get; set; }
    }
}
