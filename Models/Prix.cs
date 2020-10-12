using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetDotN.Models
{
    [Table("PRIX")]
    public class Prix
    {
 
        [Key]
        public int PrixID { get; set; }
        [Display(Name = "Prix (DH)")]
        [Range(1, 100000)]
        [Required]
        public double Price { get; set; }
        [Display(Name = "Produits")]
        public virtual ICollection<Produit> produits { get; set; }
        [Display(Name = "Vendeurs")]
        public virtual ICollection<Vendeur> vendeurs { get;}

    }
}
