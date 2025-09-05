using System.Text;
using Application.Services.Bases;
using Domain.Models.Enums;
using Domain.Models.Specs;

namespace Application.Services;

public class BaseSpecificationsHandler : SpecHandlerBase
{
    public override StringBuilder Handle(object spec, StringBuilder sb)
    {//todo make localization/transaction type handling
        if (spec is not BaseSpecifications baseSpec) return base.Handle(spec, sb);

        if (baseSpec.TransactionType is not null) sb.Append($"{baseSpec.TransactionType}/");
        
        if (baseSpec.EstateType is not null) sb.Append($"{baseSpec.EstateType}/");
        
        if (baseSpec.Localization is not null)
            sb.Append($"{baseSpec.Localization}?");
        else
            sb.Append("cala-polska?");
        
        if (baseSpec.DaysSinceCreated is not null)
            sb.Append($"daysSinceCreated={baseSpec.DaysSinceCreated}&");

        return base.Handle(spec, sb);
    }
}