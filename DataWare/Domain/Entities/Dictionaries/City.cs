using Domain.Primitives;

namespace Domain.Entities.Dictionaries;

public class City : Entity<long>
{
    public static readonly City London = new(1, "LCY", "Лондон", Country.UK);
    public static readonly City Paris = new(2, "CDG", "Париж", Country.France);
    public static readonly City Tbilisi = new(3, "TBS", "Тбилиси", Country.Georgia);
    public static readonly City LosAngeles = new(4, "LAX", "Лос Анджелес", Country.USA);
    public static readonly City Dubai = new(5, "DBX", "Дубай", Country.UAE);

    public string IATACode { get; private set; }
    public string Name { get; private set; }

    public int CountryId { get; private set; }
    public virtual Country Country { get; private set; }

    private City(long id, string iATACode, string name, Country country)
    {
        IATACode = iATACode;
        Name = name;
        Country = country;
    }

    public static IEnumerable<City> GetAll() => [London, Paris, Tbilisi, LosAngeles, Dubai];
}
