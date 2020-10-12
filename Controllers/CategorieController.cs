using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetDotN.Models;

namespace ProjetDotN.Controllers
{
    
    //[Route("/api/categories")]
    public class CategorieController : Controller
    {
        public MyDbContext MyDb { get; set; }

        public CategorieController(MyDbContext myDbContext)
        {
            this.MyDb = myDbContext;
        }

        //Get Data + Search + Pagination
        public IActionResult Index(string search ="",int page = 0, int size = 5)
        {
            int position = page * size;
            IEnumerable<Categorie> categories = categories = MyDb.Categories
                .Skip(position).Take(size).ToList();
            if (!String.IsNullOrEmpty(search))
            {
                categories = MyDb.Categories.
                Where(c => c.Nom.Contains(search))
                .Skip(position).Take(size).ToList();
            }
           
            ViewBag.currentPage = page;
            int nbCategories = MyDb.Categories.
                 Where(c => c.Nom.Contains(search)).ToList().Count;
            int totalPages;
            if (nbCategories % size == 0)
            {
                totalPages = nbCategories / size;
            }
            else
            {
                totalPages = nbCategories / size + 1;
            }
            ViewBag.search = search;
            ViewBag.totalPages = totalPages;
            return View("Categories", categories);
        }
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categorie = MyDb.Categories.Include(c => c.produits)
                .FirstOrDefault(c => c.CategorieID == id);

            IEnumerable<Produit> produits = MyDb.Produits.Include(p => p.Prix).Include(p => p.Vendeur).Include(p => p.Vendeur.Ville)
                .Where(p =>p.CategorieID == id);

            ViewBag.produits = produits;
            if (categorie == null)
            {
                return NotFound();
            }
            return View(categorie);
        }

        //Add categorie
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add([Bind("CategorieID,Nom")] Categorie categorie)
        {
            if (ModelState.IsValid)
            {
                MyDb.Add(categorie);
                await MyDb.SaveChangesAsync().ConfigureAwait(false);
                return RedirectToAction(nameof(Index));
            }
            return View(categorie);
        }

        //Edit categorie
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categorie = await MyDb.Categories.FirstOrDefaultAsync(c => c.CategorieID == id).ConfigureAwait(false);
            if (categorie == null)
            {
                return NotFound();
            }
            return View(categorie);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CategorieID,Nom")] Categorie categorie)
        {
            if (id != categorie.CategorieID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    MyDb.Update(categorie);
                    await MyDb.SaveChangesAsync().ConfigureAwait(false);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategorieExists(categorie.CategorieID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(categorie);
        }

        //Delete categorie
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categorie = await MyDb.Categories.FirstOrDefaultAsync(p => p.CategorieID == id).ConfigureAwait(false);
            if (categorie == null)
            {
                return NotFound();
            }

            return View(categorie);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var categorie = await MyDb.Categories.FindAsync(id).ConfigureAwait(false);
            MyDb.Categories.Remove(categorie);
            await MyDb.SaveChangesAsync().ConfigureAwait(false);
            return RedirectToAction(nameof(Index));
        }

        private bool CategorieExists(int id)
        {
            return MyDb.Categories.Any(e => e.CategorieID == id);
        }
    }
}