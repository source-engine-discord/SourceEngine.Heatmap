using System;
using System.Drawing;

namespace SourceEngine.Heatmap.Generator.Constants
{
	public class BrushColours
	{
		// Team Sides
		public static SolidBrush BrushTerrorist(float transparency) => new SolidBrush(Color.FromArgb(GetTransparency(transparency, 10), 204, 102, 0));
		public static SolidBrush BrushCounterTerrorist(float transparency) => new SolidBrush(Color.FromArgb(GetTransparency(transparency, 10), 51, 153, 255));

		// Bombsites
		public static SolidBrush BrushBombplant(float transparency) => new SolidBrush(Color.FromArgb(GetTransparency(transparency, 10), 153, 0, 0));
		public static SolidBrush BrushHostageRescue(float transparency) => new SolidBrush(Color.FromArgb(GetTransparency(transparency, 10), 153, 0, 0));

		// Grenades
		public static SolidBrush BrushGrenadeSmoke(float transparency) => new SolidBrush(Color.FromArgb(GetTransparency(transparency, 10), 230, 255, 255));
		public static SolidBrush BrushGrenadeFlash(float transparency) => new SolidBrush(Color.FromArgb(GetTransparency(transparency, 10), 102, 180, 255));
		public static SolidBrush BrushGrenadeHE(float transparency) => new SolidBrush(Color.FromArgb(GetTransparency(transparency, 10), 128, 102, 255));
		public static SolidBrush BrushGrenadeIncendiary(float transparency) => new SolidBrush(Color.FromArgb(GetTransparency(transparency, 10), 204, 0, 0));
		public static SolidBrush BrushGrenadeDecoy(float transparency) => new SolidBrush(Color.FromArgb(GetTransparency(transparency, 10), 102, 230, 153));


		/// <summary>
		/// Returns the transparecy value when parsed to an int
		/// </summary>
		/// <param name="transparency"></param>
		/// <param name="multiplier"></param>
		/// <returns></returns>
		public static int GetTransparency(float transparency, int multiplier)
		{
			var newTransparency = (int)Math.Round(transparency);

			if (newTransparency < 1)
			{
				newTransparency = 1;
			}

			newTransparency *= multiplier;

			if (newTransparency > 255)
			{
				newTransparency = 255;
			}

			return newTransparency;
		}
	}
}
