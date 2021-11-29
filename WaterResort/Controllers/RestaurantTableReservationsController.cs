using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WaterResort.Data;
using WaterResort.Models;

namespace WaterResort.Controllers
{
    public class RestaurantTableReservationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RestaurantTableReservationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: RestaurantTableReservations
        public async Task<IActionResult> Index()
        {
            return View(await _context.RestaurantTableReservation.ToListAsync());
        }

        // GET: RestaurantTableReservations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var restaurantTableReservation = await _context.RestaurantTableReservation
                .FirstOrDefaultAsync(m => m.Id == id);
            if (restaurantTableReservation == null)
            {
                return NotFound();
            }

            return View(restaurantTableReservation);
        }

        // GET: RestaurantTableReservations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: RestaurantTableReservations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AccountId,ReservationDate")] RestaurantTableReservation restaurantTableReservation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(restaurantTableReservation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(restaurantTableReservation);
        }

        // GET: RestaurantTableReservations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var restaurantTableReservation = await _context.RestaurantTableReservation.FindAsync(id);
            if (restaurantTableReservation == null)
            {
                return NotFound();
            }
            return View(restaurantTableReservation);
        }

        // POST: RestaurantTableReservations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AccountId,ReservationDate")] RestaurantTableReservation restaurantTableReservation)
        {
            if (id != restaurantTableReservation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(restaurantTableReservation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RestaurantTableReservationExists(restaurantTableReservation.Id))
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
            return View(restaurantTableReservation);
        }

        // GET: RestaurantTableReservations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var restaurantTableReservation = await _context.RestaurantTableReservation
                .FirstOrDefaultAsync(m => m.Id == id);
            if (restaurantTableReservation == null)
            {
                return NotFound();
            }

            return View(restaurantTableReservation);
        }

        // POST: RestaurantTableReservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var restaurantTableReservation = await _context.RestaurantTableReservation.FindAsync(id);
            _context.RestaurantTableReservation.Remove(restaurantTableReservation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RestaurantTableReservationExists(int id)
        {
            return _context.RestaurantTableReservation.Any(e => e.Id == id);
        }
    }
}
