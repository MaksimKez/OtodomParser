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
            sb.Append($"{HandleRoomNumber(defSpec.RoomNumber)}&");
        
        if (defSpec.Floors is not null)
            sb.Append($"{HandleFloors(defSpec.Floors)}&");
        
        return base.Handle(spec, sb);
    }

    private string HandleRoomNumber(IEnumerable<RoomNumber> roomNumbers)
    {
        var sb = new StringBuilder();
        sb.Append("roomsNumber=");
        var first = true;
        foreach (var roomCount in roomNumbers ?? throw new ArgumentNullException(nameof(roomNumbers)))
        {
            string token = roomCount! switch
            {
                RoomNumber.ONE => "ONE",
                RoomNumber.TWO => "TWO",
                RoomNumber.THREE => "THREE",
                RoomNumber.FOUR => "FOUR",
                RoomNumber.FIVE_AND_MORE => null, // will add two tokens below
                _ => throw new ArgumentOutOfRangeException()
            } ?? throw new Exception();

            if (roomCount == RoomNumber.FIVE_AND_MORE)
            {
                // FIVE and SIX_OR_MORE
                if (first)
                {
                    sb.Append("%5BFIVE%2CSIX_OR_MORE");
                    first = false;
                }
                else
                {
                    sb.Append("%2CFIVE%2CSIX_OR_MORE");
                }
                continue;
            }

            if (first)
            {
                sb.Append("%5B" + token);
                first = false;
            }
            else
            {
                sb.Append("%2C" + token);
            }
        }
        sb.Append("%5D");
        return sb.ToString();
    }

    private string HandleFloors(IEnumerable<FloorNumber> floors)
    {
        var sb = new StringBuilder();
        sb.Append("floors=");
        var first = true;
        foreach (var floor in floors)
        {
            string token = floor switch
            {
                FloorNumber.GROUND => "GROUND",
                FloorNumber.FIRST => "FIRST",
                FloorNumber.SECOND => "SECOND",
                FloorNumber.THIRD => "THIRD",
                FloorNumber.FOURTH => "FOURTH",
                FloorNumber.FIFTH => "FIFTH",
                FloorNumber.SIXTH => "SIXTH",
                FloorNumber.SEVENTH => "SEVENTH",
                FloorNumber.EIGHTH => "EIGHTH",
                FloorNumber.NINTH => "NINTH",
                FloorNumber.TENTH => "TENTH",
                FloorNumber.HIGHER_THAN_TENTH => "HIGHER_THAN_TENTH",
                _ => throw new ArgumentOutOfRangeException()
            };
            if (first)
            {
                sb.Append("%5B" + token);
                first = false;
            }
            else
            {
                sb.Append("%2C" + token);
            }
        }
        sb.Append("%5D");
        return sb.ToString();
    }
}
