using YemekSepeti.Models;

namespace YemekSepeti.DTO
{
    public class CustomerRegisterDTO
    {
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;

        public string Address { get; set; } = null!;

        public string Password { get; set; } = null!;

        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

        public virtual ICollection<RestaurantReview> RestaurantReviews { get; set; } = new List<RestaurantReview>();

        public virtual ICollection<Restaurant> Restaurants { get; set; } = new List<Restaurant>();
    }
}
