using SailingBoatPathfinder.Data.Models;
using SailingBoatPathfinder.Logic.Models;

namespace SailingBoatPathfinder.Logic.Services;

public class PolarDiagramServiceOld
{
    /*public double GetBoatSpeed(double windAngle, double windSpeed, SailingBoat boat)
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
        
        BoatProperty left = boat.Properties.LastOrDefault(property => property.WindVelocity <= windSpeed)!; // TODO: nullable
        PolarData leftGreatestAngle = boat.PolarData.Where(polarData => polarData.WindVelocity <= windSpeed).MaxBy(polarData => polarData.Angle)!;
        if (left.RunVmg >= leftGreatestAngle.Angle)
        {
            // kisebb leftGreatestAngle.Angle
            // nagyobb left.RunVmg
        }
        else
        {
            // kisebb left.RunVmg
            // nagyobb leftGreatestAngle.Angle
        }
        
        
        
        
        
        
        
        
        
        
        
        
        //boat.Properties.Any(property => property.RunAngle <= windAngle) || boat.PolarData.MaxBy(polarData => polarData.Angle)?.Angle <= windAngle;
        InterpolateParams? interpolateParams;
        
        if (windAngle < minAngle)
        {
            interpolateParams = UseTopEdgeValues(windAngle, windSpeed, boat);
        }
        else if (false)
        {
            interpolateParams = UseBottomEdgeValues(windAngle, windSpeed, boat);
        }
        else
        {
            interpolateParams = UseMiddleValues(windAngle, windSpeed, boat);
        }


        return interpolateParams != null ? InterpolateSpeed(interpolateParams) : 0;
    }

    // bilineáris vagy legközelebbi
    private double InterpolateSpeed(InterpolateParams interpolateParams)
    {
        throw new NotImplementedException();
    }

    private double VmgToActualSpeed(double vmg, double angle)
    {
        throw new NotImplementedException();
    }

    private InterpolateParams? UseTopEdgeValues(double windAngle, double windSpeed, Boat boat)
    {
        BoatProperty? topLeft = boat.Properties.LastOrDefault(property => property.WindVelocity <= windSpeed);
        BoatProperty? topRight = boat.Properties.FirstOrDefault(property => property.WindVelocity >= windSpeed);

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

    private InterpolateParams? UseBottomEdgeValues(double windAngle, double windSpeed, Boat boat)
    {
        throw new NotImplementedException();
    }

    private InterpolateParams UseMiddleValues(double windAngle, double windSpeed, Boat boat)
    {
        List<PolarData> topValues = boat.PolarData.Where(polarData => polarData.Angle <= windAngle).ToList();
        List<PolarData> bottomValues = boat.PolarData.Where(polarData => polarData.Angle >= windAngle).ToList();

        PolarData? topLeft = topValues.LastOrDefault(polarData => polarData.WindVelocity <= windSpeed);
        PolarData? topRight = topValues.FirstOrDefault(polarData => polarData.WindVelocity >= windSpeed);

        PolarDiagramNode? bottomLeft;
        PolarDiagramNode? bottomRight;
            
        // if (bottomValues.Count == 0)
        // {
        //     // use run angles
        //     BoatProperty? runLeft = boat.Properties.FirstOrDefault(property => property.WindVelocity <= windSpeed);
        //     BoatProperty? runRight = boat.Properties.FirstOrDefault(property => property.WindVelocity >= windSpeed);
        //     
        //     bottomLeft = new PolarDiagramNode(runLeft?.WindVelocity ?? 0, 180, VmgToActualSpeed(runLeft.RunVmg))
        // }
        // else if(bottomValues.Count == 1)
        // {
        //     bottomLeft = bottomValues.FirstOrDefault(polarData => polarData.WindVelocity <= windSpeed);
        //     bottomRight = bottomValues.FirstOrDefault(polarData => polarData.WindVelocity >= windSpeed);
        // }
        // else
        // {
        //     
        // }
        //
        //
        // return new InterpolateParams(
        //     new PolarDiagramNode(topLeft?.WindVelocity ?? 0, topLeft?.Angle ?? topRight!.Angle, topLeft?.BoatVelocity ?? 0),
        //     new PolarDiagramNode(topRight?.WindVelocity ?? topLeft!.WindVelocity, topRight?.Angle ?? topLeft!.Angle, topRight?.BoatVelocity ?? topLeft!.BoatVelocity),
        //     bottomRight,
        //     bottomLeft
        // );
        throw new NotImplementedException();
    }*/
}