using System.Collections.Generic;
using System;
using Refit;
using System.Threading.Tasks;
using System.Text;

namespace IPRangeCensysScan
{
    public class CensysApiServicee
    {
        private readonly ICensysApi _api;

        public CensysApiServicee()
        {
            var API_ID = "e3d9803a-4a6f-4596-b79b-be59de79f5d7";
            var Secret = "1e0WbVIM2qTQSzhmOUCIQ61xTLYXfQwW";
            var authHeader = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(API_ID + ":" + Secret));
            var baseAddress = "https://censys.io/api/v1";

            var refitSettings = new RefitSettings()
            {
                AuthorizationHeaderValueGetter = () => Task.FromResult(authHeader)
            };

            _api = RestService.For<ICensysApi>(baseAddress, refitSettings);
        }

        public async Task<List<CensysSearchResult>> SearchData(string index, CensysSearchPost body)
        {
            var searchResult = await _api.Search(index, body);
            Console.WriteLine(searchResult.Response[0].Status);    
            return searchResult.Response;
        }

        public async Task<CensysViewResult> ViewData()
        {
            var viewResult = await _api.View("websites", "google.com");
            Console.WriteLine(viewResult.Raw);    
            return viewResult;
        }
    }
}