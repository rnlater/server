using System;

namespace Shared.Utils;

public class TypeConverter
{
    public static TEnum? StringToEnum<TEnum>(string? value) where TEnum : struct
    {
        if (Enum.TryParse<TEnum>(value, true, out var result))
        {
            return result;
        }
        return null;
    }
}
