namespace IPRangeCensysScan
{
    public class CensysSearchPost
    {
        public string  Query { get; set; }
        public int  Page { get; set; }
        public string  Fields { get; set; }
        public string  Flatten { get; set; }
    }
}