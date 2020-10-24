using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace IPRangeCensysScan
{
    public class CensysDataRepository 
    {
        public async void InsertRangeData(IpRange range)
        {
             using (var dbContext = new DataDbContext())
             {
                 //Ensure database is created
                 dbContext.Database.EnsureCreated();
                 await dbContext.IpRanges.AddAsync(range);
                 await dbContext.SaveChangesAsync();
             }
        }

        public async void InsertRangeWithJsonData(CensysData censysData)
        {
             using (var dbContext = new DataDbContext())
             {
                 //Ensure database is created
                 dbContext.Database.EnsureCreated();
                 await dbContext.CensysDatas.AddAsync(censysData);
                 await dbContext.SaveChangesAsync();
             }
        }

        public async void BulkInsertRangeData(List<IpRange> rangeList)
        {
             using (var dbContext = new DataDbContext())
             {
                 //Ensure database is created
                 dbContext.Database.EnsureCreated();
                 await dbContext.IpRanges.AddRangeAsync(rangeList);
                 await dbContext.SaveChangesAsync();
             }
        }

         public async void BulkInsertRangeWithJsonData(List<CensysData> censysDataList)
        {
             using (var dbContext = new DataDbContext())
             {
                 //Ensure database is created
                 dbContext.Database.EnsureCreated();
                 await dbContext.CensysDatas.AddRangeAsync(censysDataList);
                 await dbContext.SaveChangesAsync();
             }
        }
    }
}