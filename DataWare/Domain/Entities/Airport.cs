namespace Domain.Entities;

public class Airport
{
    public long Id { get; private set; }
    public string IATACode { get; private set; }
    public string Name { get; private set; }

    public int CityId { get; private set; }
    public virtual City City { get; private set; }
}
