using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Rainfall.Model.Rainfall.Response
{
    public class rainfallReadingResponse
    {
        [JsonProperty("items")]
        public List<rainfallReading> readings { get; set; }
    }

    public class rainfallReading
    {
        [JsonProperty("dateTime")]
        public DateTime dateMeasured { get; set; }

        [JsonProperty("value")]
        public decimal amountMeasured { get; set; }
    }
}
