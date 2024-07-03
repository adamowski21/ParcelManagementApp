using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParcelManagementApp.Data;
using ParcelManagementApp.Models;

namespace ParcelManagementApp.Controllers
{
    [Authorize]
    public class ParcelsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ParcelsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Parcels
        public async Task<IActionResult> Index()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userParcels = _context.Parcels.Include(p => p.User).Where(p => p.UserId == userId);
            return View(await userParcels.ToListAsync());
        }

        // GET: Parcels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parcel = await _context.Parcels
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.Id == id && m.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (parcel == null)
            {
                return NotFound();
            }

            return View(parcel);
        }

        // GET: Parcels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Parcels/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Size,Address,Supplier")] ParcelDto parcelDto)
        {
            if (ModelState.IsValid)
            {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                Parcel newParcel = new Parcel
                {
                    Name = parcelDto.Name,
                    Address = parcelDto.Address,
                    Size = parcelDto.Size,
                    Supplier = parcelDto.Supplier,
                    Status = "Sent",
                    Location = "Warehouse",
                    Price = parcelDto.Size == "S" ? 5 : parcelDto.Size == "M" ? 10 : 20,
                    UserId = userId
                };

                _context.Add(newParcel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(parcelDto);
        }

        // GET: Parcels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parcel = await _context.Parcels.FirstOrDefaultAsync(p => p.Id == id && p.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (parcel == null)
            {
                return NotFound();
            }

            var editDto = new EditParcelDto
            {
                Id = parcel.Id,
                Status = parcel.Status
            };

            return View(editDto);
        }

        // POST: Parcels/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditParcelDto editDto)
        {
            if (id != editDto.Id)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var existingParcel = await _context.Parcels.FirstOrDefaultAsync(p => p.Id == id && p.UserId == userId);
            if (existingParcel == null)
            {
                return NotFound();
            }

            existingParcel.Status = editDto.Status;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(existingParcel);
                    await _context.SaveChangesAsync();
                    Console.WriteLine("Parcel status updated successfully.");
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ParcelExists(existingParcel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return View(editDto);
        }

        // GET: Parcels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parcel = await _context.Parcels
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.Id == id && m.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (parcel == null)
            {
                return NotFound();
            }

            return View(parcel);
        }

        // POST: Parcels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var parcel = await _context.Parcels.FirstOrDefaultAsync(p => p.Id == id && p.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (parcel != null)
            {
                _context.Parcels.Remove(parcel);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Parcels/Track
        [AllowAnonymous]
        public IActionResult Track()
        {
            return View();
        }

        // POST: Parcels/Track
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Track(int? id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = "Please provide a valid parcel ID.";
                return RedirectToAction(nameof(Track));
            }

            var parcel = await _context.Parcels
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (parcel == null)
            {
                TempData["ErrorMessage"] = "Parcel not found.";
                return RedirectToAction(nameof(Track));
            }

            return View("TrackingResult", parcel);
        }

        // GET: Parcels/UpdateLocation/5
        [HttpGet]
        public async Task<IActionResult> UpdateLocation(int id)
        {
            var parcel = await _context.Parcels.FindAsync(id);
            if (parcel == null)
            {
                return NotFound();
            }

            return View(parcel);
        }

        // POST: Parcels/UpdateLocation/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateLocation(int id, string newLocation)
        {
            var parcel = await _context.Parcels.FindAsync(id);
            if (parcel == null)
            {
                return NotFound();
            }

            parcel.Location = newLocation; // Update the location
            await _context.SaveChangesAsync();

            TempData["Message"] = "Parcel location updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        private bool ParcelExists(int id)
        {
            return _context.Parcels.Any(e => e.Id == id);
        }
    }
}
