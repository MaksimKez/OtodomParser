using System.Text;
using Application.Services.ChainOfSpecHandlers.Bases;
using Domain.Enums;
using Domain.Models.Specs;

namespace Application.Services.ChainOfSpecHandlers;

public class ExactSpecificationsHandler : SpecHandlerBase
{
    public override StringBuilder Handle(object spec, StringBuilder sb)
    {
        if (spec is not ExactSpecifications exactSpec) return base.Handle(spec, sb);
        
        sb.Append($"availableFrom={exactSpec.AvailableFrom}&");

        if (exactSpec.FloorsMin is not null)
            sb.Append($"floorsNumberMin={exactSpec.FloorsMin}&");

        if (exactSpec.FloorsMax is not null)
            sb.Append($"floorsNumberMax={exactSpec.FloorsMax}&");

        if (exactSpec.YearBuiltMin is not null)
            sb.Append($"buildYearMin={exactSpec.YearBuiltMin}&");

        if (exactSpec.YearBuiltMax is not null)
            sb.Append($"buildYearMax={exactSpec.YearBuiltMax}&");

        if (exactSpec.AvailableFrom is not null) sb.Append($"{exactSpec.AvailableFrom}&");

        return base.Handle(spec, sb);
    }

    private string HandleAvailableFrom(AvailabilityFrom availableFrom)
    {
        var sb = new StringBuilder();
        sb.Append("freeFrom=");


        switch (availableFrom)
        {
            case AvailabilityFrom.ASAP: sb.Append("NOW"); break;
            case AvailabilityFrom.MOUNTH: sb.Append("THIRTY_DAYS"); break;
            case AvailabilityFrom.THREEMOUNTH: sb.Append("NINETY_DAYS"); break;
            default:
                throw new ArgumentOutOfRangeException(nameof(availableFrom), availableFrom, null);
        }
        
        return sb.ToString();
    }
}
