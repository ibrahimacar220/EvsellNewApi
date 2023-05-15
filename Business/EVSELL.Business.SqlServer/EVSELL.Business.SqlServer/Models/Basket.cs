using System;
using System.Collections.Generic;

namespace Evsell.Business.SqlServer.Models;

public partial class Basket
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int ProductId { get; set; }

    public int Qty { get; set; }

    public DateTime CreateDate { get; set; }

    public int CreateUserId { get; set; }

    public virtual Product Product { get; set; }
}
