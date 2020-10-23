using Refit;
using System.Threading.Tasks;

namespace IPRangeCensysScan
{
    [Headers("Authorization: Basic")]
    public interface ICensysApi
    {
        [Post("/search")]
        Task<ApiResponse<CensysSearchResult>> Search([Query]string index, [Body]CensysSearchPost body);

        [Get("/view/{index}/{id}")]
        Task<CensysViewResult> View(string index, string id);
    }
}