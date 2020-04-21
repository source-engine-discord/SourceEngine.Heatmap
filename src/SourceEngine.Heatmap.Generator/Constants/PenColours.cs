using System;
using System.Drawing;

namespace SourceEngine.Heatmap.Generator.Constants
{
	public class PenColours
	{
		// Team Sides
		public static Pen PenTerrorist(int dataCount) => new Pen(Color.FromArgb(GetTransparency(dataCount, 50), 204, 102, 0), 1);
		public static Pen PenCounterTerrorist(int dataCount) => new Pen(Color.FromArgb(GetTransparency(dataCount, 50), 51, 153, 255), 1);
		public static Pen PenTerroristAllKills(int dataCount) => new Pen(Color.FromArgb(GetTransparency(dataCount, 10), 204, 102, 0), 1);
		public static Pen PenCounterTerroristAllKills(int dataCount) => new Pen(Color.FromArgb(GetTransparency(dataCount, 10), 51, 153, 255), 1);

		// Weapon Types
		public static Pen PenWeaponPistol(int dataCount) => new Pen(Color.FromArgb(GetTransparency(dataCount, 10), 0, 255, 0), 1);
		public static Pen PenWeaponSMG(int dataCount) => new Pen(Color.FromArgb(GetTransparency(dataCount, 10), 0, 180, 255), 1);
		public static Pen PenWeaponLMG(int dataCount) => new Pen(Color.FromArgb(GetTransparency(dataCount, 10), 255, 51, 153), 1);
		public static Pen PenWeaponShotgun(int dataCount) => new Pen(Color.FromArgb(GetTransparency(dataCount, 10), 204, 0, 204), 1);
		public static Pen PenWeaponAssaultRifle(int dataCount) => new Pen(Color.FromArgb(GetTransparency(dataCount, 10), 255, 128, 0), 1);
		public static Pen PenWeaponSniper(int dataCount) => new Pen(Color.FromArgb(GetTransparency(dataCount, 10), 255, 255, 51), 1);
		public static Pen PenWeaponGrenade(int dataCount) => new Pen(Color.FromArgb(GetTransparency(dataCount, 75), 102, 255, 255), 1);
		public static Pen PenWeaponZeus(int dataCount) => new Pen(Color.FromArgb(GetTransparency(dataCount, 100), 204, 153, 255), 1);
		public static Pen PenWeaponKnife(int dataCount) => new Pen(Color.FromArgb(GetTransparency(dataCount, 100), 255, 153, 153), 1);
		public static Pen PenWeaponEquipment(int dataCount) => new Pen(Color.FromArgb(GetTransparency(dataCount, 100), 102, 102, 102), 1);

		// Wallbang Wall Penetration Counts
		public static Pen PenWallbangCountOne(int dataCount) => new Pen(Color.FromArgb(GetTransparency(dataCount, 50), 0, 255, 0), 1);
		public static Pen PenWallbangCountTwo(int dataCount) => new Pen(Color.FromArgb(GetTransparency(dataCount, 50), 0, 0, 255), 1);
		public static Pen PenWallbangCountThreePlus(int dataCount) => new Pen(Color.FromArgb(GetTransparency(dataCount, 50), 255, 0, 0), 1);

		// Player Positions
		public static Pen PenTerroristPlayerPosition(int dataCount) => new Pen(Color.FromArgb(GetTransparency(dataCount, 1.25), 204, 102, 0), 1);
		public static Pen PenCounterTerroristPlayerPosition(int dataCount) => new Pen(Color.FromArgb(GetTransparency(dataCount, 1.25), 51, 153, 255), 1);

		// Bombsites and Hostages
		public static Pen PenBombplant(int dataCount) => new Pen(Color.FromArgb(GetTransparency(dataCount, 20), 153, 0, 0), 1);
		public static Pen PenHostageRescue(int dataCount) => new Pen(Color.FromArgb(GetTransparency(dataCount, 20), 153, 0, 0), 1);

		// Grenades
		public static Pen PenGrenadeSmoke(int dataCount) => new Pen(Color.FromArgb(GetTransparency(dataCount, 50), 230, 255, 255));
		public static Pen PenGrenadeFlash(int dataCount) => new Pen(Color.FromArgb(GetTransparency(dataCount, 50), 102, 180, 255));
		public static Pen PenGrenadeHE(int dataCount) => new Pen(Color.FromArgb(GetTransparency(dataCount, 50), 128, 102, 255));
		public static Pen PenGrenadeIncendiary(int dataCount) => new Pen(Color.FromArgb(GetTransparency(dataCount, 50), 204, 0, 0));
		public static Pen PenGrenadeDecoy(int dataCount) => new Pen(Color.FromArgb(GetTransparency(dataCount, 50), 102, 230, 153));


		/// <summary>
		/// Returns the transparency value when parsed to an int
		/// </summary>
		/// <param name="dataCount"></param>
		/// <param name="multiplier"></param>
		/// <returns></returns>
		public static int GetTransparency(int dataCount, double multiplier)
		{
			if (dataCount == 0) return 255;
			else if (dataCount < 0) return 0;

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
