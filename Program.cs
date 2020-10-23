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
            

            Console.WriteLine(ipList[0]);
        }
    }
}
