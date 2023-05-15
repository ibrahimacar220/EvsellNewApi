using Evsell.Business.Common.Response;
using Evsell.Business.SqlServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evsell.Business.SqlServer.Business.Interface
{
    public interface ILogHttpBusiness : IDisposable
    {
        ResponseDto Save(string RequestRaw, DateTime RequestDateTime, string ResponseRaw, DateTime ResponseDateTime);
    }
}
