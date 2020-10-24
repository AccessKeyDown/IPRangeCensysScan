using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace IPRangeCensysScan
{
    public class DataDbContext : DbContext
    {
        private readonly string _connectionString = $"Filename=Data{Path.DirectorySeparatorChar}IpRangeCensys.db";

        public DbSet<IpRange> IpRanges { get; set; }
        public DbSet<CensysData> CensysDatas { get; set; }
       protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(_connectionString, options =>
            {
                options.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName + Path.DirectorySeparatorChar + "Data");
            });
           base.OnConfiguring(optionsBuilder);
        }
       protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IpRange>(entity =>
            {
                entity.HasKey(e => e.ID);
            });
            modelBuilder.Entity<CensysData>()
                        .HasOne(f => f.IpRange);
           base.OnModelCreating(modelBuilder);
        }
    }

     public class IpRange
     {
         [Key]
         public int ID { get; set; }
        
         [Required]
         public string Range { get; set; }
     }

     public class CensysData
     {
         [Key]
         [ForeignKey(nameof(IpRange))]
         public int IpRangeId { get; set; }

         public string Data { get; set; }
         public string StatusCode { get; set; }
         public string Message { get; set; }
         public IpRange IpRange { get; set; }
     }
}