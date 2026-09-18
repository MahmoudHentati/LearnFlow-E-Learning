using System.Globalization;
using System.Text;
using E_Learning.Models.Enums;

namespace E_learning.Models.Helpers;

public static class FormationStatusHelper
{
    public static readonly string[] OrderedValues =
    [
        nameof(StatutFormation.Active),
        nameof(StatutFormation.Inactive),
        nameof(StatutFormation.Archivee)
    ];

    public static string Normalize(string? status)
    {
        var normalizedKey = NormalizeKey(status);

        return normalizedKey switch
        {
            "active" or "actif" => nameof(StatutFormation.Active),
            "inactive" or "inactif" or "inactivee" or "brouillon" => nameof(StatutFormation.Inactive),
            "archive" or "archivee" or "archiver" => nameof(StatutFormation.Archivee),
            "publie" or "publiee" => nameof(StatutFormation.Active),
            _ => string.IsNullOrWhiteSpace(status) ? string.Empty : status.Trim()
        };
    }

    public static string NormalizeOrDefault(string? status, string defaultValue = nameof(StatutFormation.Inactive))
    {
        var normalized = Normalize(status);
        return string.IsNullOrWhiteSpace(normalized) ? defaultValue : normalized;
    }

    public static bool IsActive(string? status)
        => string.Equals(Normalize(status), nameof(StatutFormation.Active), StringComparison.OrdinalIgnoreCase);

    public static string GetDisplayLabel(string? status)
    {
        return Normalize(status) switch
        {
            nameof(StatutFormation.Active) => "Active",
            nameof(StatutFormation.Inactive) => "Inactivé",
            nameof(StatutFormation.Archivee) => "Archivé",
            _ => string.IsNullOrWhiteSpace(status) ? "-" : status.Trim()
        };
    }

    private static string NormalizeKey(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return string.Empty;
        }

        var trimmed = value.Trim().ToLowerInvariant();
        var builder = new StringBuilder(trimmed.Length);

        foreach (var character in trimmed.Normalize(NormalizationForm.FormD))
        {
            if (CharUnicodeInfo.GetUnicodeCategory(character) != UnicodeCategory.NonSpacingMark)
            {
                builder.Append(character);
            }
        }

        return builder
            .ToString()
            .Normalize(NormalizationForm.FormC)
            .Replace(" ", string.Empty, StringComparison.Ordinal);
    }
}
