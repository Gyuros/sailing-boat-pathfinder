namespace SailingBoatPathfinder.Logic.Models;

public class WindDto
{
    public double Velocity { get; set; }
    public double AngleFrom { get; set; }
    public double Delay { get; set; }

    public WindDto(double velocity, double angleFrom)
    {
        Velocity = velocity;
        AngleFrom = angleFrom;
    }
}