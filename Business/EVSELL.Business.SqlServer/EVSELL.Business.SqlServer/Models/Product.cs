using System;
using System.Collections.Generic;

namespace Evsell.Business.SqlServer.Models;

public partial class Product
{
    public int Id { get; set; }

    public int CategoryId { get; set; }

    public int CompanyId { get; set; }

    public string Name { get; set; }

    public decimal Price { get; set; }

    public string Image { get; set; }

    public string Description { get; set; }

    public int Stock { get; set; }

    public int Tax { get; set; }

    public bool? IsActive { get; set; }

    public DateTime CreateDate { get; set; }

    public int CreateUserId { get; set; }

    public DateTime? UpdateDate { get; set; }

    public int? UpdateUserId { get; set; }

    public virtual ICollection<Basket> Baskets { get; } = new List<Basket>();

    public virtual ProductCategory Category { get; set; }

    public virtual Company Company { get; set; }

    public virtual ICollection<InvoiceProduct> InvoiceProducts { get; } = new List<InvoiceProduct>();

    public virtual ICollection<ProductComment> ProductComments { get; } = new List<ProductComment>();
}
