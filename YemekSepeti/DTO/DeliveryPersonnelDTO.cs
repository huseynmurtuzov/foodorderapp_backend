using YemekSepeti.DTO;
using YemekSepeti.Models;

namespace YemekSepeti.DTO
{
    public class DeliveryPersonnelDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string VeichleType { get; set; } = null!;
        public List<DeliveryOrderDTO> Orders { get; set; } = new List<DeliveryOrderDTO>();
        public int TotalOrders { get; set; }
    }
}
public class DeliveryOrderDTO
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    public string Status { get; set; }
    public decimal TotalAmount { get; set; }
    public int CustomerId { get; set; }

    public int RestaurantId { get; set; }
    public string Name { get; set; }
    public List<string> Meals { get; set; }
    public string OrderCustomerName { get; set; }
    public string OrderCustomerPhoneNumber { get; set; }
    public string OrderCustomerAddress { get; set; }
    public string OrderRestaurantName { get; set; }
    public string OrderRestaurantPhoneNumber { get; set; }

}