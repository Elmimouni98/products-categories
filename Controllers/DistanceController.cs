using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetDotN.Models;

namespace ProjetDotN.Controllers
{
    public class DistanceController : Controller
    {
        public MyDbContext MyDb { get; set; }

        public DistanceController(MyDbContext myDbContext)
        {
            this.MyDb = myDbContext;
        }

        //Get Data + Search + Pagination
        public IActionResult Index(string search = "",int page = 0, int size = 5)
        {
            int position = page * size;
            IEnumerable<Distance> distances = distances = MyDb.Distances
                .Skip(position).Take(size).Include(d => d.VilleDepart).Include(d => d.VilleArrive).ToList();
            if (!String.IsNullOrEmpty(search))
            {
                distances = MyDb.Distances.
                Where(d => d.VilleDepart.Nom.Contains(search) || d.VilleArrive.Nom.Contains(search))
                .Skip(position).Take(size).Include(d => d.VilleDepart).Include(d => d.VilleArrive).ToList();
            }

            ViewBag.currentPage = page;
            int nbDistances = MyDb.Distances.
                 Where(d => d.VilleDepart.Nom.Contains(search)).Where(d => d.VilleArrive.Nom.Contains(search)).ToList().Count;
            int totalPages;
            if (nbDistances % size == 0)
            {
                totalPages = nbDistances / size;
            }
            else
            {
                totalPages = nbDistances / size + 1;
            }
            ViewBag.search = search;
            ViewBag.totalPages = totalPages;
            return View("Distances", distances);
        }
        //Details
        public IActionResult Details(int? id1, int? id2)
        {
            if (id1 == null || id2 == null)
            {
                return NotFound();
            }

            var distance = MyDb.Distances.Include(d => d.VilleDepart).Include(d => d.VilleArrive)
                .FirstOrDefault(d => d.VilleDepartID == id1 && d.VilleArriveID == id2);
            if (distance == null)
            {
                return NotFound();
            }

            return View(distance);
        }

        //add 
        public IActionResult Add()
        {
            Distance d = new Distance();
            IEnumerable<Ville> villes = MyDb.Villes.ToList();
            ViewBag.villes = villes;
            return View(d);
        }


        [HttpPost]
        public IActionResult Add(Distance d)
        {
            if (ModelState.IsValid)
            {
                MyDb.Distances.Add(d);
                MyDb.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(d);
        }

        //edit 
        public IActionResult Edit(int? id1, int? id2)
        {

            if (id1 == null || id2 == null)
            {
                return NotFound();
            }

            var d = MyDb.Distances.Include(v => v.VilleDepart).Include(v => v.VilleArrive).FirstOrDefault(v => v.VilleDepartID == id1 && v.VilleArriveID == id2);
            if (d == null)
            {
                return NotFound();
            }
            IEnumerable<Ville> villes = MyDb.Villes.ToList();
            ViewBag.villes = villes;
            return View(d);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(double distance, Distance d)
        {
         
            if (ModelState.IsValid)
            {
                try
                {
                    d.distance = distance;
                    MyDb.Distances.Update(d);
                    MyDb.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DistanceExists(d.VilleDepartID, d.VilleArriveID))
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
            return View(d);
        }

        //Delete 
        public async Task<IActionResult> Delete(int? id1, int? id2)
        {
            if (id1 == null || id2 == null)
            {
                return NotFound();
            }

            var dist = await MyDb.Distances.Include(d=>d.VilleDepart).Include(d=>d.VilleArrive)
                .FirstOrDefaultAsync(d => d.VilleDepartID == id1 && d.VilleArriveID == id2).ConfigureAwait(false);
            if (dist == null)
            {
                return NotFound();
            }

            return View(dist);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Distance d)
        {
            var dist =  MyDb.Distances.Find(d.VilleDepartID,d.VilleArriveID);
            if(dist != null)
            {
                MyDb.Distances.Remove(dist);
                MyDb.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            
            return NotFound();
        }
       
        private bool DistanceExists(int id1,int id2)
        {
            return MyDb.Distances.Any(d => d.VilleDepartID == id1 && d.VilleArriveID == id2);
        }
    }
}