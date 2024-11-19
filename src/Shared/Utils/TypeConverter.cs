namespace Shared.Utils;

public class TypeConverter
{
    /// <summary>
    /// Convert a string to an enum value.
    /// </summary>
    /// <typeparam name="TEnum"></typeparam>
    /// <param name="value"></param>
    /// <returns>return the enum value if the conversion is successful, otherwise return null</returns>
    public static TEnum? StringToEnum<TEnum>(string? value) where TEnum : struct
    {
        if (Enum.TryParse<TEnum>(value, true, out var result))
        {
            return result;
        }
        return null;
    }
}
