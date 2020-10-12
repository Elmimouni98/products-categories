using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetDotN.Models;

namespace ProjetDotN.Controllers
{
    //[Route("/api/produits")]
    public class ProduitController : Controller
    {
        public MyDbContext MyDb { get; set; }

        public ProduitController(MyDbContext myDbContext)
        {
            this.MyDb = myDbContext;
        }

        //Get Data + Search + Pagination
        public IActionResult Index(string search = "",int page = 0, int size = 5)
        {
            int position = page * size;
            IEnumerable<Produit> produits = produits = MyDb.Produits
                .Skip(position).Take(size).Include(p => p.Categorie).Include(p => p.Prix)
                .Include(p => p.Vendeur).Include(p => p.Vendeur.Ville).ToList();

            if (!String.IsNullOrEmpty(search))
            {
                produits = MyDb.Produits.
                Where(p => p.Designation.Contains(search))
                .Skip(position).Take(size).Include(p => p.Categorie).Include(p => p.Prix)
                .Include(p => p.Vendeur).Include(p => p.Vendeur.Ville).ToList();
            }

            ViewBag.currentPage = page;
            int nbProduits = MyDb.Produits.
                 Where(p => p.Designation.Contains(search)).ToList().Count;
            int totalPages;
            if (nbProduits % size == 0)
            {
                totalPages = nbProduits / size;
            }
            else
            {
                totalPages = nbProduits / size + 1;
            }
            ViewBag.search = search;
            ViewBag.totalPages = totalPages;
            return View("Produits", produits);
        }

        //Details
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produit =  MyDb.Produits.Include(p => p.Prix).Include(p => p.Categorie).Include(p => p.Vendeur)
                .Include(p=>p.Vendeur.Ville).FirstOrDefault(m => m.ProduitID == id);

            IEnumerable<Commande> commandes = MyDb.Commandes.Include(c => c.Client)
              .Where(c => c.CommandeID == id);

            ViewBag.commandes = commandes;
            if (produit == null)
            {
                return NotFound();
            }

            return View(produit);
        }

        //Add product
        public IActionResult Add()
        {
            Produit p = new Produit();
            IEnumerable<Vendeur> vendeurs = MyDb.Vendeurs.ToList();
            ViewBag.vendeurs = vendeurs;

            IEnumerable<Categorie> cats = MyDb.Categories.ToList();
            ViewBag.cats = cats;
       
            IEnumerable<Ville> villes = MyDb.Villes.ToList();
            ViewBag.villes = villes;

            IEnumerable<Prix> prix = MyDb.Prix.ToList();
            ViewBag.prix = prix;

            return View(p);
        }

  
        [HttpPost]
        public IActionResult Add(Produit produit)
        {
            if (ModelState.IsValid)
            {
                MyDb.Produits.Add(produit);
                MyDb.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(produit);
        }

        //Edit product
        public IActionResult Edit(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var produit = MyDb.Produits.Include(p => p.Prix).Include(p => p.Categorie).Include(p => p.Vendeur)
                .Include(p => p.Vendeur.Ville).FirstOrDefault(p => p.ProduitID == id);
            if (produit == null)
            {
                return NotFound();
            }
            IEnumerable<Vendeur> vendeurs = MyDb.Vendeurs.ToList();
            ViewBag.vendeurs = vendeurs;

            IEnumerable<Categorie> cats = MyDb.Categories.ToList();
            ViewBag.cats = cats;

            IEnumerable<Ville> villes = MyDb.Villes.ToList();
            ViewBag.villes = villes;

            IEnumerable<Prix> prix = MyDb.Prix.ToList();
            ViewBag.prix = prix;

            return View(produit);
        }

        [HttpPost]
        public IActionResult Edit(int id,Produit produit)
        {
            if (id != produit.ProduitID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    MyDb.Produits.Update(produit);
                    MyDb.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProduitExists(produit.ProduitID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(produit);
        }

        //Delete product
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produit = MyDb.Produits.Include(p => p.Prix).Include(p => p.Categorie).Include(p => p.Vendeur)
                .Include(p=>p.Vendeur.Ville).FirstOrDefault(p => p.ProduitID == id);
            if (produit == null)
            {
                return NotFound();
            }

            return View(produit);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var produit =  MyDb.Produits.Find(id);
            MyDb.Produits.Remove(produit);
            MyDb.SaveChanges();
            return RedirectToAction("Index");
        }

        private bool ProduitExists(int id)
        {
            return MyDb.Produits.Any(e => e.ProduitID == id);
        }

        //Add command
        public IActionResult Command(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Commande commande = new Commande();
            Produit produit = MyDb.Produits.FirstOrDefault(p => p.ProduitID == id);
            commande.Produit = produit;
            ViewBag.nom = produit.Designation;
            if (produit == null)
            {
                return NotFound();
            }
            commande.ProduitID = produit.ProduitID;
            return View("Command",commande);
        }

        [HttpPost]
        public IActionResult AddCommand(Commande commande,int quantite,int idClient)
        {
             commande.Quantite = quantite;
             commande.ClientID = idClient;
             MyDb.Commandes.Add(commande);
             MyDb.SaveChanges();
             return RedirectToAction("Index");
        }


            /*API
            [HttpGet]
            public IEnumerable<Produit> listeProduits()
            {
                return MyDb.Produits.Include(p => p.Categorie).Include(p=>p.Prix);
            }
            [HttpGet("clients")]
            public IEnumerable<Client> listeClients()
            {
                return MyDb.Clients.Include(p => p.Ville);
            }

            //Pagination
            [HttpGet("paginate")]
            public IEnumerable<Produit> page(int page=0,int size=1)
            {
                int skipValue = page * size;
                return MyDb.Produits.Include(p => p.Categorie)
                           .Skip(skipValue)
                           .Take(size);
            }

            //search product
            [HttpGet("search")]
            public IEnumerable<Produit> chercher(string name)
            {
                return MyDb.Produits.Include(p => p.Categorie)
                        .Where(p => p.Designation.Contains(name));
            }


            //Get product by ID
            [HttpGet("{id}")]
            public Produit getProduit(int id)
            {
                return MyDb.Produits.Include(p => p.Categorie)
                           .FirstOrDefault(c=>c.ProduitID == id);
            }

            //save product
            [HttpPost]
            public Produit saveProduit([FromBody] Produit produit)
            {
                 MyDb.Produits.Add(produit);
                 MyDb.SaveChanges();
                 return produit;
            }

            //update product
            [HttpPut("{id}")]
            public Produit updateProduit([FromBody] Produit produit, int id)
            {
                produit.ProduitID = id;
                MyDb.Produits.Update(produit);
                MyDb.SaveChanges();
                return produit;
            }

            //delete product
            [HttpDelete("{id}")]
            public void deleteProduit(int id)
            {
                Produit produit = MyDb.Produits.FirstOrDefault(c => c.ProduitID == id);
                MyDb.Produits.Remove(produit);
                MyDb.SaveChanges();
            }
            */
        }
}