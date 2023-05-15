using Evsell.Business.Common.Response;
using Evsell.Business.SqlServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evsell.Business.SqlServer.Business.Interface
{
    public interface IInvoiceBusiness : IDisposable
    {
        ResponseDto Save(int buyerId);
        ResponseDto Cancel(int id);
        ResponseDto<List<Invoice>> GetList(bool? isCancelled);

        ResponseDto UpdateStatus(int id, EnumInvoiceStatusTypes enumInvoiceStatus);
        ResponseDto<bool> IsCanceled(int id);
    }
}