using System;
using System.Collections.Generic;

namespace Evsell.Business.SqlServer.Models;

public partial class BasketLog
{
    public int Id { get; set; }

    public int? InvoiceId { get; set; }

    public int ProductId { get; set; }

    public int Qty { get; set; }

    public bool IsInput { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime CreateDate { get; set; }

    public int CreateUserId { get; set; }
}
