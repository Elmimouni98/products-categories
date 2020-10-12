using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ProjetDotN.Models;
using Microsoft.EntityFrameworkCore;


namespace ProjetDotN.Controllers
{
    public class PrixController : Controller
    {
        public MyDbContext MyDb { get; set; }

        public PrixController(MyDbContext myDbContext)
        {
            this.MyDb = myDbContext;
        }

        public IActionResult Index()
        {
            IEnumerable<Produit> produits = MyDb.Produits.ToList();
            ViewBag.produits = produits;

            IEnumerable<Ville> villes = MyDb.Villes.ToList();
            ViewBag.villes = villes;

            return View("Prixs");
        }

        //Get la meilleur prix pour un produit en tenant compte la distance
        public IActionResult MeilleurPrix(string designation, string villeDepart)
        {
            IEnumerable<Produit> produits = MyDb.Produits.Where(p => p.Designation.Equals(designation))
                .Include(p => p.Vendeur).Include(p => p.Vendeur.Ville).Include(p => p.Prix).ToList();

            List<Tuple<double, Vendeur,double>> ListPrixs = new List<Tuple<double, Vendeur,double>>();

            foreach (Produit p in produits)
            {
                string villeArrive = p.Vendeur.Ville.Nom;
                double distance = this.getDistance(villeDepart, villeArrive);
                //On suppose : 3 dirham / Km (aller retour donc 6 DH)
                double prix = p.Prix.Price + distance * 6;
                ListPrixs.Add(Tuple.Create(prix, p.Vendeur,distance));
            }
            if(ListPrixs.Count != 0)
            {
                var minPrix = ListPrixs.Min(p => p.Item1);
                var vendeur = ListPrixs.FirstOrDefault(p => p.Item1.Equals(minPrix)).Item2;
                var d = ListPrixs.FirstOrDefault(p => p.Item1.Equals(minPrix)).Item3;

                ViewBag.minPrix = minPrix;
                ViewBag.vendeur = vendeur;
                ViewBag.distance = d;

                return View("MeilleurPrix");
            }
            else
            {
                return NotFound();
            }
          
        }


        //Get la distance entre deux villes
        private double getDistance(string ville1, string ville2)
        {
            double distance = 0;
            IEnumerable<Distance> distances = MyDb.Distances.Include(d => d.VilleDepart).Include(d => d.VilleArrive).ToList();
            foreach (Distance d in distances)
            {
                if (d.VilleDepart.Nom.Equals(ville1) && d.VilleArrive.Nom.Equals(ville2) || (d.VilleDepart.Nom.Equals(ville2) && d.VilleArrive.Nom.Equals(ville1)))
                {
                    distance = d.distance;
                    break;
                }
            }
            return distance;
        }
    }
}