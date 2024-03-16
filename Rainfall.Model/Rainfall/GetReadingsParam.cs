using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rainfall.Model.Rainfall
{
    public class GetReadingsParam
    {
        /// <summary>
        /// The id of the reading station
        /// </summary>
        public string stationId { get; set; }

        /// <summary>
        /// The number of readings to return
        /// </summary>
        public int count { get; set; }

    }
}
