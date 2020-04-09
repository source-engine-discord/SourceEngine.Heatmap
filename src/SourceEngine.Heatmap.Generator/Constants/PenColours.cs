using System.Drawing;

namespace SourceEngine.Heatmap.Generator.Constants
{
	public class PenColours
	{
		public static Pen PenTerrorist => new Pen(Color.FromArgb(100, 204, 102, 0), 1);
		public static Pen PenCounterTerrorist => new Pen(Color.FromArgb(100, 51, 153, 255), 1);
	}
}
