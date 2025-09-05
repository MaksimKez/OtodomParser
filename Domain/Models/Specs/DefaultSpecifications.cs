using Domain.Models.Enums;

namespace Domain.Models.Specs;

public class DefaultSpecifications
{
    public TransactionType? T { get; set; }
    public string? Localization { get; set; }
    public int? PriceMin { get; set; }
    public int? PriceMax { get; set; }
    public int? AreaMin { get; set; }
    public int? AreaMax { get; set; }
    public RoomNumber? RoomNumber { get; set; }
}
