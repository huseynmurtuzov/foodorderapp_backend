using System;
using System.Collections.Generic;

namespace YemekSepeti.Models;

public partial class Payment
{
    public int Id { get; set; }

    public string PaymentMethod { get; set; } = null!;

    public DateTime PaymentDate { get; set; }

    public decimal Amount { get; set; }

    public bool IsSuccessful { get; set; }

    public int OrderId { get; set; }

    public virtual Order Order { get; set; } = null!;
}
