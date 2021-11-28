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
    public class CurrentReservationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CurrentReservationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CurrentReservations
        public async Task<IActionResult> Index()
        {
            return View(await _context.CurrentReservation.ToListAsync());
        }

        // GET: CurrentReservations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var currentReservation = await _context.CurrentReservation
                .FirstOrDefaultAsync(m => m.Id == id);
            if (currentReservation == null)
            {
                return NotFound();
            }

            return View(currentReservation);
        }

        // GET: CurrentReservations/Create
        public IActionResult Create(int? id)
        {
            return View();
        }

        // POST: CurrentReservations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AccountId,StartDate,EndDate")] CurrentReservation currentReservation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(currentReservation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(currentReservation);
        }

        // GET: CurrentReservations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var currentReservation = await _context.CurrentReservation.FindAsync(id);
            if (currentReservation == null)
            {
                return NotFound();
            }
            return View(currentReservation);
        }

        // POST: CurrentReservations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AccountId,StartDate,EndDate")] CurrentReservation currentReservation)
        {
            if (id != currentReservation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(currentReservation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CurrentReservationExists(currentReservation.Id))
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
            return View(currentReservation);
        }

        // GET: CurrentReservations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var currentReservation = await _context.CurrentReservation
                .FirstOrDefaultAsync(m => m.Id == id);
            if (currentReservation == null)
            {
                return NotFound();
            }

            return View(currentReservation);
        }

        // POST: CurrentReservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var currentReservation = await _context.CurrentReservation.FindAsync(id);
            _context.CurrentReservation.Remove(currentReservation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CurrentReservationExists(int id)
        {
            return _context.CurrentReservation.Any(e => e.Id == id);
        }
    }
}
