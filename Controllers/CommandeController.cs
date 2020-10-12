using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetDotN.Models;

namespace ProjetDotN.Controllers
{
    public class CommandeController : Controller
    {
        public MyDbContext MyDb { get; set; }

        public CommandeController(MyDbContext myDbContext)
        {
            this.MyDb = myDbContext;
        }

        //Get Data + Search + Pagination
        public IActionResult Index(string search = "",int page = 0, int size = 5)
        {
            int position = page * size;
            IEnumerable<Commande> commandes = commandes = MyDb.Commandes
                .Skip(position).Take(size).Include(p => p.Produit).Include(p => p.Client).ToList();
            if (!String.IsNullOrEmpty(search))
            {
                commandes = MyDb.Commandes.
                Where(c => c.Produit.Designation.Contains(search))
                .Skip(position).Take(size).Include(p => p.Produit).Include(p => p.Client).ToList();
            }

            ViewBag.currentPage = page;
            int nbCommandes = MyDb.Commandes.
                 Where(p => p.Produit.Designation.Contains(search)).ToList().Count;
            int totalPages;
            if (nbCommandes % size == 0)
            {
                totalPages = nbCommandes / size;
            }
            else
            {
                totalPages = nbCommandes / size + 1;
            }
            ViewBag.search = search;
            ViewBag.totalPages = totalPages;
            return View("Commandes", commandes);
        }

        //Details
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var commande = MyDb.Commandes.Include(c => c.Produit).Include(c => c.Client)
                .FirstOrDefault(m => m.CommandeID == id);

            if (commande == null)
            {
                return NotFound();
            }

            return View(commande);
        }

        //Edit
        public IActionResult Edit(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var commande = MyDb.Commandes.Include(c => c.Produit).Include(c => c.Client)
                .FirstOrDefault(c => c.CommandeID == id);
            if (commande == null)
            {
                return NotFound();
            }
            IEnumerable<Produit> produits = MyDb.Produits.ToList();
            ViewBag.produits = produits;
            IEnumerable<Client> clients = MyDb.Clients.ToList();
            ViewBag.clients = clients;

            return View(commande);
        }

        [HttpPost]
        public IActionResult Edit(int id, Commande commande)
        {
            if (id != commande.CommandeID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    MyDb.Commandes.Update(commande);
                    MyDb.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommandeExists(commande.CommandeID))
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
            return View(commande);
        }

        //Delete
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var commande = MyDb.Commandes.Include(c => c.Produit).Include(c => c.Client)
                .FirstOrDefault(c => c.CommandeID == id);
            if (commande == null)
            {
                return NotFound();
            }

            return View(commande);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var commande = MyDb.Commandes.Find(id);
            MyDb.Commandes.Remove(commande);
            MyDb.SaveChanges();
            return RedirectToAction("Index");
        }

        private bool CommandeExists(int id)
        {
            return MyDb.Commandes.Any(e => e.CommandeID == id);
        }


    }
}