using System.Drawing;

namespace SourceEngine.Heatmap.Generator.Constants
{
	public class PenColours
	{
		// Team Sides
		public static Pen PenTerrorist => new Pen(Color.FromArgb(50, 204, 102, 0), 1);
		public static Pen PenCounterTerrorist => new Pen(Color.FromArgb(50, 51, 153, 255), 1);

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
	}
}
