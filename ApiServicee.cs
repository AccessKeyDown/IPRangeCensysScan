using System.Collections.Generic;
using System;
using Refit;
using System.Threading.Tasks;

namespace IPRangeCensysScan
{
    public class ApiServicee
    {
        public async void SearchData(string searchData)
        {
            var api = RestService.For<ICensysApi>("https://censys.io");
            var searchResult = await api.Search(searchData);
            Console.WriteLine(searchResult.Response[0].Status);    
        }
    }

    public interface ICensysApi
    {
        [Post("/api/v1/search")]
        Task<ApiResponse<CensysSearchResult>> Search([Query]string index);
    }

    public class CensysSearchResult
    {
        public string Status { get; set; }
    }

    public class ApiResponse<T>
    {
       public bool IsSuccess {get; set;}
       public string UserMessage {get; set;}
       public string TechnicalMessage {get; set;}
       public int TotalCount {get; set;}
       public List<T> Response {get; set;}
    }
}