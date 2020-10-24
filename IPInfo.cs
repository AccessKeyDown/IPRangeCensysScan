using System.IO;
using System.Linq;
using System.Collections.Generic;
using System;

namespace IPRangeCensysScan
{
    public class IPinfo
    {
        private readonly string filePath;
        
        public IPinfo(string path)
        {
            filePath = path;
        }

        public List<string> GetIpAddresses()
        {
            string[] fileInfo = File.ReadAllLines(filePath);
            var listOfData = fileInfo.OfType<string>().ToList();
            return listOfData.Where(s => !s.Contains('f')).ToList();
        }
    }
}