namespace Infrastructure.Parser.Interfaces;

public interface IListingUtilities
{
    string BuildAbsoluteUrl(string href, string lang = "pl");
    int MapRooms(string rooms);
    int MapFloor(string floor);
}
