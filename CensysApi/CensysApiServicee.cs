using System.Collections.Generic;
using System;
using Refit;
using System.Threading.Tasks;

namespace IPRangeCensysScan
{
    public class CensysApiServicee
    {
        public ICensysApi _api = RestService.For<ICensysApi>("https://censys.io");

        public async void SearchData(string searchData)
        {
            var searchResult = await _api.Search(searchData);
            Console.WriteLine(searchResult.Response[0].Status);    
        }
    }
}