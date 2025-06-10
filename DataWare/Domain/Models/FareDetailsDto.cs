namespace Domain.Models;

public class FareDetailsDto
{
    public string FareCode { get; set; }
    public string FareType { get; set; }
    public decimal BaseFare { get; set; }
    public decimal Taxes { get; set; }
    public decimal TotalPrice => BaseFare + Taxes;
}
