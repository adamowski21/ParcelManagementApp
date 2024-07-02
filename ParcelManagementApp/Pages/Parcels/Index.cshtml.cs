using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ParcelManagementApp.Data;
using ParcelManagementApp.Models;
using System.Security.Claims;

namespace ParcelManagementApp.Pages.Parcels
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Parcel> Parcels { get; set; }

        public async Task OnGetAsync()
        {
            Parcels = await _context.Parcels
                .Where(p => p.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier))
                .ToListAsync();
        }
    }
}
