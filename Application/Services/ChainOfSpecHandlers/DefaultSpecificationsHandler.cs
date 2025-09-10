using System.Text;
using Application.Services.ChainOfSpecHandlers.Bases;
using Domain.Enums;
using Domain.Models.Specs;

namespace Application.Services.ChainOfSpecHandlers;

public class DefaultSpecificationsHandler : SpecHandlerBase
{
    public override StringBuilder Handle(object spec, StringBuilder sb)
    {
        if (spec is not DefaultSpecifications defSpec) return base.Handle(spec, sb);
        
        if (defSpec.PriceMin is not null)
            sb.Append($"priceMin={defSpec.PriceMin}&");

        if (defSpec.PriceMax is not null)
            sb.Append($"priceMax={defSpec.PriceMax}&");

        if (defSpec.AreaMin is not null)
            sb.Append($"areaMin={defSpec.AreaMin}&");

        if (defSpec.AreaMax is not null)
            sb.Append($"areaMax={defSpec.AreaMax}&");

        if (defSpec.RoomNumber is not null)
        {
            sb.Append($"{defSpec.RoomNumber}&");
        }
        
        return base.Handle(spec, sb);
    }

    private string HandleRoomNumber(IEnumerable<RoomNumber> roomNumbers)
    {
        var sb = new StringBuilder();
        sb.Append("roomsNumber=");

        foreach (var roomCount in roomNumbers)
        {
            switch (roomCount)
            {
                case RoomNumber.ONE: sb.Append("%5BONE"); break;
                case RoomNumber.TWO: sb.Append("%2CTWO"); break;
                case RoomNumber.THREE: sb.Append("%2CTHREE"); break;
                case RoomNumber.FOUR: sb.Append("%2CFOUR"); break;
                case RoomNumber.FIVE_AND_MORE: sb.Append("%2CFIVE%2CSIX_OR_MORE"); break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        sb.Append("%5D");
        return sb.ToString();
    }
}
