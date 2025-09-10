using System.Text;
using Application.Services.ChainOfSpecHandlers.Bases;
using Domain.Enums;
using Domain.Models.Specs;

namespace Application.Services.ChainOfSpecHandlers;

public class BaseSpecificationsHandler : SpecHandlerBase
{
    public override StringBuilder Handle(object spec, StringBuilder sb)
    {
        if (spec is not BaseSpecifications baseSpec) return base.Handle(spec, sb);
        
        if (baseSpec.TransactionType is not null) sb.Append($"{HandleTransactionType(baseSpec.TransactionType)}/");
        
        if (baseSpec.EstateType is not null) sb.Append($"{HandleEstateType(baseSpec.EstateType)}/");
        
        if (baseSpec.Localization is not null)
            sb.Append($"{HandleLocalization(baseSpec.Localization)}/");
        else
            sb.Append("cala-polska?");
        
        if (baseSpec.DaysSinceCreated is not null)
            sb.Append($"daysSinceCreated={baseSpec.DaysSinceCreated}&");

        return base.Handle(spec, sb);
    }

    private static string HandleLocalization(string localization)
    {
        //todo map for cities
        return "mazowieckie/warszawa";
    }

    private static string HandleTransactionType(TransactionType? transactionType) =>
        transactionType switch
        {
            TransactionType.RENT => "wynajem",
            TransactionType.SALE => "sprzedaz",
            _ => "wynajem"
        };

    private static string HandleEstateType(EstateType? estateType) =>
        estateType switch
        {
            EstateType.FLAT => "mieszkanie",
            EstateType.ROOM => "pokoj",
            EstateType.PLOT => "mieszkanie",
            EstateType.GARAGE => "gararz",
            EstateType.WAREHOUSE => "wynajem",
            EstateType.COMMERCIAL => "wynajem",
            _ => "mieszkanie"
        };
}