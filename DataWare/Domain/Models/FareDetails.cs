namespace Domain.Models;

public class FareDetails
{
    public decimal BaseFare { get; set; }
    public decimal Taxes { get; set; }
    public decimal TotalPrice => BaseFare + Taxes;
}
