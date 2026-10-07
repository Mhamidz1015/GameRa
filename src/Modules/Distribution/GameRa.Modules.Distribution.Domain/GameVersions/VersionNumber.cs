using System.Globalization;

namespace GameRa.Modules.Distribution.Domain.GameVersions;

public readonly record struct VersionNumber : IComparable<VersionNumber>
{
    private VersionNumber(int major, int minor, int patch)
    {
        Major = major;
        Minor = minor;
        Patch = patch;
    }

    public int Major { get; }

    public int Minor { get; }

    public int Patch { get; }

    public static bool TryParse(string? value, out VersionNumber version)
    {
        version = default;

        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }

        string[] parts = value.Split('.');

        if (parts.Length != 3 ||
            !TryParsePart(parts[0], out int major) ||
            !TryParsePart(parts[1], out int minor) ||
            !TryParsePart(parts[2], out int patch))
        {
            return false;
        }

        version = new VersionNumber(major, minor, patch);

        return true;
    }

    internal static VersionNumber From(int major, int minor, int patch) => new(major, minor, patch);

    public int CompareTo(VersionNumber other)
    {
        int result = Major.CompareTo(other.Major);

        if (result != 0)
        {
            return result;
        }

        result = Minor.CompareTo(other.Minor);

        return result != 0 ? result : Patch.CompareTo(other.Patch);
    }

    public static bool operator <(VersionNumber left, VersionNumber right) => left.CompareTo(right) < 0;

    public static bool operator >(VersionNumber left, VersionNumber right) => left.CompareTo(right) > 0;

    public static bool operator <=(VersionNumber left, VersionNumber right) => left.CompareTo(right) <= 0;

    public static bool operator >=(VersionNumber left, VersionNumber right) => left.CompareTo(right) >= 0;

    public override string ToString() => $"{Major}.{Minor}.{Patch}";

    private static bool TryParsePart(string part, out int value)
    {
        value = 0;

        if (part.Length == 0 || part.Any(c => !char.IsAsciiDigit(c)) || (part.Length > 1 && part[0] == '0'))
        {
            return false;
        }

        return int.TryParse(part, NumberStyles.None, CultureInfo.InvariantCulture, out value);
    }
}
