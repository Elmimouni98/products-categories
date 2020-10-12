using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetDotN.Models
{
    [Table("GATEGORIES")]
    public class Categorie
    {
        [Key]
        public int CategorieID { get; set; }
        [StringLength(30)]
        [MinLength(2)]
        [Display(Name = "Catégorie")]
        [Required]
        public string Nom { get; set; }
        [Display(Name = "Produits")]
        public virtual ICollection<Produit> produits { get;}
    }
}
