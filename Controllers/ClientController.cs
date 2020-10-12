using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetDotN.Models;

namespace ProjetDotN.Controllers
{
    public class ClientController : Controller
    {
        public MyDbContext MyDb { get; set; }

        public ClientController(MyDbContext myDbContext)
        {
            this.MyDb = myDbContext;
        }

        //Get Data + Search + Pagination
        public IActionResult Index(string search = "",int page = 0, int size = 5)
        {
            int position = page * size;
            IEnumerable<Client> clients = MyDb.Clients
                .Skip(position).Take(size).Include(v => v.Ville).ToList();
            if (!String.IsNullOrEmpty(search))
            {
                clients = MyDb.Clients.
                Where(c => c.Nom.Contains(search))
                .Skip(position).Take(size).Include(v => v.Ville).ToList();
            }

            ViewBag.currentPage = page;
            int nbClients = MyDb.Clients.
                 Where(c => c.Nom.Contains(search)).ToList().Count;
            int totalPages;
            if (nbClients % size == 0)
            {
                totalPages = nbClients / size;
            }
            else
            {
                totalPages = nbClients / size + 1;
            }
            ViewBag.search = search;
            ViewBag.totalPages = totalPages;
            return View("Clients", clients);
        }

        //Details
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = MyDb.Clients.Include(v => v.Ville)
                .FirstOrDefault(c => c.ClientID == id);
            IEnumerable<Commande> commandes = MyDb.Commandes.Include(c => c.Produit)
                .Where(c => c.ClientID == id);

            ViewBag.commandes = commandes;

            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        //Add client
        public IActionResult Add()
        {
            Client c = new Client();
            IEnumerable<Ville> villes = MyDb.Villes.ToList();
            ViewBag.villes = villes;
            return View(c);
        }


        [HttpPost]
        public IActionResult Add(Client client)
        {
            if (ModelState.IsValid)
            {
                MyDb.Clients.Add(client);
                MyDb.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(client);
        }

        //Edit client
        public IActionResult Edit(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var client = MyDb.Clients.Include(v => v.Ville).FirstOrDefault(v => v.ClientID == id);
            if (client == null)
            {
                return NotFound();
            }
            IEnumerable<Ville> villes = MyDb.Villes.ToList();
            ViewBag.villes = villes;
            return View(client);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id,Client client)
        {
            if (id != client.ClientID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    MyDb.Update(client);
                    MyDb.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClientExists(client.ClientID))
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
            return View(client);
        }

        //Delete client
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = MyDb.Clients.Include(v => v.Ville)
                .FirstOrDefault(v => v.ClientID == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var client = MyDb.Clients.Find(id);
            MyDb.Clients.Remove(client);
            MyDb.SaveChanges();
            return RedirectToAction("Index");
        }

        private bool ClientExists(int id)
        {
            return MyDb.Clients.Any(e => e.ClientID == id);
        }
    }
}