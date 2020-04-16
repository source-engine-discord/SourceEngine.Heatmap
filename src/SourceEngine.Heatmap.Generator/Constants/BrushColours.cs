using System.Drawing;

namespace SourceEngine.Heatmap.Generator.Constants
{
	public class BrushColours
	{
		// Team Sides
		public static SolidBrush BrushTerrorist => new SolidBrush(Color.FromArgb(25, 204, 102, 0));
		public static SolidBrush BrushCounterTerrorist => new SolidBrush(Color.FromArgb(25, 51, 153, 255));

		// Bombsites
		public static SolidBrush BrushBombplants => new SolidBrush(Color.FromArgb(35, 153, 0, 0));
	}
}
