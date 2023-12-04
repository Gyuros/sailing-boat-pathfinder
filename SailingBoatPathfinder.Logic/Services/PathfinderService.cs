using System.Diagnostics;
using Geolocation;
using SailingBoatPathfinder.Data.Entities;
using SailingBoatPathfinder.Logic.Extensions;
using SailingBoatPathfinder.Logic.Models;

namespace SailingBoatPathfinder.Logic.Services;

public class PathfinderService
{
    private readonly GeographicPointProviderService _geographicPointProviderService;
    private readonly TravellingTimeService _travellingTimeService;

    private readonly Dictionary<Coordinate, BoatPosition> _boatPositionCache = new Dictionary<Coordinate, BoatPosition>();

    public PathfinderService(GeographicPointProviderService geographicPointProviderService,
        TravellingTimeService travellingTimeService)
    {
        _geographicPointProviderService = geographicPointProviderService;
        _travellingTimeService = travellingTimeService;
    }

    // végig iterálni a checkpointokon
    // mindegyik checkpoint pár között külön lefuttatni az A* algoritumst
    // service-től lekérdezni a következő geo pontot

    // current szomszédainak lekérdezése
    // minden szomszédra kiszámoljuk az időt amennyi az odajutáshoz kell
    // elmentjuk az adott szakasz, plusz az az előtti szakasz útidejének összegét
    // hozzáadjuk ehhez a heurisztikát (légvonalban szükséges idő)
    // ekkor lesz egy pontunk, aminél tudjuk, hogy oda mennyi idő volt eljutni és innen kb mennyi idő lesz még, legyen ez Tkb (time kb)
    // ezt a pontot betesszük a Neighbours listába

    // Neighbours-ből kiszedjük, melynek a legkisebb a Tkb-je
    // beállítjuk ezt currentnek, ismétlés
    public List<BoatPosition> FindPath(List<Coordinate> checkpoints, Boat boat, DateTime startTime)
    {
        Coordinate current = checkpoints.First();
        List<BoatPosition> path = new List<BoatPosition>();

        foreach (Coordinate next in checkpoints.Skip(1))
        {
            path.AddRange(FindPathBetween(current, next, startTime));
            // TODO: összekapcsolni a checkpointok közötti rész útvonalakat
            current = next;
        }

        return path;
    }

    private List<BoatPosition> FindPathBetween(Coordinate start, Coordinate finish, DateTime startTime)
    {
        double estimatedTimeFromStartToFinish = _travellingTimeService.TimeToTravel(start, finish, startTime);
        BoatPosition startPosition = new BoatPosition(null, 0, estimatedTimeFromStartToFinish, start);
        List<BoatPosition> openPositions = new List<BoatPosition>()
        {
            startPosition
        };

        while (openPositions.Count > 0)
        {
            BoatPosition current = openPositions.MinBy(neighbour => neighbour.TimeFromStartToFinish)!;

            // TODO: override equal hogy nagyjából közel legyen a 2 koordináta (pl 100 méteres körzetében
            if (current.Coordinate.Equals(finish))
            {
                return ConstructPath(current);
            }

            openPositions.Remove(current);

            List<BoatPosition> currentNeighbours = _geographicPointProviderService
                .GetNeighbours(current.Coordinate)
                .Select(geoPosition => _boatPositionCache.GetOrCreate(geoPosition,
                    new BoatPosition(current, double.PositiveInfinity, double.PositiveInfinity, geoPosition)))
                .ToList();

            foreach (BoatPosition currentNeighbour in currentNeighbours)
            {
                DateTime timeOfTravel = startTime.Add(TimeSpan.FromMicroseconds(current.TimeFromStart));
                double timeToTravel = _travellingTimeService.TimeToTravel(current.Coordinate, currentNeighbour.Coordinate, timeOfTravel);
                double timeFromStartToNeighbour = current.TimeFromStart + timeToTravel;

                if (!(timeFromStartToNeighbour < currentNeighbour.TimeFromStart))
                {
                    continue;
                }

                currentNeighbour.From = current;
                currentNeighbour.TimeFromStart = timeFromStartToNeighbour;
                currentNeighbour.EstimatedTimeToFinish = _travellingTimeService.TimeToTravel(start, finish, startTime);

                if (!openPositions.Contains(currentNeighbour))
                {
                    openPositions.Add(currentNeighbour);
                }
            }
        }

        throw new UnreachableException("No path found between start to finish");
    }

    private List<BoatPosition> ConstructPath(BoatPosition finish)
    {
        BoatPosition? current = finish;
        List<BoatPosition> path = new List<BoatPosition>();

        do
        {
            path.Add(current);
            current = current.From;
        } while (current != null);

        path.Reverse();
        return path;
    }
}