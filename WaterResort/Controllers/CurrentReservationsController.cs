using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<IdentityUser> _userManager;

        public CurrentReservationsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: CurrentReservations
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.CurrentReservations.ToListAsync());
        }

        public async Task<IActionResult> Confirmation()
        {
            return View();
        }

        // GET: CurrentReservations/Details/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var currentReservation = await _context.CurrentReservations
                .FirstOrDefaultAsync(m => m.Id == id);
            if (currentReservation == null)
            {
                return NotFound();
            }

            return View(currentReservation);
        }


        // GET: CurrentReservations/Create
        [Authorize(Roles = "Administrator, Customer")]
        public IActionResult Create(int? id)
        {
            return View();
        }

        // POST: CurrentReservations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator, Customer")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AccountId,StartDate,EndDate")] CurrentReservation currentReservation, string phoneNumber, string fullName)
        {
            if (ModelState.IsValid)
            {
                _context.Add(currentReservation);
                await _context.SaveChangesAsync();
                if (User.IsInRole("Administrator"))
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return RedirectToAction("Confirmation", currentReservation);
                }
                
            }

            return View(currentReservation);
        }

        // GET: CurrentReservations/Edit/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var currentReservation = await _context.CurrentReservations.FindAsync(id);
            if (currentReservation == null)
            {
                return NotFound();
            }
            return View(currentReservation);
        }

        // POST: CurrentReservations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator")]
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
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var currentReservation = await _context.CurrentReservations
                .FirstOrDefaultAsync(m => m.Id == id);
            if (currentReservation == null)
            {
                return NotFound();
            }

            return View(currentReservation);
        }

        // POST: CurrentReservations/Delete/5
        [Authorize(Roles = "Administrator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var currentReservation = await _context.CurrentReservations.FindAsync(id);
            _context.CurrentReservations.Remove(currentReservation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CurrentReservationExists(int id)
        {
            return _context.CurrentReservations.Any(e => e.Id == id);
        }
    }
}
