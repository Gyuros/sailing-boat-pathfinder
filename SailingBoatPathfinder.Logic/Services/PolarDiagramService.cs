using SailingBoatPathfinder.Data.Models;
using SailingBoatPathfinder.Logic.Models;

namespace SailingBoatPathfinder.Logic.Services;

public class PolarDiagramService
{
    public double GetBoatSpeed(double windAngle, double windSpeed, SailingBoat boat)
    {
        List<IGrouping<double, SailingBoatPolarData>> groupedByWindSpeed = boat.PolarData.GroupBy(data => data.WindVelocity).ToList();
        List<IGrouping<double, SailingBoatPolarData>> left = groupedByWindSpeed.Where(group => group.Key <= windSpeed).ToList();
        List<IGrouping<double, SailingBoatPolarData>> right = groupedByWindSpeed.Where(group => group.Key >= windSpeed).ToList();

        SailingBoatPolarData? topLeft = left.LastOrDefault()?.LastOrDefault(data => data.WindAngle <= windAngle);
        SailingBoatPolarData? bottomLeft = left.LastOrDefault()?.FirstOrDefault(data => data.WindAngle >= windAngle);
        SailingBoatPolarData? topRight = right.FirstOrDefault()?.LastOrDefault(data => data.WindAngle <= windAngle);
        SailingBoatPolarData? bottomRight = right.FirstOrDefault()?.FirstOrDefault(data => data.WindAngle >= windAngle);

        // if (windAngle < 46 && windAngle > 45)
        // {
        //     ;
        // }

        if (topLeft == null || bottomLeft == null)
        {
            return 0;
        }

        topRight ??= topLeft;
        bottomRight ??= bottomLeft;

        if (
            topLeft.DataType == PolarDataType.Inner &&
            bottomLeft.DataType == PolarDataType.Inner &&
            topRight.DataType == PolarDataType.Inner &&
            bottomRight.DataType == PolarDataType.Inner
        )
        {
            return BilinearInterpolation(topLeft, topRight, bottomRight, bottomLeft, windSpeed, windAngle) * 0.51;
        }

        if (!IsInsideLimits(topLeft, topRight, bottomRight, bottomLeft, windSpeed, windAngle))
        {
            return 0;
        }

        return NearestNeighbourInterpolation(topLeft, topRight, bottomRight, bottomLeft, windSpeed, windAngle) * 0.51;
    }

    public bool IsInsideLimits(
        SailingBoatPolarData topLeft,
        SailingBoatPolarData topRight,
        SailingBoatPolarData bottomRight,
        SailingBoatPolarData bottomLeft,
        double windSpeed,
        double windAngle
    )
    {
        if (topLeft == topRight && bottomLeft == bottomRight)
        {
            return windAngle >= topLeft.WindAngle && windAngle <= bottomLeft.WindAngle;
        }
        
        var point = new Point(windSpeed, windAngle);
        var poly = new[]
        {
            new Point(topLeft.WindVelocity, topLeft.WindAngle),
            new Point(topRight.WindVelocity, topRight.WindAngle),
            new Point(bottomRight.WindVelocity, bottomRight.WindAngle),
            new Point(bottomLeft.WindVelocity, bottomLeft.WindAngle)
        };

        var coef = poly.Skip(1).Select((p, i) =>
                (point.Y - poly[i].Y) * (p.X - poly[i].X)
                - (point.X - poly[i].X) * (p.Y - poly[i].Y))
            .ToList();

        if (coef.Any(p => p == 0))
            return true;

        for (int i = 1; i < coef.Count(); i++)
        {
            if (coef[i] * coef[i - 1] < 0)
                return false;
        }

        return true;
    }

    public double NearestNeighbourInterpolation(
        SailingBoatPolarData topLeft,
        SailingBoatPolarData topRight,
        SailingBoatPolarData bottomRight,
        SailingBoatPolarData bottomLeft,
        double windSpeed,
        double windAngle
    )
    {
        double topWindAngleDiff = Math.Abs(topLeft.WindAngle - windAngle);
        double bottomWindAngleDiff = Math.Abs(bottomLeft.WindAngle - windAngle);

        if (topWindAngleDiff < bottomWindAngleDiff)
        {
            double leftWindSpeedDiff = Math.Abs(topLeft.WindVelocity - windSpeed);
            double rightWindSpeedDiff = Math.Abs(topRight.WindVelocity - windSpeed);
            
            if (leftWindSpeedDiff < rightWindSpeedDiff)
            {
                return topLeft.BoatVelocity;
            }
            else
            {
                return topRight.BoatVelocity;
            }
        }
        else
        {
            double leftWindSpeedDiff = Math.Abs(bottomLeft.WindVelocity - windSpeed);
            double rightWindSpeedDiff = Math.Abs(bottomRight.WindVelocity - windSpeed);
            
            if (leftWindSpeedDiff < rightWindSpeedDiff)
            {
                return bottomLeft.BoatVelocity;
            }
            else
            {
                return bottomRight.BoatVelocity;
            }
        }
    }

    public double BilinearInterpolation(
        SailingBoatPolarData topLeft,
        SailingBoatPolarData topRight,
        SailingBoatPolarData bottomRight,
        SailingBoatPolarData bottomLeft,
        double windSpeed,
        double windAngle
    )
    {
        
        double x = 0;
        if (topRight != topLeft)
        {
            x = Math.Abs((windSpeed - topLeft.WindVelocity) / (topRight.WindVelocity - topLeft.WindVelocity));
        }

        double y = 0;
        if (bottomLeft != topLeft)
        {
            y = Math.Abs((windAngle - bottomLeft.WindAngle) / (bottomLeft.WindAngle - topLeft.WindAngle));
        }

        double wTopLeft = (1 - x) * y;
        double wTopRight = x * y;
        double wBottomRight = x * (1 - y);
        double wBottomLeft = (1 - x) * (1 - y);

        return topLeft.BoatVelocity * wTopLeft +
               topRight.BoatVelocity * wTopRight +
               bottomRight.BoatVelocity * wBottomRight +
               bottomLeft.BoatVelocity * wBottomLeft;
    }
}