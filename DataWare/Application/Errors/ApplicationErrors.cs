using Domain.Shared;

namespace Application.Errors;

internal static class ApplicationErrors
{
    public static class General
    {
        public static readonly Error Unexpected = Error.Failure(
            "General.Unexpected",
            "Во время работы приложения произошла непредвиденная ошибка.");
    }
}
