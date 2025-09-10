using Domain.Enums;

namespace Domain.Models.Specs;

//idea behind of this is to make handlers (query creator) for each spec type
// therefore there is no need in checking all fields from full-spec, if only 2-3 base specs are needed

public class BaseSpecifications
{
    public EstateType? EstateType { get; set; }
    public string? Localization { get; set; }
    public TransactionType? TransactionType { get; set; }
    public int? DaysSinceCreated { get; set; }
}
