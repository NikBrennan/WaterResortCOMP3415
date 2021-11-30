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
            ViewData["Users"] = _userManager;
            ViewData["_context"] =  _context;
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
        public async Task<IActionResult> Create(int? id)
        {
            ViewData["Room"] = await _context.Rooms.FirstOrDefaultAsync(m => m.Id == id);
            return View();
        }

        // POST: CurrentReservations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator, Customer")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AccountId,StartDate,EndDate,TotalCost,RoomId")] CurrentReservation currentReservation)
        {
            var room = await _context.Rooms.FirstOrDefaultAsync(m => m.Id == currentReservation.RoomId);
            if (User.IsInRole("Customer"))
            {
                if (room.Reserved == true)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            if (ModelState.IsValid)
            {
                _context.Add(currentReservation);

                var AR = await _context.AccountsReceivables.FirstOrDefaultAsync(m => m.AccountId == currentReservation.AccountId) as AccountsReceivable;

                //Check if the user already has a standing balance.
                if(AR != null)
                {
                    AR.Balance = AR.Balance + currentReservation.TotalCost;
                }
                else
                {
                    AccountsReceivable accountsReceivable = new AccountsReceivable();
                    accountsReceivable.AccountId = currentReservation.AccountId;
                    accountsReceivable.Balance = currentReservation.TotalCost;
                    _context.AccountsReceivables.Add(accountsReceivable);
                }
                
                //Check if the reservation starting date is today
                if (currentReservation.StartDate == DateTime.Today)
                {
                    
                    room.Reserved = true;
                    room.AccountId = currentReservation.AccountId;
                }


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

            ViewData["Room"] = await _context.Rooms.FirstOrDefaultAsync(m => m.Id == currentReservation.RoomId);

            
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,AccountId,StartDate,EndDate,TotalCost,RoomId")] CurrentReservation currentReservation)
        {
            if (id != currentReservation.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    await DeleteConfirmed(id);

                    var room = await _context.Rooms.FirstOrDefaultAsync(m => m.Id == currentReservation.RoomId);

                    CurrentReservation cr = new CurrentReservation();
                    cr.AccountId = currentReservation.AccountId;
                    cr.StartDate = currentReservation.StartDate;
                    cr.EndDate = currentReservation.EndDate;
                    cr.TotalCost = (decimal)(currentReservation.EndDate - currentReservation.StartDate).TotalDays * room.CostPerNight;
                    cr.RoomId = currentReservation.RoomId;

                    if(cr.StartDate <= DateTime.Today && cr.EndDate >= DateTime.Today)
                    {
                        room.Reserved = true;
                        room.AccountId = currentReservation.AccountId;
                    } else
                    {
                        room.Reserved = false;
                        room.AccountId = null;
                    }

                    await Create(cr);

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

            var room = await _context.Rooms.FirstOrDefaultAsync(m => m.Id == currentReservation.RoomId);

            //Update the balance in the accounts receivables
            var AR = await _context.AccountsReceivables.FirstOrDefaultAsync(m =>
                m.AccountId == currentReservation.AccountId) as AccountsReceivable;
            var totalDays = (currentReservation.EndDate - currentReservation.StartDate).TotalDays;
            var totalCost = (decimal)totalDays * room.CostPerNight;
            AR.Balance = AR.Balance - totalCost;

            //Remove reservation status of room and user's accountId if the room is currently reserved
            if (room.Reserved == true)
            {
                room.Reserved = false;
                room.AccountId = null;
            }

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
