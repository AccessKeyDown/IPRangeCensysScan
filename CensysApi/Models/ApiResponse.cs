using System.Collections.Generic;

namespace IPRangeCensysScan
{
    public class ApiResponse<T>
    {
       public bool IsSuccess {get; set;}
       public string UserMessage {get; set;}
       public string TechnicalMessage {get; set;}
       public int TotalCount {get; set;}
       public List<T> Response {get; set;}
    }
}