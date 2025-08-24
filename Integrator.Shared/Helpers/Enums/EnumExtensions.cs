using System.ComponentModel.DataAnnotations;

namespace Integrator.Shared.Helpers.Enums;
public static class EnumExtensions
{
    public static string GetDisplayName(this Enum enumValue)
    {
        var field = enumValue.GetType().GetField(enumValue.ToString());
        var attribute = (DisplayAttribute)Attribute.GetCustomAttribute(field, typeof(DisplayAttribute));
        return attribute?.Name ?? enumValue.ToString();
    }
}
