namespace SailingBoatPathfinder.Logic.Models;

public class WindData
{
    public double Velocity { get; set; }
    public double AngleFrom { get; set; }
    public double Delay { get; set; }

    public WindData(double velocity, double angleFrom)
    {
        Velocity = velocity;
        AngleFrom = angleFrom;
    }
}