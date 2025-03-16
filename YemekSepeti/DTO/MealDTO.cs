using System.Text.Json.Serialization;
using YemekSepeti.Models;

namespace YemekSepeti.DTO
{
    public class MealDTO
    {
        public string Name { get; set; } = null!;

        public decimal Price { get; set; }

        public string Description { get; set; } = null!;

        public bool IsAvailable { get; set; }

        public string Image { get; set; } = null!;
        public int Quantity { get; set; }

        public int RestaurantId { get; set; }

        public virtual ICollection<Category> Categories { get; set; } = new List<Category>();

        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
