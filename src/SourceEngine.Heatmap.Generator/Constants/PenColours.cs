using System.Drawing;

namespace SourceEngine.Heatmap.Generator.Constants
{
	public class PenColours
	{
		// Team Sides
		public static Pen PenTerrorist => new Pen(Color.FromArgb(100, 204, 102, 0), 1);
		public static Pen PenCounterTerrorist => new Pen(Color.FromArgb(100, 51, 153, 255), 1);

		// Weapon Types
		public static Pen PenWeaponPistol => new Pen(Color.FromArgb(50, 0, 255, 0), 1);
		public static Pen PenWeaponSMG => new Pen(Color.FromArgb(50, 0, 128, 255), 1);
		public static Pen PenWeaponLMG => new Pen(Color.FromArgb(50, 255, 51, 153), 1);
		public static Pen PenWeaponShotgun => new Pen(Color.FromArgb(50, 204, 0, 204), 1);
		public static Pen PenWeaponAssaultRifle => new Pen(Color.FromArgb(50, 255, 128, 0), 1);
		public static Pen PenWeaponSniper => new Pen(Color.FromArgb(50, 255, 255, 51), 1);
		public static Pen PenWeaponGrenade => new Pen(Color.FromArgb(50, 102, 255, 255), 1);
		public static Pen PenWeaponZeus => new Pen(Color.FromArgb(50, 204, 153, 255), 1);
		public static Pen PenWeaponKnife => new Pen(Color.FromArgb(50, 255, 153, 153), 1);
		public static Pen PenWeaponEquipment => new Pen(Color.FromArgb(50, 102, 102, 102), 1);

		// Wallbang Wall Penetration Counts
		public static Pen PenWallbangCountOne => new Pen(Color.FromArgb(50, 0, 255, 0), 1);
		public static Pen PenWallbangCountTwo => new Pen(Color.FromArgb(50, 0, 0, 255), 1);
		public static Pen PenWallbangCountThreePlus => new Pen(Color.FromArgb(50, 255, 0, 0), 1);

		// Bombsites
		public static Pen PenBombplant => new Pen(Color.FromArgb(35, 153, 0, 0), 1);
		public static Pen PenHostageRescue => new Pen(Color.FromArgb(35, 153, 0, 0), 1);

		// Grenades
		public static Pen PenGrenadeSmoke => new Pen(Color.FromArgb(35, 230, 255, 255));
		public static Pen PenGrenadeFlash => new Pen(Color.FromArgb(35, 102, 180, 255));
		public static Pen PenGrenadeHE => new Pen(Color.FromArgb(35, 128, 102, 255));
		public static Pen PenGrenadeIncendiary => new Pen(Color.FromArgb(35, 204, 0, 0));
		public static Pen PenGrenadeDecoy => new Pen(Color.FromArgb(35, 102, 230, 153));
	}
}
