using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetDotN.Models
{
    public class MyDbContext : DbContext
    {
        public DbSet<Produit> Produits { get; set; }
        public DbSet<Categorie> Categories { get; set; }
        public DbSet<Commande> Commandes { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Ville> Villes { get; set; }
        public DbSet<Vendeur> Vendeurs { get; set; }
        public DbSet<Prix> Prix { get; set; }
        public DbSet<Distance> Distances { get; set; }
        public MyDbContext(DbContextOptions<MyDbContext> options):base(options){}


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Categorie>().ToTable("Categories");
            modelBuilder.Entity<Ville>().ToTable("Villes");
            modelBuilder.Entity<Prix>().ToTable("Prix");
            modelBuilder.Entity<Produit>().ToTable("Produits");
            modelBuilder.Entity<Client>().ToTable("Clients");
            modelBuilder.Entity<Vendeur>().ToTable("Vendeurs");
            modelBuilder.Entity<Commande>().ToTable("Commandes");
            modelBuilder.Entity<Distance>().ToTable("Distances");

            modelBuilder.Entity<Distance>()
                        .HasOne(d => d.VilleDepart)
                        .WithMany()
                        .HasForeignKey(d => d.VilleDepartID)
                        .OnDelete(default); ;

            modelBuilder.Entity<Distance>()
                        .HasOne(d => d.VilleArrive)
                        .WithMany()
                        .HasForeignKey(d => d.VilleArriveID)
                        .OnDelete(default);

            modelBuilder.Entity<Distance>()
                .HasKey(d => new { d.VilleDepartID, d.VilleArriveID });

            modelBuilder.Entity<Produit>()
                       .HasOne(p=>p.Categorie)
                       .WithMany(p=>p.produits)
                       .HasForeignKey(p => p.CategorieID);


            modelBuilder.Entity<Commande>()
                     .HasOne(c => c.Produit)
                     .WithMany()
                     .HasForeignKey(p => p.ProduitID)
                     .OnDelete(default);
        }
    }
}
