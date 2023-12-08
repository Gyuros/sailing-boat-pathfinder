using SailingBoatPathfinder.Data.Models;
using SailingBoatPathfinder.Logic.Models;

namespace SailingBoatPathfinder.Logic.Services;

public class PolarDiagramService
{
    public double GetBoatSpeed(double windAngle, double windSpeed, SailingBoat boat)
    {
        List<SailingBoatPolarData> left = boat.PolarData.Where(data => data.WindVelocity <= windSpeed).ToList();
        List<SailingBoatPolarData> right = boat.PolarData.Where(data => data.WindVelocity >= windSpeed).ToList();

        SailingBoatPolarData? topLeft = left.LastOrDefault(data => data.WindAngle <= windAngle);
        SailingBoatPolarData? bottomLeft = left.FirstOrDefault(data => data.WindAngle >= windAngle);
        SailingBoatPolarData? topRight = right.LastOrDefault(data => data.WindAngle <= windAngle);
        SailingBoatPolarData? bottomRight = right.FirstOrDefault(data => data.WindAngle >= windAngle);

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
            return BilinearInterpolation(topLeft, topRight, bottomRight, bottomLeft, windSpeed, windAngle);
        }

        if (IsUnderBeatAngleOrOverGybeAngle(topLeft, topRight, bottomRight, bottomLeft, windSpeed, windAngle))
        {
            return 0;
        }

        return NearestNeighbourInterpolation(topLeft, topRight, bottomRight, bottomLeft, windSpeed, windAngle);
    }

    public bool IsUnderBeatAngleOrOverGybeAngle(
        SailingBoatPolarData topLeft,
        SailingBoatPolarData topRight,
        SailingBoatPolarData bottomRight,
        SailingBoatPolarData bottomLeft,
        double windSpeed,
        double windAngle
    )
    {
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
        double leftWindSpeedDiff = Math.Abs(topLeft.WindVelocity - windSpeed);
        double rightWindSpeedDiff = Math.Abs(topRight.WindVelocity - windSpeed);

        if (leftWindSpeedDiff < rightWindSpeedDiff)
        {
            double topWindAngleDiff = Math.Abs(topLeft.WindAngle - windAngle);
            double bottomWindAngleDiff = Math.Abs(bottomLeft.WindAngle - windAngle);

            if (topWindAngleDiff < bottomWindAngleDiff)
            {
                return topLeft.BoatVelocity;
            }
            else
            {
                return bottomLeft.BoatVelocity;
            }
        }
        else
        {
            double topWindAngleDiff = Math.Abs(topRight.WindAngle - windAngle);
            double bottomWindAngleDiff = Math.Abs(bottomRight.WindAngle - windAngle);

            if (topWindAngleDiff < bottomWindAngleDiff)
            {
                return topRight.BoatVelocity;
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
        double x = Math.Abs((windSpeed - topLeft.WindVelocity) / (topRight.WindVelocity - topLeft.WindVelocity));
        double y = Math.Abs((windAngle - bottomLeft.WindAngle) / (bottomLeft.WindAngle - topLeft.WindAngle));

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