using Domain.Primitives;

namespace Domain.Entities.Dictionaries;

public class Airport : Entity<long>
{
    public static readonly Airport CDG = new(1, "CDG", "Аэропорт Шарль-Де-Голь", City.Paris.Id);
    public static readonly Airport LAX = new(2, "LAX", "Международный аэропорт Лос-Анджелес", City.LosAngeles.Id);
    public static readonly Airport DBX = new(3, "DBX", "Международный аэропорт Дубая", City.Dubai.Id);
    public static readonly Airport LHR = new(4, "LHR", "Лондонский аэропорт Хитроу", City.London.Id);
    public static readonly Airport TBS = new(5, "TBS", "Международный аэропорт Тбилиси имени Шота Руставели", City.Tbilisi.Id);

    public string IATACode { get; private set; }
    public string Name { get; private set; }

    public long CityId { get; private set; }
    public virtual City City { get; private set; }

    private Airport() { }

    private Airport(long id, string IATACode, string name, long cityId)
    {
        Id = id;
        this.IATACode = IATACode;
        Name = name;
        CityId = cityId;
    }

    public static IEnumerable<Airport> GetAll() => [CDG, LAX, DBX, LHR, TBS];
}
