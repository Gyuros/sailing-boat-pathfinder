using SailingBoatPathfinder.Data.Models;

namespace SailingBoatPathfinder.Data.Services;

public class RunConfigurationService
{
    private readonly LoaderService _loaderService;

    public RunConfigurationService(LoaderService loaderService)
    {
        _loaderService = loaderService;
    }

    public async Task<RunConfiguration> GetRunConfigurationAsync(CancellationToken cancellationToken)
    {
        RunConfiguration runConfiguration = await _loaderService.ReadFromFileAsync<RunConfiguration>("run-configuration.json", cancellationToken);

        runConfiguration.Boat = await LoadBoatAsync(runConfiguration.BoatsFileName, runConfiguration.BoatType, cancellationToken);
        runConfiguration.WindMap = await LoadWindMapAsync(runConfiguration.WindMapFileName, cancellationToken);

        return runConfiguration;
    }

    private async Task<SailingBoat> LoadBoatAsync(string fileName, string boatType, CancellationToken cancellationToken)
    {
        List<SailingBoat> boats = await _loaderService.ReadFromFileAsync<List<SailingBoat>>(fileName, cancellationToken);
        return boats.First(boat => boat.Type == boatType);
    }
    
    private async Task<WindMap> LoadWindMapAsync(string fileName, CancellationToken cancellationToken)
    {
        return await _loaderService.ReadFromFileAsync<WindMap>(fileName, cancellationToken);
    }
}