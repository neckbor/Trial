using Domain.Errors;
using Domain.Shared;

namespace Domain.Entities.Dictionaries;

public class SearchStatus
{
    public static readonly SearchStatus Pending = new(1, "", "");
    public static readonly SearchStatus Completed = new(2, "", "");
    public static readonly SearchStatus Failed = new(3, "", "");

    public int Id { get; private set; }
    public string Code { get; private set; }
    public string Name { get; private set; }

    private SearchStatus(int id, string code, string name)
    {
        Id = id;
        Code = code;
        Name = name;
    }

    private static readonly Dictionary<int, SearchStatus> _byId = new Dictionary<int, SearchStatus>()
    {
        { Pending.Id, Pending },
        { Completed.Id, Completed },
        { Failed.Id, Failed },
    };

    public static Result<SearchStatus> FromId(int id)
    {
        return _byId.TryGetValue(id, out var result) ? result
               : Result.Failure<SearchStatus>(DomainErrors.SearchStatus.NotFound);
    }
}
