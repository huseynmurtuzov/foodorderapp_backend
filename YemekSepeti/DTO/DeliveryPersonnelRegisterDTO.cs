using YemekSepeti.Models;

namespace YemekSepeti.DTO
{
    public class DeliveryPersonnelRegisterDTO
    {
        public string Name { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;

        public string VeichleType { get; set; } = null!;

        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
