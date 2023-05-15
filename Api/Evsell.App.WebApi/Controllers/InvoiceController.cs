using Evsell.Business.Common.Response;
using Evsell.Business.SqlServer;
using Evsell.Business.SqlServer.Business.Interface;
using Evsell.Business.SqlServer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Evsell.App.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class InvoiceController : ControllerBase
    {

        public InvoiceController(IInvoiceBusiness invoiceBusiness)
        {
            _invoiceBusiness = invoiceBusiness;
        }
        public IInvoiceBusiness _invoiceBusiness;

        [HttpPost("Save")]

        public ResponseDto Save(int buyerId)
        {
            return _invoiceBusiness.Save(buyerId);
        }

        [HttpPost("Cancel")]

        public ResponseDto Cancel(int id)
        {
            return _invoiceBusiness.Cancel(id);
        }

        [HttpPost("UpdateStatus")]

        public ResponseDto UpdateStatus(int Id, EnumInvoiceStatusTypes enumInvoiceStatus)
        {
            return _invoiceBusiness.UpdateStatus(Id, enumInvoiceStatus);
        }

        [HttpGet("GetList")]

        public ResponseDto<List<Invoice>> GetList(bool isCancalled)
        {
            return _invoiceBusiness.GetList(isCancalled);
        }

        [HttpGet("IsCanceled")]

        public ResponseDto<bool> IsCanceled(int id)
        {
            return _invoiceBusiness.IsCanceled(id);
        }
    }
}
