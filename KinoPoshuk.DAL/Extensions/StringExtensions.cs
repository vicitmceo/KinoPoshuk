namespace KinoPoshuk.DAL.Extensions;

public static class StringExtensions
{
    public static string TrimToLength(this string value, int maxLength)
    {
        return value.Length <= maxLength ? value : value[..maxLength];
    }

    public static bool IsBlank(this string? value)
    {
        return string.IsNullOrWhiteSpace(value);
    }
}
