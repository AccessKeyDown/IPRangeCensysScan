using System;
using System.Collections.Generic;
using System.IO;
using System.Json;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IPRangeCensysScan
{
    class Program
    {
        public static List<CensysJsonResult> requestedData = new List<CensysJsonResult>();

        static void AccountInfo()
        {
            CensysDataRepository repo = new CensysDataRepository();
            var accounts = repo.GetAllCensysAccounts().Result;
            foreach(var account in accounts)
            {
               CensysApiServicee service = new CensysApiServicee(account.API_ID, account.Secret);
               var data = service.GetAccountData().Result;
               System.Console.WriteLine(data);
            }
        }

        static void InsertDataToDb()
        {
            IPinfo ipInfo = new IPinfo("Files/suip.ipranges");
            var ipRangeList = ipInfo.GetIpAddresses();
            CensysDataRepository repo = new CensysDataRepository();
            
            // var tasks = new List<Task>();

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
                                                                   query = $"ip:{ipRange}"
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
                
                var prcent = ((j * 100) / totalCount);
                System.Console.WriteLine("Passed - " + prcent + j);

                if(j % 120 == 0 && j != 0 || data.ResponseStatusCode == "TooManyRequests")
                {
                    System.Console.WriteLine("Account Changed");
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
        }

        static List<CensysParsedData> ParseData()
        {
            List<CensysParsedData> listOfParsedData = new List<CensysParsedData>();
            
            CensysDataRepository repo = new CensysDataRepository();
            var censysData = repo.GetCensysJsonData().Result;
            
            foreach(var item in censysData)
            {
                JsonValue json = JsonValue.Parse(item.Json);
                dynamic data = JObject.Parse(item.Json);
                
                if (data.results != null)
                {
                    // foreach loop test
                    foreach (var result in data.results)
                    {
                        var protocols = JArray.Parse(result.protocols.ToString());
                        List<string> protList = new List<string>();
                        foreach(var prot in protocols)
                        {
                            protList.Add(prot.ToString());
                        }
                        var parcedData = new CensysParsedData()
                        {
                            IpRangesId = item.IpRangeId,
                            Ip = result.ip.ToString(),
                            Protocols = string.Join(',', protList),
                            Country = result["location.country"] == null ? string.Empty : result["location.country"] .ToString(),
                            Province = result["location.province"] == null ? string.Empty : result["location.province"].ToString(),
                            Timezone = result["location.timezone"] == null ? string.Empty : result["location.timezone"].ToString(),
                            Longitude = result["location.longitude"] == null ? string.Empty : result["location.longitude"].ToString(),
                            Latitude = result["location.latitude"] == null ? string.Empty : result["location.latitude"].ToString()
                        };
                        listOfParsedData.Add(parcedData);
                    }
                }

                System.Console.WriteLine("Parsed-" + item.IpRangeId);
            }

            return listOfParsedData;
        } 

        static void Main(string[] args)
        {
            //AccountInfo();

            //Get IpRanges send request to Censys insert json data into table
            InsertDataToDb();

            //Parse json data insert to the table
            // var parsedData = ParseData();
            // CensysDataRepository repo = new CensysDataRepository();
            // repo.AddCensysParcedData(parsedData);

            System.Console.WriteLine("Done!");
        }
    }
}
