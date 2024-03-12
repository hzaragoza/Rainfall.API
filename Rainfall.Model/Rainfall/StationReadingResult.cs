using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rainfall.Model.Rainfall
{
    public class StationReadingResult
    {
        [JsonProperty("@context")]
        public string strContext { get; set; }

        [JsonProperty("meta")]
        public StationReadingMetaResult meta { get; set; }

        [JsonProperty("items")]
        public List<StationReadingItemResult> items { get; set; }
    }

    public class StationReadingMetaResult
    {
        [JsonProperty("publisher")]
        public string strPublisher { get; set; }

        [JsonProperty("licence")]
        public string strLicence { get; set; }

        [JsonProperty("documentation")]
        public string strDocumentation { get; set; }

        [JsonProperty("version")]
        public string strVersion { get; set; }

        [JsonProperty("comment")]
        public string strComment { get; set; }

        [JsonProperty("hasFormat")]
        public List<string> hasFormat { get; set; }

        [JsonProperty("limit")]
        public int intLimit { get; set; }
    }

    public class StationReadingItemResult
    {
        [JsonProperty("@id")]
        public string strId { get; set; }

        [JsonProperty("dateTime")]
        public DateTime dtmDateTime { get; set; }

        [JsonProperty("measure")]
        public string strMeasure { get; set; }

        [JsonProperty("value")]
        public double dblValue { get; set; }
    }
}
