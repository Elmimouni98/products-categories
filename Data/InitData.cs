
namespace ProjetDotN.Models
{
    public static class InitData
    {
        public static void init(MyDbContext myDbContext)
        {

            myDbContext.Database.EnsureDeleted();
            myDbContext.Database.EnsureCreated();

            //Categories
            var categories = new Categorie[]
            {
                new Categorie { Nom = "Ordinateur" },
                new Categorie { Nom = "Portable" },
                new Categorie { Nom = "Montre" },
                new Categorie { Nom = "Casque" },
                new Categorie { Nom = "TV" },
                new Categorie { Nom = "Caméra" }
            };

            foreach (Categorie c in categories)
            {
                myDbContext.Categories.Add(c);
            }
            myDbContext.SaveChanges();

            //Villes
            var villes = new Ville[]
            {
               new Ville { Nom = "Agadir" },
               new Ville { Nom = "Rabat" },
               new Ville { Nom = "Casa" },
               new Ville { Nom = "Marrakech" },
               new Ville { Nom = "Tanger" },
               new Ville { Nom = "Oujda" }
            };
            foreach (Ville v in villes)
            {
                myDbContext.Villes.Add(v);
            }

            myDbContext.SaveChanges();

            //Prix
            var prix = new Prix[]
            {
                new Prix { Price = 10000 },
                new Prix { Price = 9000 },
                new Prix { Price = 8000 },
                new Prix { Price = 7000 },
                new Prix { Price = 6000 },
                new Prix { Price = 5000 }
            };

            foreach (Prix p in prix)
            {
                myDbContext.Prix.Add(p);
            }

            myDbContext.SaveChanges();

            //Vendeurs
            var vendeurs = new Vendeur[]
            {
               new Vendeur { Nom = "Bestmark", VilleID = 1},
               new Vendeur { Nom = "Electroplanet", VilleID = 2},
               new Vendeur { Nom = "Technoplace", VilleID = 3 },
               new Vendeur { Nom = "Bestmark", VilleID = 3 },
               new Vendeur { Nom = "ETCEINFO", VilleID = 4 },
               new Vendeur { Nom = "Electroplanet", VilleID = 5 },
               new Vendeur { Nom = "EVIMAS", VilleID = 6 },
            };

            foreach (Vendeur v in vendeurs)
            {
                myDbContext.Vendeurs.Add(v);
            }

            myDbContext.SaveChanges();

            //Produits
            var produits = new Produit[]
            {
               new Produit { Designation = "HP Elitebook 840", CategorieID = 1, PrixID = 1 ,VendeurID=1},
               new Produit { Designation = "iPhone 11 Pro", CategorieID = 2, PrixID = 2,VendeurID=2 },
               new Produit { Designation = "Garmin Venu", CategorieID = 3, PrixID = 3 ,VendeurID=3},
               new Produit { Designation = "Bose casque", CategorieID = 4, PrixID = 6 ,VendeurID=4},
               new Produit { Designation = "Samsung TV", CategorieID = 5, PrixID = 5 ,VendeurID=5},
               new Produit { Designation = "SONY DSC-HX350", CategorieID = 6, PrixID = 4 ,VendeurID=6},
               new Produit { Designation = "Sony ILCE-6000L", CategorieID = 6, PrixID = 1 ,VendeurID=7}
            };

            foreach (Produit p in produits)
            {
                myDbContext.Produits.Add(p);
            }

            myDbContext.SaveChanges();

            //Clients
            var clients = new Client[]
            {
               new Client {Nom = "Mohamed", VilleID = 1 },
               new Client { Nom = "Aziz", VilleID = 2 },
               new Client { Nom = "Karim", VilleID = 3 },
               new Client { Nom = "Imad", VilleID = 4 },
               new Client { Nom = "omar", VilleID = 5 },
               new Client { Nom = "brahim", VilleID = 6 }
            };

            foreach (Client c in clients)
            {
                myDbContext.Clients.Add(c);
            }

            myDbContext.SaveChanges();

            //Commandes
            var Commandes = new Commande[]
            {
               new Commande { Quantite = 2, ProduitID = 1, ClientID = 1 },
               new Commande { Quantite = 3, ProduitID = 2, ClientID = 2 },
               new Commande { Quantite = 4, ProduitID = 3, ClientID = 3 },
               new Commande { Quantite = 1, ProduitID = 4, ClientID = 4 },
               new Commande { Quantite = 2, ProduitID = 5, ClientID = 5 },
               new Commande { Quantite = 3, ProduitID = 6, ClientID = 6 },
               new Commande { Quantite = 1, ProduitID = 7, ClientID = 4 }
            };

            foreach (Commande c in Commandes)
            {
                myDbContext.Commandes.Add(c);
            }

            myDbContext.SaveChanges();

            //Distances
            var distances = new Distance[]
            {
               new Distance { distance = 549,  VilleDepartID = 1, VilleArriveID = 2 },
               new Distance { distance = 465,  VilleDepartID = 1, VilleArriveID = 3 },
               new Distance { distance = 248,  VilleDepartID = 1, VilleArriveID = 4 },
               new Distance { distance = 803,  VilleDepartID = 1, VilleArriveID = 5 },
               new Distance { distance = 1075, VilleDepartID = 1, VilleArriveID = 6 },
               new Distance { distance = 90,   VilleDepartID = 2, VilleArriveID = 3 },
               new Distance { distance = 327,  VilleDepartID = 2, VilleArriveID = 4 },
               new Distance { distance = 308 , VilleDepartID = 2, VilleArriveID = 5 },
               new Distance { distance = 518,  VilleDepartID = 2, VilleArriveID = 6 },
               new Distance { distance = 241,  VilleDepartID = 3, VilleArriveID = 4 },
               new Distance { distance = 342,  VilleDepartID = 3, VilleArriveID = 5 },
               new Distance { distance = 618,  VilleDepartID = 3, VilleArriveID = 6 },
               new Distance { distance = 577,  VilleDepartID = 4, VilleArriveID = 5 },
               new Distance { distance = 851,  VilleDepartID = 4, VilleArriveID = 6 },
               new Distance { distance = 722,  VilleDepartID = 5, VilleArriveID = 6 },
            };

            foreach (Distance d in distances)
            {
                myDbContext.Distances.Add(d);
            }

            myDbContext.SaveChanges();
            
        }
    }
}
