using SailingBoatPathfinder.Data.Models;
using SailingBoatPathfinder.Logic.Models;

namespace SailingBoatPathfinder.Logic.Services;

public class PolarDiagramService
{
    public double GetBoatSpeed(double windAngle, double windSpeed, Boat boat)
    {
        // mindent m/s-ban számulunk, átváltás kell
        
        // polar datas legkisebb szöge alatt van (52), akkor
            // properties-t nézünk
            // kiválasztjuk azt a 2 oszlopot, amik közé a szél sebessége esik
            // ha a szél sebessége nagyobb, mint 20
                // akkor utolsó oszlopot használjuk
            // különben, ha kisebb mint első oszlop
                // akkor 0 sebességet használunk interpolálásnál
            // különben
                // ha szél szöge kisebb mint mindkét beat angle, akkor
                    // visszatérünk 0val
                // különben 
                    // bilineáris interpoláció trapézban
        // különben
            // polar datas-t nézünk
            // run VMG szög az = 180 fok
            // bal szélső értéknek 0át, jobb szélső értéknek az utolsó oszlopot használjuk

        double minAngle = boat.PolarData.Min(polarData => polarData.Angle);
        InterpolateParams? interpolateParams;
        
        if (windAngle < minAngle)
        {
            interpolateParams = UseEdgeValues(windAngle, windSpeed, boat);
        }
        else
        {
            interpolateParams = UseMiddleValues(windAngle, windSpeed, boat);
        }


        return interpolateParams != null ? InterpolateSpeed(interpolateParams) : 0;
    }

    private double InterpolateSpeed(InterpolateParams interpolateParams)
    {
        throw new NotImplementedException();
    }

    private double VmgToActualSpeed(double vmg, double angle)
    {
        throw new NotImplementedException();
    }

    private InterpolateParams? UseEdgeValues(double windAngle, double windSpeed, Boat boat)
    {
        BoatProperty? topLeft = boat.Properties.LastOrDefault(property => property.WindVelocity <= windSpeed);
        BoatProperty? topRight = boat.Properties.LastOrDefault(property => property.WindVelocity >= windSpeed);

        topLeft ??= new BoatProperty()
        {
            BeatAngle = topRight!.BeatAngle,
            BeatVmg = 0,
            WindVelocity = 0,
        };

        topRight ??= topLeft;

        if (windAngle < topLeft.BeatAngle && windAngle < topRight.BeatAngle)
        {
            return null;
        }

        PolarData? bottomRight = boat.PolarData.Where(polarData => polarData.WindVelocity >= windSpeed).MinBy(polarData => polarData.Angle);
        PolarData? bottomLeft = boat.PolarData.Where(polarData => polarData.WindVelocity <= windSpeed).MinBy(polarData => polarData.Angle);

        bottomLeft ??= new PolarData()
        {
            Angle = bottomRight!.Angle,
            WindVelocity = 0,
            BoatVelocity = 0
        };
            
        bottomRight ??= bottomLeft;

        return new InterpolateParams(
            new PolarDiagramNode(topLeft.WindVelocity, topLeft.BeatAngle, VmgToActualSpeed(topLeft!.BeatVmg, topLeft!.BeatAngle)),
            new PolarDiagramNode(topRight.WindVelocity, topRight.BeatAngle, VmgToActualSpeed(topRight!.BeatVmg, topRight!.BeatAngle)),
            new PolarDiagramNode(bottomRight.WindVelocity, bottomRight.Angle, bottomRight.BoatVelocity),
            new PolarDiagramNode(bottomLeft.WindVelocity, bottomLeft.Angle, bottomLeft.BoatVelocity)
        );
    }

    private InterpolateParams UseMiddleValues(double windAngle, double windSpeed, Boat boat)
    {
        throw new NotImplementedException();
    }
}