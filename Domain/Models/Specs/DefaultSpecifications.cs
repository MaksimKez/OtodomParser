using Domain.Enums;

namespace Domain.Models.Specs;

public class DefaultSpecifications : BaseSpecifications
{
    public int? PriceMin { get; set; }
    public int? PriceMax { get; set; }
    public int? AreaMin { get; set; }
    public int? AreaMax { get; set; }
    public IEnumerable<RoomNumber>? RoomNumber { get; set; }
    public IEnumerable<FloorNumber>? Floors { get; set; }
}
