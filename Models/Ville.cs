using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetDotN.Models
{
    [Table("VILLES")]
    public class Ville
    {
        [Key]
        public int VilleID { get; set; }
        [StringLength(30, MinimumLength = 1)]
        [Required]
        [Display(Name = "Ville")]
        public string Nom { get; set; }
    }
}
