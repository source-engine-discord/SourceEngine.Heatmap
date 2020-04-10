using SourceEngine.Demo.Stats.Models;
using SourceEngine.Heatmap.Generator.Constants;
using SourceEngine.Heatmap.Generator.Enums;
using SourceEngine.Heatmap.Generator.Models;
using System.Collections.Generic;
using System.Drawing;

namespace SourceEngine.Heatmap.Generator
{
	public class HeatmapTypeDataGatherer
	{
        private HeatmapLogicCenter heatmapLogicCenter = new HeatmapLogicCenter();

        public HeatmapTypeDataGatherer()
        { }

        public void GenerateByHeatmapType(string heatmapType, OverviewInfo overviewInfo, List<AllStats> allStatsList, Graphics graphics)
        {
            switch (heatmapType.ToLower())
            {
                // kills - team sides
                case "tkills":
                    GenerateHeatmapDataTeamKills(overviewInfo, allStatsList, graphics, Sides.Terrorists);
                    break;
                case "ctkills":
                    GenerateHeatmapDataTeamKills(overviewInfo, allStatsList, graphics, Sides.CounterTerrorists);
                    break;

                // kills - weapon types
                case "pistolkills":
                    GenerateHeatmapDataWeaponClassKills(overviewInfo, allStatsList, graphics, "pistol");
                    break;
                case "smgkills":
                    GenerateHeatmapDataWeaponClassKills(overviewInfo, allStatsList, graphics, "smg");
                    break;
                case "lmgkills":
                    GenerateHeatmapDataWeaponClassKills(overviewInfo, allStatsList, graphics, "lmg");
                    break;
                case "shotgunkills":
                    GenerateHeatmapDataWeaponClassKills(overviewInfo, allStatsList, graphics, "shotgun");
                    break;
                case "assaultriflekills":
                    GenerateHeatmapDataWeaponClassKills(overviewInfo, allStatsList, graphics, "assaultrifle");
                    break;
                case "sniperkills":
                    GenerateHeatmapDataWeaponClassKills(overviewInfo, allStatsList, graphics, "sniper");
                    break;
                case "grenadekills":
                    GenerateHeatmapDataWeaponClassKills(overviewInfo, allStatsList, graphics, "grenade");
                    break;
                case "zeuskills":
                    GenerateHeatmapDataWeaponClassKills(overviewInfo, allStatsList, graphics, "zeus");
                    break;
                case "knifekills":
                    GenerateHeatmapDataWeaponClassKills(overviewInfo, allStatsList, graphics, "knife");
                    break;
                case "equipmentkills":
                    GenerateHeatmapDataWeaponClassKills(overviewInfo, allStatsList, graphics, "equipment");
                    break;

                // kills - random
                case "wallbangkills":
                    GenerateHeatmapDataWallbangKills(overviewInfo, allStatsList, graphics);
                    break;
            }
        }

        public void GenerateHeatmapDataTeamKills(OverviewInfo overviewInfo, List<AllStats> allStatsList, Graphics graphics, Sides side)
        {
            foreach (var allStats in allStatsList)
            {
                foreach (var kill in allStats.killsStats)
                {
                    var killerTeam = heatmapLogicCenter.GetTeamOfPlayerInKill(allStats, kill.Round, kill.KillerSteamID);
                    var victimTeam = heatmapLogicCenter.GetTeamOfPlayerInKill(allStats, kill.Round, kill.VictimSteamID);

                    var killerSide = heatmapLogicCenter.GetSideOfPlayerInKill(allStats, kill.Round);

                    if (killerTeam != victimTeam && killerSide == side)
                    {
                        PointsData pointsData = new PointsData()
                        {
                            DataForPoint1X = kill.XPositionKill,
                            DataForPoint1Y = kill.YPositionKill,
                            DataForPoint2X = kill.XPositionDeath,
                            DataForPoint2Y = kill.YPositionDeath,
                        };

                        LinePoints linePoints = heatmapLogicCenter.CreateLinePoints(overviewInfo, pointsData);

                        Pen pen = side == Sides.Terrorists
                                    ? PenColours.PenTerrorist
                                    : PenColours.PenCounterTerrorist;

                        heatmapLogicCenter.DrawLine(graphics, pen, linePoints);
                    }
                }
            }
        }

        public void GenerateHeatmapDataWeaponClassKills(OverviewInfo overviewInfo, List<AllStats> allStatsList, Graphics graphics, string weaponClass)
        {
            foreach (var allStats in allStatsList)
            {
                foreach (var kill in allStats.killsStats)
                {
                    var killerWeaponClass = kill.WeaponType.ToLower();

                    var killerSide = heatmapLogicCenter.GetSideOfPlayerInKill(allStats, kill.Round);

                    if (killerWeaponClass == weaponClass)
                    {
                        PointsData pointsData = new PointsData()
                        {
                            DataForPoint1X = kill.XPositionKill,
                            DataForPoint1Y = kill.YPositionKill,
                            DataForPoint2X = kill.XPositionDeath,
                            DataForPoint2Y = kill.YPositionDeath,
                        };

                        LinePoints linePoints = heatmapLogicCenter.CreateLinePoints(overviewInfo, pointsData);

                        Pen pen = weaponClass switch
                        {
                            "pistol" => PenColours.PenWeaponPistol,
                            "smg" => PenColours.PenWeaponSMG,
                            "lmg" => PenColours.PenWeaponLMG,
                            "shotgun" => PenColours.PenWeaponShotgun,
                            "assaultrifle" => PenColours.PenWeaponAssaultRifle,
                            "sniper" => PenColours.PenWeaponSniper,
                            "grenade" => PenColours.PenWeaponGrenade,
                            "zeus" => PenColours.PenWeaponZeus,
                            "knife" => PenColours.PenWeaponKnife,
                            _ => PenColours.PenWeaponEquipment,
                        };

                        heatmapLogicCenter.DrawLine(graphics, pen, linePoints);
                    }
                }
            }
        }

        public void GenerateHeatmapDataWallbangKills(OverviewInfo overviewInfo, List<AllStats> allStatsList, Graphics graphics)
        {
            foreach (var allStats in allStatsList)
            {
                foreach (var kill in allStats.killsStats)
                {
                    if (kill.PenetrationsCount > 0)
                    {
                        PointsData pointsData = new PointsData()
                        {
                            DataForPoint1X = kill.XPositionKill,
                            DataForPoint1Y = kill.YPositionKill,
                            DataForPoint2X = kill.XPositionDeath,
                            DataForPoint2Y = kill.YPositionDeath,
                        };

                        LinePoints linePoints = heatmapLogicCenter.CreateLinePoints(overviewInfo, pointsData);

                        Pen pen = kill.PenetrationsCount switch
                        {
                            1 => PenColours.PenWallbangCountOne,
                            2 => PenColours.PenWallbangCountTwo,
                            _ => PenColours.PenWallbangCountThreePlus,
                        };

                        heatmapLogicCenter.DrawLine(graphics, pen, linePoints);
                    }
                }
            }
        }
    }
}
