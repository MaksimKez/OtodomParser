using Infrastructure.Parser.Interfaces;

namespace Infrastructure.Parser;

public class Extractor : IExtractor
{
    public int ExtractFloor(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
            return 0;

        if (description.Contains("parter", StringComparison.OrdinalIgnoreCase))
            return 0;

        var parts = description.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        foreach (var part in parts)
        {
            if (int.TryParse(part.Replace("I", "1"), out var floor))
                return floor;
        }

        return 0;
    }

    public bool ExtractIsFurnished(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
            return false;

        return description.Contains("umeblowane", StringComparison.OrdinalIgnoreCase)
               || description.Contains("meble", StringComparison.OrdinalIgnoreCase)
               || description.Contains("wyposażone", StringComparison.OrdinalIgnoreCase);
    }

    public bool ExtractPets(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
            return false;

        return description.Contains("zwierzęta", StringComparison.OrdinalIgnoreCase)
               || description.Contains("pies", StringComparison.OrdinalIgnoreCase)
               || description.Contains("kot", StringComparison.OrdinalIgnoreCase);
    }

    public bool ExtractBalcony(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
            return false;

        return description.Contains("balkon", StringComparison.OrdinalIgnoreCase)
               || description.Contains("taras", StringComparison.OrdinalIgnoreCase)
               || description.Contains("loggia", StringComparison.OrdinalIgnoreCase);
    }

    public bool ExtractAppliances(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
            return false;

        return description.Contains("AGD", StringComparison.OrdinalIgnoreCase)
               || description.Contains("pralka", StringComparison.OrdinalIgnoreCase)
               || description.Contains("lodówka", StringComparison.OrdinalIgnoreCase)
               || description.Contains("zmywarka", StringComparison.OrdinalIgnoreCase);
    }
}
