using System;
using System.Collections.Generic;

namespace YemekSepeti.Models;

public partial class Category
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Meal> Meals { get; set; } = new List<Meal>();

    public virtual ICollection<Restaurant> Restaurants { get; set; } = new List<Restaurant>();
}
