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
            // CensysApiServicee service = new CensysApiServicee("8fb4e440-4b6f-4fec-82d3-dfd675214a9e", "fVw3G5h4vw8E5Tu86a2KrvNLcbCyLaTk");
            // var data = service.GetAccountData().Result;
            IPinfo ipInfo = new IPinfo("Files/suip.ipranges");
            var ipRangeList = ipInfo.GetIpAddresses();
            CensysDataRepository repo = new CensysDataRepository();
            
            var tasks = new List<Task>();

            int totalCount = ipRangeList.Count;
            System.Console.WriteLine(totalCount);

            var accounts = repo.GetAllCensysAccounts().Result;
            var accId = 0;

            for(int j = 0; j < ipRangeList.Count; j++)
            {
                var ipRange = ipRangeList[j];
                var account = accounts[accId];

                CensysApiServicee service = new CensysApiServicee(account.API_ID, account.Secret);

                // Task task = Task.Run( async () =>
                // {
                //    var data =  await service.GetJsonSearchData(IndexEnum.ipv4, new CensysSearchPost()
                //                                                   {
                //                                                       query = ipRange
                //                                                   });
                //     requestedData.Add(data);
                //     System.Console.WriteLine("Passed-" + requestedData.Count);
                // });

                // tasks.Add(task);

                var data = service.GetJsonSearchData(IndexEnum.ipv4, new CensysSearchPost()
                                                               {
                                                                   query = ipRange
                                                               }).Result;

                repo.InsertRangeWithJsonData(new CensysData()
                {
                    Data = data.Json,
                    StatusCode = data.ResponseStatusCode,
                    Message = data.ResponseMessage,
                    IpRange = new IpRange()
                    {
                        Range = data.IpRange
                    }
                });

                System.Console.WriteLine("Passed-" + j);

                if(j % 120 == 0 && j != 0)
                {
                    accId++;
                }
            }

            // Task.WaitAll(tasks.ToArray());

            
            // repo.BulkInsertRangeWithJsonData(requestedData.Select(s => new CensysData()
            // {
            //     Data = s.Json,
            //     StatusCode = s.ResponseStatusCode,
            //     Message = s.ResponseMessage,
            //     IpRange = new IpRange()
            //     {
            //         Range = s.IpRange
            //     }
            // }).ToList());

            System.Console.WriteLine("Done!");
        }
    }
}
