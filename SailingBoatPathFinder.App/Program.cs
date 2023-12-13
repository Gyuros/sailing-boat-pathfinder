// See https://aka.ms/new-console-template for more information

using SailingBoatPathfinder.Data.Models;
using SailingBoatPathfinder.Data.Services;
using SailingBoatPathfinder.Logic.Services;

var loaderService = new LoaderService();
var runConfigurationService = new RunConfigurationService(loaderService);
var runConfiguration = runConfigurationService.GetRunConfigurationAsync(CancellationToken.None).Result;
var pathFinder = new PathfinderService(new CoordinateProviderService(),
    new TravellingTimeService(new WindProviderService(runConfiguration.WindMap), new PolarDiagramService()));

var path = pathFinder.FindPath(runConfiguration.Coordinates, runConfiguration.Boat, runConfiguration.DateTime);
var output = path.Select(x => new Output() { Coordinate = x.Coordinate, TimeFromStart = x.TimeFromStart }).ToList();
loaderService.WriteOutput(output).Wait();