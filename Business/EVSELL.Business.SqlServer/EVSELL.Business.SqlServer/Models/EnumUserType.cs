using System;
using System.Collections.Generic;

namespace Evsell.Business.SqlServer.Models;

public partial class EnumUserType
{
    public int Id { get; set; }

    public string Name { get; set; }

    public virtual ICollection<User> Users { get; } = new List<User>();
}
