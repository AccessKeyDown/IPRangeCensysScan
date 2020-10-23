using Refit;
using System.Threading.Tasks;

namespace IPRangeCensysScan
{
    public interface ICensysApi
    {
        [Post("/api/v1/search")]
        Task<ApiResponse<CensysSearchResult>> Search([Query]string index);
    }
}