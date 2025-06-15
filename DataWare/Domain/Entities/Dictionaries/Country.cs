using Domain.Primitives;

namespace Domain.Entities.Dictionaries;

public class Country : Entity<int>
{
    public static readonly Country France = new(1, "FR", "Франция");
    public static readonly Country USA = new(2, "USA", "США");
    public static readonly Country UK = new(3, "UK", "Великобритания");
    public static readonly Country Georgia = new(4, "GEO", "Грузия");
    public static readonly Country UAE = new(5, "UAE", "ОАЭ");

    public string Code { get; private set; }
    public string Name { get; private set; }

    private Country(int id, string code, string name)
    {
        Id = id;
        Code = code;
        Name = name;
    }
}
