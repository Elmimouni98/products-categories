using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProjetDotN.Models;
using Microsoft.EntityFrameworkCore;

namespace ProjetDotN.Controllers
{
    public class VilleController : Controller
    {
        public MyDbContext MyDb { get; set; }

        public VilleController(MyDbContext myDbContext)
        {
            this.MyDb = myDbContext;
        }

        //Get Data + Search + Pagination
        public IActionResult Index(int page = 0, int size = 5, string search = "")
        {
            int position = page * size;
            IEnumerable<Ville> villes = villes = MyDb.Villes.Skip(position).Take(size).ToList();
            if (!String.IsNullOrEmpty(search))
            {
                villes = MyDb.Villes.Where(v => v.Nom.Contains(search)).Skip(position).Take(size).ToList();
            }
            ViewBag.currentPage = page;
            int nbVilles = MyDb.Villes.
                 Where(v => v.Nom.Contains(search)).ToList().Count;
            int totalPages;
            if (nbVilles % size == 0)
            {
                totalPages = nbVilles / size;
            }
            else
            {
                totalPages = nbVilles / size + 1;
            }
            ViewBag.search = search;
            ViewBag.totalPages = totalPages;
            return View("Villes", villes);
        }

        //Add
        public IActionResult Add()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add([Bind("VilleID,Nom")] Ville ville)
        {
            if (ModelState.IsValid)
            {
                MyDb.Add(ville);
                await MyDb.SaveChangesAsync().ConfigureAwait(false);
                return RedirectToAction(nameof(Index));
            }
            return View(ville);
        }
        //Edit
        public  IActionResult Edit(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var ville =  MyDb.Villes.FirstOrDefault(v => v.VilleID == id);
            if (ville == null)
            {
                return NotFound();
            }
            return View(ville);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VilleID,Nom")] Ville ville)
        {
            if (id != ville.VilleID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    MyDb.Update(ville);
                    await MyDb.SaveChangesAsync().ConfigureAwait(false);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VilleExists(ville.VilleID))
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
            return View(ville);
        }

        //Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ville = await MyDb.Villes.FirstOrDefaultAsync(p => p.VilleID == id).ConfigureAwait(false);
            if (ville == null)
            {
                return NotFound();
            }

            return View(ville);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ville = await MyDb.Villes.FindAsync(id).ConfigureAwait(false);
            MyDb.Villes.Remove(ville);
            await MyDb.SaveChangesAsync().ConfigureAwait(false);
            return RedirectToAction(nameof(Index));
        }

        private bool VilleExists(int id)
        {
            return MyDb.Villes.Any(e => e.VilleID == id);
        }
    }
}