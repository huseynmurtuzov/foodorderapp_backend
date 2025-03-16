using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace YemekSepeti.Models;

public partial class Customer
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string Password { get; set; } = null!;
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<RestaurantReview> RestaurantReviews { get; set; } = new List<RestaurantReview>();

    public virtual ICollection<Restaurant> Restaurants { get; set; } = new List<Restaurant>();
}
