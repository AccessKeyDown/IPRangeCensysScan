using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace IPRangeCensysScan
{
    class Program
    {
        public static List<CensysJsonResult> requestedData = new List<CensysJsonResult>();
        static void Main(string[] args)
        {
            IPinfo ipInfo = new IPinfo("files/suip.ipranges");
            var ipRangeList = ipInfo.GetIpAddresses();
            CensysApiServicee service = new CensysApiServicee();
            var tasks = new List<Task>();

            int totalCount = ipRangeList.Count;
            System.Console.WriteLine(totalCount);

            foreach(string ipRange in ipRangeList.Take(200))
            {
                Task task = Task.Run( async () =>
                {
                   var data =  await service.GetJsonSearchData(IndexEnum.ipv4, new CensysSearchPost()
                                                                  {
                                                                      query = ipRange
                                                                  });
                    requestedData.Add(data);
                    System.Console.WriteLine("Passed-" + requestedData.Count);
                });
                tasks.Add(task);
            }

            Task.WaitAll(tasks.ToArray());

            CensysDataRepository repo = new CensysDataRepository();
            
            repo.BulkInsertRangeWithJsonData(requestedData.Select(s => new CensysData()
            {
                Data = s.Json,
                StatusCode = s.ResponseStatusCode,
                Message = s.ResponseMessage,
                IpRange = new IpRange()
                {
                    Range = s.IpRange
                }
            }).ToList());

            System.Console.WriteLine("Done!");
        }
    }
}
