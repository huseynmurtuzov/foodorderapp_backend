 using System;
using System.Collections.Generic;

namespace YemekSepeti.Models;

public partial class RestaurantReview
{
    public int Id { get; set; }

    public int Rating { get; set; }

    public string Comment { get; set; } = null!;

    public DateTime ReviewDate { get; set; }

    public int CustomerId { get; set; }

    public int RestaurantId { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual Restaurant Restaurant { get; set; } = null!;
}
