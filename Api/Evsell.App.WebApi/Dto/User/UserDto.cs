using Evsell.Business.SqlServer;

namespace Evsell.App.WebApi.Dto.User
{
    public class UserDto
    {
        public int Id { get; set; }

        public EnumCostumerUserTypes UserType { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string FirtsName { get; set; }

        public string LastName { get; set; }
    }
}
