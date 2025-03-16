using YemekSepeti.Models;

namespace YemekSepeti.DTO
{
    public class OrderDTO
    {
        public DateTime OrderDate { get; set; }

        public decimal TotalAmount { get; set; }

        public string Status { get; set; } = null!;

        public int CustomerId { get; set; }

        public int RestaurantId { get; set; }

        public int DeliveryPersonelId { get; set; }
        public string PaymentMethod { get; set; }
        public ICollection<MealDTO> Meals { get; set; } = new List<MealDTO>();
    }
}
