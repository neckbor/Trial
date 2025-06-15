using Domain.Errors;
using Domain.Shared;

namespace Domain.Entities.Dictionaries;

public class BookingStatus
{
    public static readonly BookingStatus Created = new(1, "CREATED", "Создано");
    public static readonly BookingStatus Pending = new(2, "PENDING", "В обработке");
    public static readonly BookingStatus Booked = new(3, "BOOKED", "Забронировано");
    public static readonly BookingStatus Failed = new(4, "FAILED", "Ошибка");

    public int Id { get; private set; }
    public string Code { get; private set; }
    public string Name { get; private set; }

    private BookingStatus() { }

    private BookingStatus(int id, string code, string name)
    {
        Id = id;
        Code = code;
        Name = name;
    }

    private static readonly Dictionary<int, BookingStatus> _byId = new Dictionary<int, BookingStatus>()
    {
        { Created.Id, Created },
        { Pending.Id, Pending },
        { Booked.Id, Booked },
        { Failed.Id, Failed },
    };

    public static Result<BookingStatus> FromId(int id)
    {
        return _byId.TryGetValue(id, out var result) ? result
               : Result.Failure<BookingStatus>(DomainErrors.BookingStatus.NotFound);
    }

    public static IEnumerable<BookingStatus> GetAll() => _byId.Values.ToList();
}
