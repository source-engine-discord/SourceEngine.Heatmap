using System;
using System.Drawing;

namespace SourceEngine.Heatmap.Generator.Constants
{
	public class PenColours
	{
		// Team Sides
		public static Pen PenTerrorist(int dataCount) => new Pen(Color.FromArgb(GetTransparency(dataCount), 204, 102, 0), 1);
		public static Pen PenCounterTerrorist(int dataCount) => new Pen(Color.FromArgb(GetTransparency(dataCount), 51, 153, 255), 1);
		public static Pen PenTerroristAllKills(int dataCount) => new Pen(Color.FromArgb(GetTransparency(dataCount), 204, 102, 0), 1);
		public static Pen PenCounterTerroristAllKills(int dataCount) => new Pen(Color.FromArgb(GetTransparency(dataCount), 51, 153, 255), 1);

		// Weapon Types
		public static Pen PenWeaponPistol(int dataCount) => new Pen(Color.FromArgb(GetTransparency(dataCount), 0, 255, 0), 1);
		public static Pen PenWeaponSMG(int dataCount) => new Pen(Color.FromArgb(GetTransparency(dataCount), 0, 180, 255), 1);
		public static Pen PenWeaponLMG(int dataCount) => new Pen(Color.FromArgb(GetTransparency(dataCount), 255, 51, 153), 1);
		public static Pen PenWeaponShotgun(int dataCount) => new Pen(Color.FromArgb(GetTransparency(dataCount), 204, 0, 204), 1);
		public static Pen PenWeaponAssaultRifle(int dataCount) => new Pen(Color.FromArgb(GetTransparency(dataCount), 255, 128, 0), 1);
		public static Pen PenWeaponSniper(int dataCount) => new Pen(Color.FromArgb(GetTransparency(dataCount), 255, 255, 51), 1);
		public static Pen PenWeaponGrenade(int dataCount) => new Pen(Color.FromArgb(GetTransparency(dataCount), 102, 255, 255), 1);
		public static Pen PenWeaponZeus(int dataCount) => new Pen(Color.FromArgb(GetTransparency(dataCount), 204, 153, 255), 1);
		public static Pen PenWeaponKnife(int dataCount) => new Pen(Color.FromArgb(GetTransparency(dataCount), 255, 153, 153), 1);
		public static Pen PenWeaponEquipment(int dataCount) => new Pen(Color.FromArgb(GetTransparency(dataCount), 102, 102, 102), 1);

		// Wallbang Wall Penetration Counts
		public static Pen PenWallbangCountOne(int dataCount) => new Pen(Color.FromArgb(GetTransparency(dataCount), 0, 255, 0), 1);
		public static Pen PenWallbangCountTwo(int dataCount) => new Pen(Color.FromArgb(GetTransparency(dataCount), 0, 102, 255), 1);
		public static Pen PenWallbangCountThreePlus(int dataCount) => new Pen(Color.FromArgb(GetTransparency(dataCount), 255, 0, 0), 1);

		// Player Positions
		public static Pen PenTerroristPlayerPosition(int dataCount) => new Pen(Color.FromArgb(GetTransparency(dataCount), 204, 102, 0), 1);
		public static Pen PenCounterTerroristPlayerPosition(int dataCount) => new Pen(Color.FromArgb(GetTransparency(dataCount), 51, 153, 255), 1);

		// Bombsites and Hostages
		public static Pen PenBombplant(int dataCount) => new Pen(Color.FromArgb(GetTransparency(dataCount), 153, 0, 0), 1);
		public static Pen PenHostageRescue(int dataCount) => new Pen(Color.FromArgb(GetTransparency(dataCount), 153, 0, 0), 1);

		// Grenades
		public static Pen PenGrenadeSmoke(int dataCount) => new Pen(Color.FromArgb(GetTransparency(dataCount), 230, 255, 255));
		public static Pen PenGrenadeFlash(int dataCount) => new Pen(Color.FromArgb(GetTransparency(dataCount), 102, 180, 255));
		public static Pen PenGrenadeHE(int dataCount) => new Pen(Color.FromArgb(GetTransparency(dataCount), 128, 102, 255));
		public static Pen PenGrenadeIncendiary(int dataCount) => new Pen(Color.FromArgb(GetTransparency(dataCount), 204, 0, 0));
		public static Pen PenGrenadeDecoy(int dataCount) => new Pen(Color.FromArgb(GetTransparency(dataCount), 102, 230, 153));


		/// <summary>
		/// Returns the transparency value when parsed to an int
		/// </summary>
		/// <param name="dataCount"></param>
		/// <returns></returns>
		public static int GetTransparency(int dataCount)
		{
			if (dataCount == 0) return 255;
			else if (dataCount < 0) return 0;

			double multiplier = MultiplierBorders.GetMultiplier(dataCount);

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
