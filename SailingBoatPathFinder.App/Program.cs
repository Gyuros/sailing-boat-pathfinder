// See https://aka.ms/new-console-template for more information

using System.Text.Json;
using Geolocation;
using SailingBoatPathfinder.Data.Models;
using SailingBoatPathfinder.Data.Services;
using SailingBoatPathfinder.Logic.Models;
using SailingBoatPathfinder.Logic.Services;

var poly = new List<Point>()
{
    new Point(3, 0),
    new Point(3, 0),
    new Point(3, 3),
    new Point(3, 3),
};
var point = new Point(0, 5);

static bool Inside(List<Point> poly, Point point)
{
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

Console.WriteLine(Inside(poly, point));

// var boatLoader = new BoatLoaderService();
// var pathFinder = new PathfinderService(new CoordinateProviderService(),
//     new TravellingTimeService(new WindProviderService(), new PolarDiagramService()));
// var boats = boatLoader.ReadFromFileAsync(CancellationToken.None).Result.ToList();
// var boat = boats.First();
//
// var coordinates = new List<Coordinate>()
// {
//     new Coordinate(47.0073, 18.0265),
//     new Coordinate(47.0083, 18.0465),
// };
// var path = pathFinder.FindPath(coordinates, boat, DateTime.Now);
//
// foreach (BoatPosition boatPosition in path)
// {
//         Console.WriteLine($"{boatPosition.Coordinate.Latitude.ToString().Replace(',','.')}, {boatPosition.Coordinate.Longitude.ToString().Replace(',','.')}");
// }

// var last = path.Last();
// ;



// var boatLoader = new BoatLoaderService();
// var boats = boatLoader.ReadFromFileAsync(CancellationToken.None).Result.ToList();
// var boat = boats.FirstOrDefault(x => x.Type == "S40 JUBILEE");
// var beats = boat.PolarData.Where(x => x.DataType == PolarDataType.Beat).ToList();
// var run = boat.PolarData.Where(x => x.DataType == PolarDataType.Run).ToList();
// var inner = boat.PolarData.Where(x => x.DataType == PolarDataType.Inner).ToList();



var service = new PolarDiagramService();
// var result = service.IsUnderBeatAngleOrOverGybeAngle(150, 150, 160, 165, 6, 8,162.5, 7);

// var topLeft = new SailingBoatPolarData() { WindVelocity = 10, WindAngle = 20, BoatVelocity = 16 };
// var topRight = new SailingBoatPolarData() { WindVelocity = 20, WindAngle = 20, BoatVelocity = 14 };
// var bottomLeft = new SailingBoatPolarData() { WindVelocity = 10, WindAngle = 10, BoatVelocity = 10 };
// var bottomRight = new SailingBoatPolarData() { WindVelocity = 20, WindAngle = 10, BoatVelocity = 12 };
// var result = service.NearestNeighbourInterpolation(topLeft, topRight, bottomRight, bottomLeft, 12.5, 12.5);
// Console.WriteLine(result);








// var newBoats = boats.Select(boat =>
// {
//     List<SailingBoatPolarData> newPolarDatas = new List<SailingBoatPolarData>();
//
//     foreach (PolarData polarData in boat.PolarData)
//     {
//         newPolarDatas.Add(new SailingBoatPolarData()
//         {
//             DataType = PolarDataType.Inner,
//             BoatVelocity = polarData.BoatVelocity,
//             WindVelocity = polarData.WindVelocity,
//             WindAngle = polarData.Angle
//         });
//     }
//
//     foreach (BoatProperty property in boat.Properties)
//     {
//         newPolarDatas.Add(new SailingBoatPolarData()
//         {
//             DataType = PolarDataType.Beat,
//             BoatVelocity = property.BeatVmg / Math.Cos(property.BeatAngle * Math.PI / 180),
//             WindAngle = property.BeatAngle,
//             WindVelocity = property.WindVelocity
//         });
//
//         newPolarDatas.Add(new SailingBoatPolarData()
//         {
//             DataType = PolarDataType.Run,
//             BoatVelocity = property.RunVmg / Math.Abs(Math.Cos(property.RunAngle * Math.PI / 180)),
//             WindAngle = property.RunAngle,
//             WindVelocity = property.WindVelocity
//         });
//     }
//
//     return new SailingBoat()
//     {
//         Type = boat.Type,
//         PolarData = newPolarDatas
//     };
// }).ToList();
//
// string newBoatsJson = JsonSerializer.Serialize(newBoats, JsonSerializerOptions.Default);
// StreamWriter outputFile = new StreamWriter("sailing-boats.json");
// outputFile.Write(newBoatsJson);
// outputFile.Flush();
// outputFile.Close();











// Boat? maxBoat = null;
// double maxDiff = 0;
// foreach (Boat boat in boats)
// {
//     BoatProperty? prev = boat.Properties.FirstOrDefault();
//     foreach (BoatProperty property in boat.Properties.Skip(1))
//     {
//         double diff = Math.Abs(property.BeatAngle - prev!.BeatAngle);
//         if (diff > maxDiff)
//         {
//             maxDiff = diff;
//             maxBoat = boat;
//         }
//
//         prev = property;
//     }
// }
//
// var beats = maxBoat.Properties.Select(x => x.BeatVmg).ToList();
// Console.WriteLine(beats);