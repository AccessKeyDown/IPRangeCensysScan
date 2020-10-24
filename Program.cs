using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace IPRangeCensysScan
{
    class Program
    {
        static void Main(string[] args)
        {
            IPinfo ipInfo = new IPinfo("files/suip.ipranges");
            var ipRangeList = ipInfo.GetIpAddresses();
            CensysApiServicee service = new CensysApiServicee();
            var tasks = new List<Task>();
            var tempIpRangeList = new List<string> ()
            {
                "[85.132.78.0 TO 85.132.78.255]",
                "[164.215.96.0 TO 164.215.96.255]",
                "[94.20.18.0 TO 94.20.18.223]"
            };

            CensysDataRepository repo = new CensysDataRepository();
            repo.InsertRangeData(new IpRange()
            {
                Range = tempIpRangeList[0]
            });
            var a =  service.GetJsonSearchData(IndexEnum.ipv4, 
                                              new CensysSearchPost()
                                              {
                                                  query = tempIpRangeList[0]
                                              }).Result;

             
            // foreach(string ipRange in tempIpRangeList)
            // {
            //     Task task = Task.Run( async () => await service.GetJsonViewData(IndexEnum.ipv4, ipRange));
            //     tasks.Add(task);
            // }

            // Task.WaitAll(tasks.ToArray());
            
            System.Console.WriteLine("Finish !");
        }
    }
}
