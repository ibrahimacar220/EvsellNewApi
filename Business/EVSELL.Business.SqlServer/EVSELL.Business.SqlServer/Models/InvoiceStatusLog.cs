using System;
using System.Collections.Generic;

namespace Evsell.Business.SqlServer.Models;

public partial class InvoiceStatusLog
{
    public int Id { get; set; }

    public int InvoiceId { get; set; }

    public int Status { get; set; }

    public DateTime CreateDate { get; set; }

    public int CreateUserId { get; set; }

    public virtual Invoice Invoice { get; set; }
}
