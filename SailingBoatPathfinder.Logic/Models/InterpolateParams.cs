namespace SailingBoatPathfinder.Logic.Models;

public class InterpolateParams
{
    public InterpolateParams(PolarDiagramNode topLeft, PolarDiagramNode topRight, PolarDiagramNode bottomRight, PolarDiagramNode bottomLeft)
    {
        TopLeft = topLeft;
        TopRight = topRight;
        BottomRight = bottomRight;
        BottomLeft = bottomLeft;
    }

    public PolarDiagramNode TopLeft { get; set; }
    public PolarDiagramNode TopRight { get; set; }
    public PolarDiagramNode BottomRight { get; set; }
    public PolarDiagramNode BottomLeft { get; set; }
}