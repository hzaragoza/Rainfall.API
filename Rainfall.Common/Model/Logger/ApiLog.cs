using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rainfall.Common.Model.Logger
{
    public class ApiLog
    {
        public string strTransactionId { get; set; } = null;
        public string strEndpoint { get; set; } = null;
        public string strMessage { get; set; } = null;
    }
}
