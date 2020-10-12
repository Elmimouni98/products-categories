using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ProjetDotN.Models
{
    [Table("CLIENTS")]
    public class Client
    {
        [Key]
        public int ClientID { get; set; }
        [StringLength(30)]
        [MinLength(2)]
        [Display(Name = "Client")]
        [Required]
        public string Nom { get; set; }
        public int VilleID { get; set; }
        [ForeignKey("VilleID")]
        [Display(Name = "Ville client")]
        public virtual Ville Ville { get; set; }
        [Display(Name = "Commandes")]
        public virtual ICollection<Commande> commandes { get; }
    }
}
