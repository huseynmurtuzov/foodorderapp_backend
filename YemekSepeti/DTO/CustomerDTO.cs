using YemekSepeti.Models;

namespace YemekSepeti.DTO
{
    public class CustomerDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Address { get; set; } = null!;
        public virtual ICollection<OrderDTO> Orders { get; set; } = new List<OrderDTO>();
        public virtual ICollection<ReviewDTO> RestaurantReviews { get; set; } = new List<ReviewDTO>();
        public int OrderCount { get; set; }
        public decimal TotalSpending { get; set; }
    }
}
