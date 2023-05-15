using System;
using System.Collections.Generic;

namespace Evsell.Business.SqlServer.Models;

public partial class Company
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string Name { get; set; }

    public bool? IsActive { get; set; }

    public DateTime CreateDate { get; set; }

    public int CreateUserId { get; set; }

    public DateTime? UpdateDate { get; set; }

    public int? UpdateUserId { get; set; }

    public virtual ICollection<Product> Products { get; } = new List<Product>();

    public virtual User User { get; set; }
}
