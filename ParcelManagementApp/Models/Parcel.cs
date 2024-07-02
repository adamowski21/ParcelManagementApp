using System.ComponentModel.DataAnnotations;

namespace ParcelManagementApp.Models
{
    public class Parcel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter the parcel name.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please select the parcel size.")]
        public string Size { get; set; }

        [Required(ErrorMessage = "Please enter the delivery address.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Please select the supplier.")]
        public string Supplier { get; set; }

        public double Price { get; set; }

        public string Status { get; set; }

        public string Location { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }
    }

}
