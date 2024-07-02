using Microsoft.AspNetCore.Mvc.RazorPages;
using ParcelManagementApp.Data;
using ParcelManagementApp.Models;

namespace ParcelManagementApp.Pages.Parcels
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Parcel Parcel { get; set; }

        public async Task OnGetAsync(int? id)
        {
            if (id == null)
            {
                return;
            }

            Parcel = await _context.Parcels.FindAsync(id);
        }
    }
}
