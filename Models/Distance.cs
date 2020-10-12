using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetDotN.Models
{
    [Table("DISTANCES")]
    public class Distance
    {
        public int VilleDepartID { get; set; }
        public int VilleArriveID { get; set; }
        [Range(1, 4000)]
        [Display(Name = "Distance (KM)")]
        [Required]
        public double distance { get; set; }
        [ForeignKey("VilleDepartID")]
        [Display(Name = "Ville de départ")]
        public virtual Ville VilleDepart { get; set; }
        [ForeignKey("VilleArriveID")]
        [Display(Name = "Ville d'arrivée")]
        public virtual Ville VilleArrive { get; set; }
       
    }
}
