using System;
using System.Collections.Generic;

namespace Evsell.Business.SqlServer.Models;

public partial class User
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

    public virtual ICollection<Company> Companies { get; } = new List<Company>();

    public virtual ICollection<ProductComment> ProductComments { get; } = new List<ProductComment>();

    public virtual EnumUserType UserTypeNavigation { get; set; }
}
