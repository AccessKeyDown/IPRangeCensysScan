using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
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

        public async Task<List<CensysAccount>> GetAllCensysAccounts()
        {
            using(var dbcontext = new DataDbContext())
            {
                var listOfAccounts = await dbcontext.CensysAccounts.ToListAsync();
                return listOfAccounts;
            }
        }

        public async Task<List<CensysJsonResult>> GetCensysJsonData()
        {

            using(var dbcontext = new DataDbContext())
            {
                var listOfCensysData = await dbcontext.CensysDatas.Select(s => new CensysJsonResult()
                {
                    IpRangeId = s.IpRangeId,
                    IpRange = s.IpRange.Range,
                    Json = s.Data,
                    ResponseMessage = s.Message,
                    ResponseStatusCode = s.StatusCode
                }).ToListAsync();
                return listOfCensysData;
            }
        }
        public async void AddCensysParcedData(List<CensysParsedData> dataList)
        {
            using(var dbcontext = new DataDbContext())
            {
                await dbcontext.CensysParsedData.AddRangeAsync(dataList);
                await dbcontext.SaveChangesAsync();
            }
        }
    }
}