using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rainfall.Common.Model.Response
{
    /// <summary>
    /// Status Codes: 400, 404 and 500
    /// </summary>
    public class error
    {
        public string message { get; set; }
        public List<errorDetail> detail { get; set; } = new List<errorDetail>();
    }

    public class errorDetail
    {
        public string propertyName { get; set; }
        public string message { get; set; }
    }
}
