using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace YemekSepeti.Models;

public partial class Restaurant
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;

    public string WorkingHours { get; set; } = null!;

    public double Rating { get; set; }

    public string Image { get; set; } = null!;
    public string Password { get; set; } = null!;

    public virtual ICollection<Meal> Meals { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<RestaurantReview> RestaurantReviews { get; set; } = new List<RestaurantReview>();

    public virtual ICollection<Category> Categories { get; set; } = new List<Category>();

    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();
}
