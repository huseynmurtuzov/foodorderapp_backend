namespace YemekSepeti.DTO
{
    public class ReviewDTO
    {
        public int Id { get; set; }
        public string Comment { get; set; }
        public int Rating { get; set; }
        public DateTime ReviewDate { get; set; }
        public string CustomerName { get; set; }
        public int CustomerId { get; set; }
    }
}
