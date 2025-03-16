namespace YemekSepeti.DTO
{
    public class RestaurantReviewDTO
    {
        public int Rating { get; set; }

        public string Comment { get; set; } = null!;
        public int CustomerId { get; set; }

        public int RestaurantId { get; set; }
    }
}
