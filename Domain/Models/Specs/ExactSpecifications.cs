using Domain.Enums;

namespace Domain.Models.Specs;

public class ExactSpecifications : DefaultSpecifications
{
    public AvailabilityFrom? AvailableFrom { get; set; }
    public int? FloorsMin { get; set; }
    public int? FloorsMax { get; set; }
    public int? YearBuiltMin { get; set; }
    public int? YearBuiltMax { get; set; }
}
