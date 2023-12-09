namespace SailingBoatPathfinder.Data.Models;

public class SailingBoatPolarData
{
    public double WindAngle { get; set; }

    public double WindVelocity { get; set; }

    public double BoatVelocity { get; set; }

    public PolarDataType DataType { get; set; }

    public override string ToString()
    {
        return $"{WindAngle} - {WindVelocity}";
    }
}