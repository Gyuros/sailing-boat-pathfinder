namespace SailingBoatPathfinder.Logic.Models;

public class PolarDiagramNode
{
    public PolarDiagramNode(double windSpeed, double windAngle, double boatSpeed)
    {
        WindSpeed = windSpeed;
        WindAngle = windAngle;
        BoatSpeed = boatSpeed;
    }

    public double WindSpeed { get; set; }
    public double WindAngle { get; set; }
    public double BoatSpeed { get; set; }
}