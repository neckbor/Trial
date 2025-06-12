namespace Domain.Entities.Dictionaries;

public class City
{
    public int Id { get; private set; }
    public string IATACode { get; private set; }
    public string Name { get; private set; }

    public int CountryId { get; private set; }
    public virtual Country Country { get; private set; }
}
