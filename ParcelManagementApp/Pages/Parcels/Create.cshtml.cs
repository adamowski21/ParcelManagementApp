using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ParcelManagementApp.Data;
using ParcelManagementApp.Models;
using System.Security.Claims;
using System.ComponentModel.DataAnnotations;

namespace ParcelManagementApp.Pages.Parcels
{
    public class CreateModel : PageModel
    {
        private readonly ILogger<CreateModel> _logger;
        private readonly ApplicationDbContext _context;

        public CreateModel(ILogger<CreateModel> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [BindProperty]
        public ParcelDto Parcel { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            _logger.LogInformation("OnPostAsync called");

            if (!ModelState.IsValid)
            {
                foreach (var modelStateKey in ModelState.Keys)
                {
                    var modelStateVal = ModelState[modelStateKey];
                    if (modelStateVal.Errors.Any())
                    {
                        foreach (var error in modelStateVal.Errors)
                        {
                            _logger.LogWarning($"ModelState Error: {modelStateKey} - {error.ErrorMessage}");
                        }
                    }
                }
                return Page();
            }

            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogError("User ID not found in claims.");
                return BadRequest("User ID not found in claims.");
            }

            Parcel newParcel = new Parcel
            {
                Name = Parcel.Name,
                Address = Parcel.Address,
                Size = Parcel.Size,
                Supplier = Parcel.Supplier,
                Status = "Sent",
                Location = "Warehouse",
                Price = Parcel.Size == "S" ? 5 : Parcel.Size == "M" ? 10 : 20,
                UserId = userId
            };

            await _context.Parcels.AddAsync(newParcel);
            await _context.SaveChangesAsync();

            ModelState.Clear();

            // TempData is used to pass data to the next request
            TempData["ParcelName"] = newParcel.Name;
            TempData["ParcelStatus"] = newParcel.Status;
            TempData["ParcelLocation"] = newParcel.Location;

            _logger.LogInformation("Parcel created successfully");
            return RedirectToPage("./Index");
        }
    }

    public class ParcelDto
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Size is required")]
        public string Size { get; set; }

        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Supplier is required")]
        public string Supplier { get; set; }
    }

}
