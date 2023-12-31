using System.Diagnostics;
using Geolocation;
using SailingBoatPathfinder.Data.Models;
using SailingBoatPathfinder.Logic.Extensions;
using SailingBoatPathfinder.Logic.Models;

namespace SailingBoatPathfinder.Logic.Services;

public class PathfinderService
{
    private readonly CoordinateProviderService _coordinateProviderService;
    private readonly TravellingTimeService _travellingTimeService;

    private readonly Dictionary<Coordinate, BoatPosition> _boatPositionCache = new Dictionary<Coordinate, BoatPosition>();

    public PathfinderService(CoordinateProviderService coordinateProviderService,
        TravellingTimeService travellingTimeService)
    {
        _coordinateProviderService = coordinateProviderService;
        _travellingTimeService = travellingTimeService;
    }

    public List<BoatPosition> FindPath(List<Coordinate> checkpoints, SailingBoat boat, DateTime startTime)
    {
        checkpoints.ForEach(coordinate => _coordinateProviderService.RoundCoordinate(coordinate));
        Coordinate current = checkpoints.First();
        List<BoatPosition> path = new List<BoatPosition>();
        BoatPosition? partialLast = null;

        foreach (Coordinate next in checkpoints.Skip(1))
        {
            List<BoatPosition> partialPath = FindPathBetween(current, next, startTime, boat);
            
            BoatPosition partialFirst = partialPath.FirstOrDefault()!;
            partialFirst.From = partialLast;
            partialLast = partialPath.LastOrDefault();
            
            path.AddRange(partialPath);
            current = next;
        }

        return path;
    }

    private List<BoatPosition> FindPathBetween(Coordinate start, Coordinate finish, DateTime startTime, SailingBoat boat)
    {
        double estimatedTimeFromStartToFinish = _travellingTimeService.TimeToTravel(start, finish, startTime, boat);
        BoatPosition startPosition = new BoatPosition(null, 0, estimatedTimeFromStartToFinish, start);
        List<BoatPosition> openPositions = new List<BoatPosition>()
        {
            startPosition
        };

        while (openPositions.Count > 0)
        {
            BoatPosition current = openPositions.MinBy(neighbour => neighbour.TimeFromStartToFinish)!;

            if (GeoCalculator.GetDistance(current.Coordinate, finish, 0, DistanceUnit.Meters) < 50)
            {
                return ConstructPath(current);
            }

            openPositions.Remove(current);

            List<BoatPosition> currentNeighbours = _coordinateProviderService
                .GetNeighbours(current.Coordinate)
                .Select(geoPosition => _boatPositionCache.GetOrCreate(
                        geoPosition,
                        new BoatPosition(current, double.PositiveInfinity, double.PositiveInfinity, geoPosition)
                    )
                )
                .ToList();
            
            foreach (BoatPosition currentNeighbour in currentNeighbours)
            {
                DateTime timeOfTravelFromCurrent = startTime.Add(TimeSpan.FromSeconds(current.TimeFromStart));
                double timeToTravel = _travellingTimeService.TimeToTravel(current.Coordinate, currentNeighbour.Coordinate, timeOfTravelFromCurrent, boat);
                double timeFromStartToNeighbour = current.TimeFromStart + timeToTravel;

                if (!(timeFromStartToNeighbour < currentNeighbour.TimeFromStart))
                {
                    continue;
                }
                
                currentNeighbour.From = current;
                currentNeighbour.TimeFromStart = timeFromStartToNeighbour;
                DateTime timeOfTravelFromCurrentNeighbour = timeOfTravelFromCurrent.Add(TimeSpan.FromSeconds(timeToTravel));
                currentNeighbour.EstimatedTimeToFinish = _travellingTimeService.TimeToTravel(currentNeighbour.Coordinate, finish, timeOfTravelFromCurrentNeighbour, boat);
                
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