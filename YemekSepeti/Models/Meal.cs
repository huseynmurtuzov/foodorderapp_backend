using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace YemekSepeti.Models;

public partial class Meal
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public decimal Price { get; set; }

    public string Description { get; set; } = null!;

    public bool IsAvailable { get; set; }

    public string Image { get; set; } = null!;
    public int Quantity { get; set; } 

    public int RestaurantId { get; set; }

    [JsonIgnore]
    public virtual Restaurant Restaurant { get; set; } = null!;

    public virtual ICollection<Category> Categories { get; set; } = new List<Category>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
