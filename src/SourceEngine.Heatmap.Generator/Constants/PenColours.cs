using System;
using System.Drawing;

namespace SourceEngine.Heatmap.Generator.Constants
{
	public class PenColours
	{
		// Team Sides
		public static Pen PenTerrorist(float transparency) => new Pen(Color.FromArgb(GetTransparency(transparency, 50), 204, 102, 0), 1);
		public static Pen PenCounterTerrorist(float transparency) => new Pen(Color.FromArgb(GetTransparency(transparency, 50), 51, 153, 255), 1);
		public static Pen PenTerroristAllKills(float transparency) => new Pen(Color.FromArgb(GetTransparency(transparency, 10), 204, 102, 0), 1);
		public static Pen PenCounterTerroristAllKills(float transparency) => new Pen(Color.FromArgb(GetTransparency(transparency, 10), 51, 153, 255), 1);

		// Weapon Types
		public static Pen PenWeaponPistol(float transparency) => new Pen(Color.FromArgb(GetTransparency(transparency, 10), 0, 255, 0), 1);
		public static Pen PenWeaponSMG(float transparency) => new Pen(Color.FromArgb(GetTransparency(transparency, 10), 0, 180, 255), 1);
		public static Pen PenWeaponLMG(float transparency) => new Pen(Color.FromArgb(GetTransparency(transparency, 10), 255, 51, 153), 1);
		public static Pen PenWeaponShotgun(float transparency) => new Pen(Color.FromArgb(GetTransparency(transparency, 10), 204, 0, 204), 1);
		public static Pen PenWeaponAssaultRifle(float transparency) => new Pen(Color.FromArgb(GetTransparency(transparency, 10), 255, 128, 0), 1);
		public static Pen PenWeaponSniper(float transparency) => new Pen(Color.FromArgb(GetTransparency(transparency, 10), 255, 255, 51), 1);
		public static Pen PenWeaponGrenade(float transparency) => new Pen(Color.FromArgb(GetTransparency(transparency, 75), 102, 255, 255), 1);
		public static Pen PenWeaponZeus(float transparency) => new Pen(Color.FromArgb(GetTransparency(transparency, 100), 204, 153, 255), 1);
		public static Pen PenWeaponKnife(float transparency) => new Pen(Color.FromArgb(GetTransparency(transparency, 100), 255, 153, 153), 1);
		public static Pen PenWeaponEquipment(float transparency) => new Pen(Color.FromArgb(GetTransparency(transparency, 100), 102, 102, 102), 1);

		// Wallbang Wall Penetration Counts
		public static Pen PenWallbangCountOne(float transparency) => new Pen(Color.FromArgb(GetTransparency(transparency, 50), 0, 255, 0), 1);
		public static Pen PenWallbangCountTwo(float transparency) => new Pen(Color.FromArgb(GetTransparency(transparency, 50), 0, 0, 255), 1);
		public static Pen PenWallbangCountThreePlus(float transparency) => new Pen(Color.FromArgb(GetTransparency(transparency, 50), 255, 0, 0), 1);

		// Player Positions
		public static Pen PenTerroristPlayerPosition(float transparency) => new Pen(Color.FromArgb(GetTransparency(transparency, 10), 204, 102, 0), 1);
		public static Pen PenCounterTerroristPlayerPosition(float transparency) => new Pen(Color.FromArgb(GetTransparency(transparency, 10), 51, 153, 255), 1);

		// Bombsites and Hostages
		public static Pen PenBombplant(float transparency) => new Pen(Color.FromArgb(GetTransparency(transparency, 20), 153, 0, 0), 1);
		public static Pen PenHostageRescue(float transparency) => new Pen(Color.FromArgb(GetTransparency(transparency, 20), 153, 0, 0), 1);

		// Grenades
		public static Pen PenGrenadeSmoke(float transparency) => new Pen(Color.FromArgb(GetTransparency(transparency, 50), 230, 255, 255));
		public static Pen PenGrenadeFlash(float transparency) => new Pen(Color.FromArgb(GetTransparency(transparency, 50), 102, 180, 255));
		public static Pen PenGrenadeHE(float transparency) => new Pen(Color.FromArgb(GetTransparency(transparency, 50), 128, 102, 255));
		public static Pen PenGrenadeIncendiary(float transparency) => new Pen(Color.FromArgb(GetTransparency(transparency, 50), 204, 0, 0));
		public static Pen PenGrenadeDecoy(float transparency) => new Pen(Color.FromArgb(GetTransparency(transparency, 50), 102, 230, 153));


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
