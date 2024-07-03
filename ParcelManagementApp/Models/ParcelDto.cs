using System.ComponentModel.DataAnnotations;

namespace ParcelManagementApp.Models
{
    public class ParcelDto
    {
        [Required(ErrorMessage = "Please enter the parcel name.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please select the parcel size.")]
        public string Size { get; set; }

        [Required(ErrorMessage = "Please enter the delivery address.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Please select the supplier.")]
        public string Supplier { get; set; }
    }
}
