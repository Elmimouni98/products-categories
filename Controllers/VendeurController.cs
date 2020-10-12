using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetDotN.Models;

namespace ProjetDotN.Controllers
{
    public class VendeurController : Controller
    {
        public MyDbContext MyDb { get; set; }

        public VendeurController(MyDbContext myDbContext)
        {
            this.MyDb = myDbContext;
        }

        //Get Data + Search + Pagination
        public IActionResult Index(string search="",int page = 0, int size = 5)
        {
            int position = page * size;
            IEnumerable<Vendeur> vendeurs = MyDb.Vendeurs.Skip(position).Take(size).Include(v => v.Ville).ToList();
            if (!String.IsNullOrEmpty(search))
            {
                vendeurs = MyDb.Vendeurs.
                Where(v => v.Nom.Contains(search))
                .Skip(position).Take(size).Include(v => v.Ville).ToList();
            }

            ViewBag.currentPage = page;
            int nbVendeurs = MyDb.Vendeurs.
                 Where(v => v.Nom.Contains(search)).ToList().Count;
            int totalPages;
            if (nbVendeurs % size == 0)
            {
                totalPages = nbVendeurs / size;
            }
            else
            {
                totalPages = nbVendeurs / size + 1;
            }
            ViewBag.search = search;
            ViewBag.totalPages = totalPages;
            return View("Vendeurs", vendeurs);
        }
        //Details
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vendeur = MyDb.Vendeurs.Include(v => v.Ville)
                .FirstOrDefault(v => v.VendeurID == id);

            IEnumerable<Produit> produits = MyDb.Produits.Include(p => p.Prix).Include(p => p.Categorie)
            .Where(p => p.VendeurID == id);

            ViewBag.produits = produits;

            if (vendeur == null)
            {
                return NotFound();
            }

            return View(vendeur);
        }
        //Add vendor
        public IActionResult Add()
        {
            Vendeur v = new Vendeur();
            IEnumerable<Ville> villes = MyDb.Villes.ToList();
            ViewBag.villes = villes;
            return View(v);
        }


        [HttpPost]
        public IActionResult Add(Vendeur vendeur)
        {
            if (ModelState.IsValid)
            {
                MyDb.Vendeurs.Add(vendeur);
                MyDb.SaveChanges();
                return RedirectToAction("Index");
            }
            IEnumerable<Ville> villes = MyDb.Villes.ToList();
            ViewBag.villes = villes;
            return View("Add", vendeur);
        }
        //Edit vendor
        public IActionResult Edit(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var vendeur =  MyDb.Vendeurs.Include(v => v.Ville).FirstOrDefault(v => v.VendeurID == id);
            if (vendeur == null)
            {
                return NotFound();
            }
            IEnumerable<Ville> villes = MyDb.Villes.ToList();
            ViewBag.villes = villes;
            return View(vendeur);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id,Vendeur vendeur)
        {
            if (id != vendeur.VendeurID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    MyDb.Vendeurs.Update(vendeur);
                    MyDb.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VendeurExists(vendeur.VendeurID))
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
            return View(vendeur);
        }

        //Delete vendor
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vendeur = MyDb.Vendeurs.Include(v => v.Ville)
                .FirstOrDefault(v => v.VendeurID == id);
            if (vendeur == null)
            {
                return NotFound();
            }

            return View(vendeur);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var vendeur = MyDb.Vendeurs.Find(id);
            MyDb.Vendeurs.Remove(vendeur);
            MyDb.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        private bool VendeurExists(int id)
        {
            return MyDb.Vendeurs.Any(e => e.VendeurID == id);
        }
    }
}