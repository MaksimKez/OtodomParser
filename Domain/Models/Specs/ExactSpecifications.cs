using Domain.Models.Enums;

namespace Domain.Models.Specs;

public class ExactSpecifications : DefaultSpecifications
{
    public IEnumerable<FloorNumber>? FloorAccepted { get; set; }
    public IEnumerable<EstateType>? EstateTypesAccepted { get; set; }
    public AvailabilityFrom AvailableFrom { get; set; }
    public int? FloorsMin { get; set; }
    public int? FloorsMax { get; set; }
    public int? YearBuiltMin { get; set; }
    public int? YearBuiltMax { get; set; }
}
