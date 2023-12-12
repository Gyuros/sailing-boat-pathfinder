namespace SailingBoatPathfinder.Data.Models;

public class SailingBoat
{
    public string Type { get; set; } = string.Empty;

    private List<SailingBoatPolarData> _polarData = new();
    
    private bool _ordered = false;

    public List<SailingBoatPolarData> PolarData
    {
        get
        {
            if (!_ordered)
            {   
                _polarData = _polarData.OrderBy(data => data.WindVelocity).ThenBy(data => data.WindAngle).ToList();
                _ordered = true;
            }

            return _polarData;
        }
        set => _polarData = value;
    }

}