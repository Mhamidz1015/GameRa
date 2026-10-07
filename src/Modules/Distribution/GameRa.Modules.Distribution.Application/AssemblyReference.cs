using System.Reflection;

namespace GameRa.Modules.Distribution.Application;

public static class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}
