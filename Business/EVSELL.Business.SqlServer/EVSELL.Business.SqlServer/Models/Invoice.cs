using System;
using System.Collections.Generic;

namespace Evsell.Business.SqlServer.Models;

public partial class Invoice
{
    public int Id { get; set; }

    public int BuyerUserId { get; set; }

    public int SellerCompanyId { get; set; }

    public DateTime TransactionDateTime { get; set; }

    public decimal Total { get; set; }

    public decimal VatTotal { get; set; }

    public decimal GrandTotal { get; set; }

    public bool IsCancelled { get; set; }

    public int StatusId { get; set; }

    public DateTime CreateDate { get; set; }

    public int CreateUserId { get; set; }

    public DateTime? CancelDate { get; set; }

    public virtual ICollection<InvoiceProduct> InvoiceProducts { get; } = new List<InvoiceProduct>();

    public virtual ICollection<InvoiceStatusLog> InvoiceStatusLogs { get; } = new List<InvoiceStatusLog>();
}
