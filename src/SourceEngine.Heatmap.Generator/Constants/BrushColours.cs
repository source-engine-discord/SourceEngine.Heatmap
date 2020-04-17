using System.Drawing;

namespace SourceEngine.Heatmap.Generator.Constants
{
	public class BrushColours
	{
		// Team Sides
		public static SolidBrush BrushTerrorist => new SolidBrush(Color.FromArgb(25, 204, 102, 0));
		public static SolidBrush BrushCounterTerrorist => new SolidBrush(Color.FromArgb(25, 51, 153, 255));

		// Bombsites
		public static SolidBrush BrushBombplant => new SolidBrush(Color.FromArgb(35, 153, 0, 0));
		public static SolidBrush BrushHostageRescue => new SolidBrush(Color.FromArgb(35, 153, 0, 0));

		// Grenades
		public static SolidBrush BrushGrenadeSmoke => new SolidBrush(Color.FromArgb(35, 230, 255, 255));
		public static SolidBrush BrushGrenadeFlash => new SolidBrush(Color.FromArgb(35, 102, 180, 255));
		public static SolidBrush BrushGrenadeHE => new SolidBrush(Color.FromArgb(35, 128, 102, 255));
		public static SolidBrush BrushGrenadeIncendiary => new SolidBrush(Color.FromArgb(35, 204, 0, 0));
		public static SolidBrush BrushGrenadeDecoy => new SolidBrush(Color.FromArgb(35, 102, 230, 153));
	}
}
