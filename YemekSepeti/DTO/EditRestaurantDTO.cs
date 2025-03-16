using YemekSepeti.Models;
namespace YemekSepeti.DTO
{
    public class EditRestaurantDTO
    {
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string WorkingHours { get; set; } = null!;
        public string Image { get; set; } = null!;
    }
}
