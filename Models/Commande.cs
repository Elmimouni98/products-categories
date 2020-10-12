using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetDotN.Models
{
    [Table("COMMANDES")]
    public class Commande
    {
        [Key]
        [Display(Name = "Numéro")]
        public int CommandeID { get; set; }
        [Display(Name = "Quantité")]
        [Range(1, 100)]
        [Required]
        public int Quantite { get; set; } 
        public int ProduitID { get; set; }
        public int ClientID { get; set; }
        [ForeignKey("ProduitID")]
        public virtual Produit Produit { get; set; }
        [ForeignKey("ClientID")]
        public virtual Client Client { get; set; }
    }
}
