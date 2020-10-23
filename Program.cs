using System;
using System.IO;

namespace IPRangeCensysScan
{
    class Program
    {
        static void Main(string[] args)
        {
            IPinfo ipInfo = new IPinfo("files/suip.ipranges");
            var ipList = ipInfo.GetIpAddresses();
            CensysApiServicee service = new CensysApiServicee();
            var a = service.ViewData().Result;
            var result = service.SearchData("ipv4", new CensysSearchPost()
            {
                Query = ipList[0],
                Page = 1
            }).Result;
            Console.WriteLine(result[0].Status);
            System.Console.WriteLine("Finish !");
        }
    }
}
