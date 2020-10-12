using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetDotN.Models
{
    [Table("PRODUITS")]
    public class Produit
    {
        [Key]
        public int ProduitID { get; set; }
        [StringLength(30)]
        [Required]
        [MinLength(2), MaxLength(20)]
        [Display(Name = "Produit")]
        public string Designation { get; set; }
        public int CategorieID { get; set; }
        public int PrixID { get; set; }
        public int VendeurID { get; set; }
        [ForeignKey("CategorieID")]
        [Display(Name = "Catégorie")]
        public virtual Categorie Categorie { get; set; }
        [ForeignKey("PrixID")]
        [Display(Name = "Prix")]
        public virtual Prix Prix { get; set; }
        [ForeignKey("VendeurID")]
        [Display(Name = "Vendeur")]
        public virtual Vendeur Vendeur { get; set; }
        [Display(Name = "Commandes")]
        public virtual ICollection<Commande> commandes { get; set; }

    }
}
