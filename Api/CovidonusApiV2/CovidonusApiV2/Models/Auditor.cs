using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CovidonusApiV2.Models
{
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
}