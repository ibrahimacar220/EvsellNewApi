using System;
using System.Collections.Generic;

namespace Evsell.Business.SqlServer.Models;

public partial class ProductCategory
{
    public int Id { get; set; }

    public string Name { get; set; }

    public int? ParentId { get; set; }

    public virtual ICollection<Product> Products { get; } = new List<Product>();
}
