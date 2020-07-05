using MySql.Data.Entity;
using System.Data.Entity;

namespace CovidonusApi.Models
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class CovidonusContext : DbContext
    {
        public DbSet<StateWiseData> StateWiseDatas { get; set; }
        public DbSet<DistrictWiseData> DistrictWiseDatas { get; set; }
        public DbSet<DeltaData> DeltaDatas { get; set; }
        public DbSet<CasesTimeSeries> CasesTimeSeries { get; set; }
        public DbSet<Tested> Testeds { get; set; }
        public DbSet<DailyStateWiseData> DailyStateWiseDatas { get; set; }
        public DbSet<DailyDistrictWiseData> DailyDistrictWiseDatas { get; set; }
        public CovidonusContext() : base("DefaultConnection")
        {
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
        }
        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<DistrictWiseData>()
        //        .HasOptional(a => a.Delta)
        //        .WithRequired(ab => ab.DistrictWiseData);
        //}
    }
}