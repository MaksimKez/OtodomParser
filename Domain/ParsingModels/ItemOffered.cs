using System.Text.Json.Serialization;
using Domain.Models;

namespace Domain.ParsingModels;

public class ItemOffered
{
    [JsonPropertyName("@type")]
    public string Type { get; set; }

    public string Description { get; set; }

    public Address Address { get; set; }

    public int NumberOfRooms { get; set; }

    public FloorSize FloorSize { get; set; }
}
