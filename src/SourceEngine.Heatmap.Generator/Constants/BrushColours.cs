using System;
using System.Drawing;

namespace SourceEngine.Heatmap.Generator.Constants
{
	public class BrushColours
	{
		// Team Sides
		public static SolidBrush BrushTerrorist(int dataCount) => new SolidBrush(Color.FromArgb(GetTransparency(dataCount), 204, 102, 0));
		public static SolidBrush BrushCounterTerrorist(int dataCount) => new SolidBrush(Color.FromArgb(GetTransparency(dataCount), 51, 153, 255));

		// Bombsites
		public static SolidBrush BrushBombplant(int dataCount) => new SolidBrush(Color.FromArgb(GetTransparency(dataCount), 153, 0, 0));
		public static SolidBrush BrushHostageRescue(int dataCount) => new SolidBrush(Color.FromArgb(GetTransparency(dataCount), 153, 0, 0));

		// Grenades
		public static SolidBrush BrushGrenadeSmoke(int dataCount) => new SolidBrush(Color.FromArgb(GetTransparency(dataCount), 230, 255, 255));
		public static SolidBrush BrushGrenadeFlash(int dataCount) => new SolidBrush(Color.FromArgb(GetTransparency(dataCount), 102, 180, 255));
		public static SolidBrush BrushGrenadeHE(int dataCount) => new SolidBrush(Color.FromArgb(GetTransparency(dataCount), 128, 102, 255));
		public static SolidBrush BrushGrenadeIncendiary(int dataCount) => new SolidBrush(Color.FromArgb(GetTransparency(dataCount), 204, 0, 0));
		public static SolidBrush BrushGrenadeDecoy(int dataCount) => new SolidBrush(Color.FromArgb(GetTransparency(dataCount), 102, 230, 153));


		/// <summary>
		/// Returns the transparency value when parsed to an int
		/// </summary>
		/// <param name="dataCount"></param>
		/// <returns></returns>
		public static int GetTransparency(int dataCount)
		{
			if (dataCount == 0) return 255;
			else if (dataCount < 0) return 0;

			double multiplier = TransparencyMultiplierBorders.GetMultiplier(dataCount);

			var log = Math.Log(dataCount, multiplier);
			var transparency = (int)Math.Round(255 / log);

			if (transparency < 1)
			{
				transparency = 1;
			}
			else if (transparency > 255)
			{
				transparency = 255;
			}

			return transparency;
		}
	}
}
