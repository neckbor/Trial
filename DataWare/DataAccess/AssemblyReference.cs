using System.Reflection;

namespace DataAccess;

internal class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}
