using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ParcelManagementApp.Data;
using ParcelManagementApp.Models;

namespace ParcelManagementApp.Pages.Parcels
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Parcel Parcel { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Parcel = await _context.Parcels.FindAsync(id);

            if (Parcel == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var existingParcel = await _context.Parcels.FindAsync(Parcel.Id);

            if (existingParcel != null)
            {
                existingParcel.Status = Parcel.Status;
                existingParcel.Location = Parcel.Location;

                if (existingParcel.Address == existingParcel.Location)
                {
                    existingParcel.Status = "Delivered";
                }

                _context.Update(existingParcel);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
