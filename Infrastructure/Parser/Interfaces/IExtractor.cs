namespace Infrastructure.Parser.Interfaces;

public interface IExtractor
{
    int ExtractFloor(string description);
    bool ExtractIsFurnished(string description);
    bool ExtractPets(string description);
    bool ExtractBalcony(string description);
    bool ExtractAppliances(string description);
}
