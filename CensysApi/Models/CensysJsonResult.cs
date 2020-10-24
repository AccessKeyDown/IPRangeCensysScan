using System.Collections.Generic;

namespace IPRangeCensysScan
{
    public class CensysJsonResult
    {
        public string IpRange {get; set;}
        public string Json { get; set; }
        public string ResponseStatusCode { get; set; }
        public string ResponseMessage { get; set; }
    }
}