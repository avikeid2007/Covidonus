using MySql.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace CovidonusApi.Models
{
    public class StateWiseData : Auditor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string State { get; set; }
        [Required]
        [StringLength(5)]
        public string StateCode { get; set; }
        public ICollection<DistrictWiseData> DistrictData { get; set; }
    }
    public abstract class Auditor
    {
        [Required]
        [Column(TypeName = "datetime")]
        public DateTime? Created { get; set; }
        public string CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }
        public bool IsActive { get; set; }
    }
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class CovidonusContext : DbContext
    {
        //  static string conn = ConfigurationManager.ConnectionStrings["DefaultConnection"]?.ConnectionString;
        //Add your Dbsets here  
        public DbSet<StateWiseData> StateWiseDatas
        {
            get;
            set;
        }
        public DbSet<DistrictWiseData> DistrictWiseDatas
        {
            get;
            set;
        }
        public DbSet<DeltaData> DeltaDatas
        {
            get;
            set;
        }
        public CovidonusContext() : base("DefaultConnection")
        {

        }
    }
}