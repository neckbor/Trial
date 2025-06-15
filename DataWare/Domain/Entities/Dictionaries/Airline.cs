using Domain.Primitives;

namespace Domain.Entities.Dictionaries;

public class Airline : Entity<int>
{
    public static readonly Airline AirFrance = new(1, "AF", "AFR", "AirFrance", Country.France);
    public static readonly Airline BritishAirways = new(2, "BA", "BAW", "British Airways", Country.UK);
    public static readonly Airline Emirates = new(3, "EK", "UAE", "Emirates", Country.UAE);

    public string IATACode { get; private set; }
    public string ICAOCode { get; private set; }
    public string Name { get; private set; }

    public virtual Country Country { get; private set; }

    private Airline(int id, string iATACode, string iCAOCode, string name, Country country)
    {
        Id = id;
        IATACode = iATACode;
        ICAOCode = iCAOCode;
        Name = name;
        Country = country;
    }
}
