using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace YemekSepeti.Models;

public partial class Order
{
    public int Id { get; set; }

    public DateTime OrderDate { get; set; }

    public decimal TotalAmount { get; set; }

    public string Status { get; set; } = null!;

    public int CustomerId { get; set; }

    public int RestaurantId { get; set; }

    public int DeliveryPersonelId { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual DeliveryPersonnel DeliveryPersonel { get; set; } = null!;

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual Restaurant Restaurant { get; set; } = null!;

    public virtual ICollection<Meal> Meals { get; set; } = new List<Meal>();
}
