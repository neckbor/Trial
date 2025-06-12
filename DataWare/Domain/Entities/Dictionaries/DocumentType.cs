using Domain.Errors;
using Domain.Shared;

namespace Domain.Entities.Dictionaries;

public class DocumentType
{
    public static readonly DocumentType InternalPassport = new(1, "INTERNAL_PASSPORT", "Внутренний паспорт");
    public static readonly DocumentType InternationalPassport = new(2, "INTERNATIONAL_PASSPORT", "Заграничный паспорт");
    public static readonly DocumentType IdCard = new(3, "ID_CARD", "Удостоверение личности");
    public static readonly DocumentType ForeignPassport = new(4, "FOREIGN_PASSPORT", "Иностранный паспорт");

    public int Id { get; private set; }
    public string Code { get; private set; }
    public string Name { get; private set; }

    private DocumentType(int id, string code, string name)
    {
        Id = id;
        Code = code;
        Name = name;
    }

    private static readonly Dictionary<int, DocumentType> _byId = new Dictionary<int, DocumentType>()
    {
        { InternalPassport.Id, InternalPassport },
        { InternalPassport.Id, InternationalPassport },
        { IdCard.Id, IdCard },
        { ForeignPassport.Id, ForeignPassport },
    };

    public static Result<DocumentType> FromId(int id)
    {
        return _byId.TryGetValue(id, out var result) ? result 
            : Result.Failure<DocumentType>(DomainErrors.DocumentType.NotFound);
    }
}
