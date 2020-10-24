using System.Collections.Generic;
using System;
using Refit;
using System.Threading.Tasks;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace IPRangeCensysScan
{
    public class CensysApiServicee
    {
        private readonly ICensysApi _api;
        private readonly AuthenticationHeaderValue _authorizationHeader;
        private const string _baseUrl = "https://censys.io/api/v1";
        public CensysApiServicee(string _API_ID, string _Secret)
        {
            var API_ID = _API_ID;
            var Secret = _Secret;
            var authHeader = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(API_ID + ":" + Secret));
            var baseAddress = "https://censys.io/api/v1";
            _authorizationHeader = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", authHeader);
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

        public async Task<string> GetJsonViewData(IndexEnum index, string id)
        {
            using(var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = _authorizationHeader;
                var result = await client.GetAsync($"{_baseUrl}/view/{index}/{id}");
                if( ((int)result.StatusCode >= 200) && ((int)result.StatusCode <= 299))
                {
                    var jsonResult = await result.Content.ReadAsStringAsync();

                    return jsonResult;
                }

                return "";
            }
        }

        public async Task<CensysJsonResult> GetJsonSearchData(IndexEnum index, CensysSearchPost body)
        {
            using(var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = _authorizationHeader;

                var json = JsonConvert.SerializeObject(body);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                var url = $"{_baseUrl}/search/{index}";

                var response = await client.PostAsync(url, data);

                var message =  $"Request Message Information:- \n\n{response?.RequestMessage}\n\n\n";
                    message += $"Response Message Header \n\n {response?.Content?.Headers}\n";

                if( ((int)response.StatusCode >= 200) && ((int)response.StatusCode <= 299))
                {
                    var jsonResult = await response.Content.ReadAsStringAsync();
                    

                    return new CensysJsonResult
                    {
                        IpRange = body.query,
                        Json = jsonResult,
                        ResponseMessage = message,
                        ResponseStatusCode = response.StatusCode.ToString()
                    };
                }

                return new CensysJsonResult
                    {
                        IpRange = body.query,
                        ResponseMessage = message,
                        ResponseStatusCode = response.StatusCode.ToString()
                    };
            }
        }

        public async Task<string> GetAccountData()
        {
            using(var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = _authorizationHeader;
                var result = await client.GetAsync($"{_baseUrl}/account");
                if( ((int)result.StatusCode >= 200) && ((int)result.StatusCode <= 299))
                {
                    var jsonResult = await result.Content.ReadAsStringAsync();

                    return jsonResult;
                }

                return "";
            }
        }
    }
}