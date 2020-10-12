using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetDotN.Models
{
    [Table("VENDEURS")]
    public class Vendeur
    {
        [Key]
        public int VendeurID { get; set; }
        [Required]
        [Display(Name = "Vendeur")]
        [StringLength(30)]
        [MinLength(2)]
        public string Nom { get; set; }
        public int VilleID { get; set; }
        [Display(Name = "Prixs")]
        public virtual ICollection<Prix> prix { get; }
        [Display(Name = "¨Produits")]
        public virtual ICollection<Produit> produits { get;  }
        [ForeignKey("VilleID")]
        [Display(Name = "Ville vendeur")]
        public virtual Ville Ville { get; set; }
    }
}
