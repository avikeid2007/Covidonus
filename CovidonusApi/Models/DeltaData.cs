using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CovidonusApi.Models
{
    public class DeltaData
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int Confirmed { get; set; }
        public int Deceased { get; set; }
        public int Recovered { get; set; }
    }
}