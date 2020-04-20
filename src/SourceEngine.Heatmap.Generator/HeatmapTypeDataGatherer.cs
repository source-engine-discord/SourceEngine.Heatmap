using SourceEngine.Demo.Parser;
using SourceEngine.Demo.Stats.Models;
using SourceEngine.Heatmap.Generator.Constants;
using SourceEngine.Heatmap.Generator.Enums;
using SourceEngine.Heatmap.Generator.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

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
                case HeatmapTypeNames.TKills:
                    GenerateHeatmapDataTeamKills(overviewInfo, allStatsList, graphics, Sides.Terrorists, HeatmapTypeNames.TKills);
                    break;
                case HeatmapTypeNames.TKillsBeforeBombplant:
                    GenerateHeatmapDataTeamKills(overviewInfo, allStatsList, graphics, Sides.Terrorists, HeatmapTypeNames.TKillsBeforeBombplant);
                    break;
                case HeatmapTypeNames.TKillsAfterBombplant:
                    GenerateHeatmapDataTeamKills(overviewInfo, allStatsList, graphics, Sides.Terrorists, HeatmapTypeNames.TKillsAfterBombplant);
                    break;
                case HeatmapTypeNames.TKillsBeforeHostageTaken:
                    GenerateHeatmapDataTeamKills(overviewInfo, allStatsList, graphics, Sides.Terrorists, HeatmapTypeNames.TKillsBeforeHostageTaken);
                    break;
                case HeatmapTypeNames.TKillsAfterHostageTaken:
                    GenerateHeatmapDataTeamKills(overviewInfo, allStatsList, graphics, Sides.Terrorists, HeatmapTypeNames.TKillsAfterHostageTaken);
                    break;
                case HeatmapTypeNames.CTKills:
                    GenerateHeatmapDataTeamKills(overviewInfo, allStatsList, graphics, Sides.CounterTerrorists, HeatmapTypeNames.CTKills);
                    break;
                case HeatmapTypeNames.CTKillsBeforeBombplant:
                    GenerateHeatmapDataTeamKills(overviewInfo, allStatsList, graphics, Sides.CounterTerrorists, HeatmapTypeNames.CTKillsBeforeBombplant);
                    break;
                case HeatmapTypeNames.CTKillsAfterBombplant:
                    GenerateHeatmapDataTeamKills(overviewInfo, allStatsList, graphics, Sides.CounterTerrorists, HeatmapTypeNames.CTKillsAfterBombplant);
                    break;
                case HeatmapTypeNames.CTKillsBeforeHostageTaken:
                    GenerateHeatmapDataTeamKills(overviewInfo, allStatsList, graphics, Sides.CounterTerrorists, HeatmapTypeNames.CTKillsBeforeHostageTaken);
                    break;
                case HeatmapTypeNames.CTKillsAfterHostageTaken:
                    GenerateHeatmapDataTeamKills(overviewInfo, allStatsList, graphics, Sides.CounterTerrorists, HeatmapTypeNames.CTKillsAfterHostageTaken);
                    break;

                // kills - weapon types
                case HeatmapTypeNames.PistolKills:
                    GenerateHeatmapDataWeaponClassKills(overviewInfo, allStatsList, graphics, "pistol");
                    break;
                case HeatmapTypeNames.SmgKills:
                    GenerateHeatmapDataWeaponClassKills(overviewInfo, allStatsList, graphics, "smg");
                    break;
                case HeatmapTypeNames.LmgKills:
                    GenerateHeatmapDataWeaponClassKills(overviewInfo, allStatsList, graphics, "lmg");
                    break;
                case HeatmapTypeNames.ShotgunKills:
                    GenerateHeatmapDataWeaponClassKills(overviewInfo, allStatsList, graphics, "shotgun");
                    break;
                case HeatmapTypeNames.AssaultRifleKills:
                    GenerateHeatmapDataWeaponClassKills(overviewInfo, allStatsList, graphics, "assaultrifle");
                    break;
                case HeatmapTypeNames.SniperKills:
                    GenerateHeatmapDataWeaponClassKills(overviewInfo, allStatsList, graphics, "sniper");
                    break;
                case HeatmapTypeNames.GrenadeKills:
                    GenerateHeatmapDataWeaponClassKills(overviewInfo, allStatsList, graphics, "grenade");
                    break;
                case HeatmapTypeNames.ZeusKills:
                    GenerateHeatmapDataWeaponClassKills(overviewInfo, allStatsList, graphics, "zeus");
                    break;
                case HeatmapTypeNames.KnifeKills:
                    GenerateHeatmapDataWeaponClassKills(overviewInfo, allStatsList, graphics, "knife");
                    break;
                case HeatmapTypeNames.EquipmentKills:
                    GenerateHeatmapDataWeaponClassKills(overviewInfo, allStatsList, graphics, "equipment");
                    break;

                // kills - random
                case HeatmapTypeNames.WallbangKills:
                    GenerateHeatmapDataWallbangKills(overviewInfo, allStatsList, graphics);
                    break;

                // positions - players by team
                case HeatmapTypeNames.PlayerPositionsByTeam:
                    GenerateHeatmapDataPlayerPositions(overviewInfo, allStatsList, graphics);
                    break;
                case HeatmapTypeNames.CampingSpotsByTeam:
                    GenerateHeatmapDataCampingSpotPositions(overviewInfo, allStatsList, graphics);
                    break;
                case HeatmapTypeNames.FirstKillPositionsByTeam:
                    GenerateHeatmapDataFirstKillPositions(overviewInfo, allStatsList, graphics);
                    break;

                // locations - objectives
                case HeatmapTypeNames.BombPlantLocations:
                    GenerateHeatmapDataBombPlantLocations(overviewInfo, allStatsList, graphics);
                    break;
                case HeatmapTypeNames.HostageRescueLocations:
                    GenerateHeatmapDataHostageRescueLocations(overviewInfo, allStatsList, graphics);
                    break;

                // locations - grenades
                case HeatmapTypeNames.SmokeGrenadeLocations:
                    GenerateHeatmapDataGrenadeLocations(overviewInfo, allStatsList, graphics, "smoke");
                    break;
                case HeatmapTypeNames.FlashGrenadeLocations:
                    GenerateHeatmapDataGrenadeLocations(overviewInfo, allStatsList, graphics, "flash");
                    break;
                case HeatmapTypeNames.HEGrenadeLocations:
                    GenerateHeatmapDataGrenadeLocations(overviewInfo, allStatsList, graphics, "he");
                    break;
                case HeatmapTypeNames.IncendiaryGrenadeLocations:
                    GenerateHeatmapDataGrenadeLocations(overviewInfo, allStatsList, graphics, "incendiary");
                    break;
                case HeatmapTypeNames.DecoyGrenadeLocations:
                    GenerateHeatmapDataGrenadeLocations(overviewInfo, allStatsList, graphics, "decoy");
                    break;
            }
        }

        public void GenerateHeatmapDataTeamKills(OverviewInfo overviewInfo, List<AllStats> allStatsList, Graphics graphics, Sides side, string heatmapTypeName)
        {
            foreach (var allStats in allStatsList)
            {
                foreach (var kill in allStats.killsStats)
                {
                    var round = allStats.roundsStats.Where(x => x.Round == kill.Round).FirstOrDefault();

                    if (round != null)
                    {
                        var bombPlanted = (round.TimeInRoundPlanted != null && round.TimeInRoundPlanted > 0 && round.TimeInRoundPlanted <= kill.TimeInRound) ? true : false;
                        var hostageRescuedA = (round.TimeInRoundRescuedHostageA != null && round.TimeInRoundRescuedHostageA > 0 && round.TimeInRoundRescuedHostageA <= kill.TimeInRound) ? true : false;
                        var hostageRescuedB = (round.TimeInRoundRescuedHostageB != null && round.TimeInRoundRescuedHostageB > 0 && round.TimeInRoundRescuedHostageB <= kill.TimeInRound) ? true : false;
                        
                        if (heatmapTypeName == HeatmapTypeNames.TKills ||
                            heatmapTypeName == HeatmapTypeNames.CTKills ||
                            (heatmapTypeName == HeatmapTypeNames.TKillsBeforeBombplant && !bombPlanted) ||
                            (heatmapTypeName == HeatmapTypeNames.TKillsAfterBombplant && bombPlanted) ||
                            (heatmapTypeName == HeatmapTypeNames.CTKillsBeforeBombplant && !bombPlanted) ||
                            (heatmapTypeName == HeatmapTypeNames.CTKillsAfterBombplant && bombPlanted) ||
                            (heatmapTypeName == HeatmapTypeNames.TKillsBeforeHostageTaken && !hostageRescuedA && !hostageRescuedB) ||
                            (heatmapTypeName == HeatmapTypeNames.TKillsAfterHostageTaken && (hostageRescuedA || hostageRescuedB)) ||
                            (heatmapTypeName == HeatmapTypeNames.CTKillsBeforeHostageTaken && !hostageRescuedA && !hostageRescuedB) ||
                            (heatmapTypeName == HeatmapTypeNames.CTKillsAfterHostageTaken && (hostageRescuedA || hostageRescuedB))
                        )
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

                                var transparency = 255 / allStatsList.Sum(x => x.killsStats.Count());

                                Pen pen = side == Sides.Terrorists
                                            ? heatmapTypeName == HeatmapTypeNames.TKills
                                                ? PenColours.PenTerroristAllKills(transparency)
                                                : PenColours.PenTerrorist(transparency)
                                            : heatmapTypeName == HeatmapTypeNames.CTKills
                                                ? PenColours.PenCounterTerroristAllKills(transparency)
                                            : PenColours.PenCounterTerrorist(transparency);

                                heatmapLogicCenter.DrawLine(graphics, pen, linePoints);

                                pen.Dispose();
                            }
                        }
                    }
                }
            }
        }

        public void GenerateHeatmapDataWeaponClassKills(OverviewInfo overviewInfo, List<AllStats> allStatsList, Graphics graphics, string weaponType)
        {
            foreach (var allStats in allStatsList)
            {
                foreach (var kill in allStats.killsStats)
                {
                    var killerWeaponClass = kill.WeaponType?.ToLower();

                    var killerSide = heatmapLogicCenter.GetSideOfPlayerInKill(allStats, kill.Round);

                    if (killerWeaponClass == weaponType)
                    {
                        PointsData pointsData = new PointsData()
                        {
                            DataForPoint1X = kill.XPositionKill,
                            DataForPoint1Y = kill.YPositionKill,
                            DataForPoint2X = kill.XPositionDeath,
                            DataForPoint2Y = kill.YPositionDeath,
                        };

                        LinePoints linePoints = heatmapLogicCenter.CreateLinePoints(overviewInfo, pointsData);

                        var transparency = 255 / allStatsList.Sum(x => x.killsStats.Where(y => y.WeaponType?.ToLower() == weaponType).Count());

                        Pen pen = weaponType switch
                        {
                            "pistol" => PenColours.PenWeaponPistol(transparency),
                            "smg" => PenColours.PenWeaponSMG(transparency),
                            "lmg" => PenColours.PenWeaponLMG(transparency),
                            "shotgun" => PenColours.PenWeaponShotgun(transparency),
                            "assaultrifle" => PenColours.PenWeaponAssaultRifle(transparency),
                            "sniper" => PenColours.PenWeaponSniper(transparency),
                            "grenade" => PenColours.PenWeaponGrenade(transparency),
                            "zeus" => PenColours.PenWeaponZeus(transparency),
                            "knife" => PenColours.PenWeaponKnife(transparency),
                            _ => PenColours.PenWeaponEquipment(transparency),
                        };

                        heatmapLogicCenter.DrawLine(graphics, pen, linePoints);

                        pen.Dispose();
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

                        var transparency = 255 / allStatsList.Sum(x => x.killsStats.Where(y => y.PenetrationsCount > 0).Count());

                        Pen pen = kill.PenetrationsCount switch
                        {
                            1 => PenColours.PenWallbangCountOne(transparency),
                            2 => PenColours.PenWallbangCountTwo(transparency),
                            _ => PenColours.PenWallbangCountThreePlus(transparency),
                        };

                        heatmapLogicCenter.DrawLine(graphics, pen, linePoints);

                        pen.Dispose();
                    }
                }
            }
        }

        public void GenerateHeatmapDataPlayerPositions(OverviewInfo overviewInfo, List<AllStats> allStatsList, Graphics graphics)
        {
            int allStatsParsedCount = 0;

            foreach (var allStats in allStatsList)
            {
                var playerSteamIds = allStats.playerStats.Select(x => x.SteamID).Distinct().ToList();

                foreach (var round in allStats.playerPositionsStats)
                {
                    foreach (var player in playerSteamIds)
                    {
                        var playerPositionsInRound = new List<PlayerPositionBySteamID>();

                        foreach (var timeInRound in round.PlayerPositionByTimeInRound)
                        {
                            var playerPos = timeInRound.PlayerPositionBySteamID.Where(x => x.SteamID == player).FirstOrDefault();

                            if (playerPos != null) // if no data for this specific second
                            {
                                playerPositionsInRound.Add(playerPos);
                            }
                            else
                            {
                                var mostRecent = (
                                    from    roundList in allStats.playerPositionsStats
                                    from    timeInRoundList in roundList.PlayerPositionByTimeInRound
                                    from    playerList in timeInRoundList.PlayerPositionBySteamID
                                    where   roundList.Round == round.Round &&
                                            timeInRoundList.TimeInRound < timeInRound.TimeInRound &&
                                            playerList.SteamID == player
                                    orderby timeInRoundList.TimeInRound descending
                                    select  playerList
                                ).FirstOrDefault();
                                
                                var closestUpcoming = (
                                    from    roundList in allStats.playerPositionsStats
                                    from    timeInRoundList in roundList.PlayerPositionByTimeInRound
                                    from    playerList in timeInRoundList.PlayerPositionBySteamID
                                    where   roundList.Round == round.Round &&
                                            timeInRoundList.TimeInRound > timeInRound.TimeInRound &&
                                            playerList.SteamID == player
                                    orderby timeInRoundList.TimeInRound ascending
                                    select  playerList
                                ).FirstOrDefault();


                                if (closestUpcoming != null)
                                {
                                    var newPlayerPositionBySteamID = new PlayerPositionBySteamID();

                                    if (mostRecent != null)
                                    {
                                        var playerPosPrevUpcomingList = new List<PlayerPositionBySteamID>() { mostRecent, closestUpcoming };

                                        newPlayerPositionBySteamID = new PlayerPositionBySteamID()
                                        {
                                            SteamID = player,
                                            TeamSide = closestUpcoming.TeamSide,
                                            XPosition = (int)playerPosPrevUpcomingList.Average(x => x.XPosition),
                                            YPosition = (int)playerPosPrevUpcomingList.Average(x => x.YPosition),
                                            ZPosition = (int)playerPosPrevUpcomingList.Average(x => x.ZPosition),
                                        };
                                    }
                                    else
                                    {
                                        newPlayerPositionBySteamID = new PlayerPositionBySteamID()
                                        {
                                            SteamID = player,
                                            TeamSide = closestUpcoming.TeamSide,
                                            XPosition = closestUpcoming.XPosition,
                                            YPosition = closestUpcoming.YPosition,
                                            ZPosition = closestUpcoming.ZPosition,
                                        };
                                    }

                                    playerPositionsInRound.Add(newPlayerPositionBySteamID);
                                }
                            }
                        }

                        // draw lines
                        for (int i = 1; i < playerPositionsInRound.Count(); i++)
                        {
                            /*var side = allStats.teamStats.Where(x => x.Round == round.Round &&
                                            (x.TeamAlpha.Any(x => x == player) && x.Side.ToLower() == "terrorist" ||
                                             x.TeamBravo.Any(x => x == player) && x.Side.ToLower() == "terrorist"))
                                                ? Sides.Terrorists
                                                : Sides.CounterTerrorists;*/

                            PointsData pointsData = new PointsData()
                            {
                                DataForPoint1X = playerPositionsInRound[i].XPosition,
                                DataForPoint1Y = playerPositionsInRound[i].YPosition,
                                DataForPoint2X = playerPositionsInRound[i-1].XPosition,
                                DataForPoint2Y = playerPositionsInRound[i-1].YPosition,
                            };

                            LinePoints linePoints = heatmapLogicCenter.CreateLinePoints(overviewInfo, pointsData);

                            var maxLineLengthValid = 75;
                            if (Math.Abs(linePoints.Point1.X - linePoints.Point2.X) <= maxLineLengthValid && Math.Abs(linePoints.Point1.Y - linePoints.Point2.Y) <= maxLineLengthValid) // do not render anomalies
                            {
                                var asd = allStatsList.Sum(x => x.playerPositionsStats.Sum(y => y.PlayerPositionByTimeInRound.Select(z => z.PlayerPositionBySteamID).Count()));
                                var transparency = 255 / allStatsList.Sum(x => x.playerPositionsStats.Sum(y => y.PlayerPositionByTimeInRound.Select(z => z.PlayerPositionBySteamID).Count()));

                                Pen pen = playerPositionsInRound[i].TeamSide.ToLower() == "t"
                                            ? PenColours.PenTerroristPlayerPosition(transparency)
                                            : PenColours.PenCounterTerroristPlayerPosition(transparency);

                                /*Pen pen = side == Sides.Terrorists
                                            ? PenColours.PenTerrorist
                                            : PenColours.PenCounterTerrorist;*/

                                heatmapLogicCenter.DrawCurve(graphics, pen, linePoints);

                                pen.Dispose();
                            }
                        }
                    }
                }

                allStatsParsedCount++;
                Console.WriteLine(string.Concat("Finished parsing player positions data for demo: ", allStats.mapInfo.DemoName, " - ", allStatsParsedCount, " done."));
            }
        }

        public void GenerateHeatmapDataCampingSpotPositions(OverviewInfo overviewInfo, List<AllStats> allStatsList, Graphics graphics)
        {
            foreach (var allStats in allStatsList)
            {
                var playerSteamIds = allStats.playerStats.Select(x => x.SteamID).Distinct().ToList();

                foreach (var round in allStats.playerPositionsStats)
                {
                    foreach (var player in playerSteamIds)
                    {
                        var numOfSecondsCamped = 0; // used for camping positions heatmap
                        var campingPosition = new Vector(); // used for camping positions heatmap

                        foreach (var timeInRound in round.PlayerPositionByTimeInRound)
                        {
                            var playerPos = timeInRound.PlayerPositionBySteamID.Where(x => x.SteamID == player).FirstOrDefault();

                            if (playerPos != null) // if no data for this specific second
                            {
                                numOfSecondsCamped = (campingPosition.X == (int)playerPos.XPosition && campingPosition.Y == (int)playerPos.YPosition)
                                                        ? numOfSecondsCamped + 1
                                                        : 0;
                                campingPosition = new Vector() { X = (int)playerPos.XPosition, Y = (int)playerPos.YPosition, Z = (int)playerPos.ZPosition };

                                if (numOfSecondsCamped == 10) // if camped for x seconds, render the position (only done once until player moves)
                                {
                                    PointF singlePoint = heatmapLogicCenter.CreateSinglePoint(overviewInfo, campingPosition);

                                    var transparency = 255 / allStatsList.Sum(x => x.playerPositionsStats.Select(y => y.PlayerPositionByTimeInRound.Select(z => z.PlayerPositionBySteamID)).Count());

                                    SolidBrush brush = playerPos.TeamSide.ToLower() == "t"
                                                ? BrushColours.BrushTerrorist(transparency)
                                                : BrushColours.BrushCounterTerrorist(transparency);
                                    Pen pen = playerPos.TeamSide.ToLower() == "t"
                                                ? PenColours.PenTerrorist(transparency)
                                                : PenColours.PenCounterTerrorist(transparency);

                                    heatmapLogicCenter.DrawFilledCircle(graphics, brush, pen, singlePoint, 10);

                                    brush.Dispose();
                                    pen.Dispose();
                                }
                            }
                        }
                    }
                }
            }
        }

        public void GenerateHeatmapDataFirstKillPositions(OverviewInfo overviewInfo, List<AllStats> allStatsList, Graphics graphics)
        {
            foreach (var allStats in allStatsList)
            {
                foreach (var round in allStats.firstDamageStats)
                {
                    foreach (var firstDamage in round.FirstDamageToEnemyByPlayers)
                    {
                        PointsData pointsData = new PointsData()
                        {
                            DataForPoint1X = firstDamage.XPositionShooter,
                            DataForPoint1Y = firstDamage.YPositionShooter,
                            DataForPoint2X = firstDamage.XPositionVictim,
                            DataForPoint2Y = firstDamage.YPositionVictim,
                        };

                        LinePoints linePoints = heatmapLogicCenter.CreateLinePoints(overviewInfo, pointsData);

                        var transparency = 255 / allStatsList.Sum(x => x.firstDamageStats.Select(y => y.FirstDamageToEnemyByPlayers).Count());

                        Pen pen = firstDamage.TeamSideShooter.ToLower() == "terrorist"
                                        ? PenColours.PenTerrorist(transparency)
                                        : PenColours.PenCounterTerrorist(transparency);

                        heatmapLogicCenter.DrawLine(graphics, pen, linePoints);

                        pen.Dispose();
                    }
                }
            }
        }

        public void GenerateHeatmapDataBombPlantLocations(OverviewInfo overviewInfo, List<AllStats> allStatsList, Graphics graphics)
        {
            foreach (var allStats in allStatsList)
            {
                foreach (var round in allStats.roundsStats)
                {
                    if (round.BombsitePlantedAt != null && round.BombPlantPositionX != null && round.BombPlantPositionY != null && round.BombPlantPositionZ != null)
                    {
                        var bombPlantLocation = new Vector() { X = (int)round.BombPlantPositionX, Y = (int)round.BombPlantPositionY, Z = (int)round.BombPlantPositionZ };

                        PointF singlePoint = heatmapLogicCenter.CreateSinglePoint(overviewInfo, bombPlantLocation);

                        var transparency = 255 / allStatsList.Sum(x => x.roundsStats.Where(y => y.BombsitePlantedAt != null).Count());

                        SolidBrush brush = BrushColours.BrushBombplant(transparency);
                        Pen pen = PenColours.PenBombplant(transparency);

                        heatmapLogicCenter.DrawFilledCircle(graphics, brush, pen, singlePoint, 4);

                        brush.Dispose();
                        pen.Dispose();
                    }
                }
            }
        }

        public void GenerateHeatmapDataHostageRescueLocations(OverviewInfo overviewInfo, List<AllStats> allStatsList, Graphics graphics)
        {
            foreach (var allStats in allStatsList)
            {
                foreach (var round in allStats.roundsStats)
                {
                    List<Vector> hostageRescuedLocations = new List<Vector>();

                    if (round.RescuedHostageA == true && round.RescuedHostageAPositionX != null && round.RescuedHostageAPositionY != null && round.RescuedHostageAPositionZ != null)
                    {
                        hostageRescuedLocations.Add(new Vector() { X = (int)round.RescuedHostageAPositionX, Y = (int)round.RescuedHostageAPositionY, Z = (int)round.RescuedHostageAPositionZ });
                    }
                    if (round.RescuedHostageB == true && round.RescuedHostageBPositionX != null && round.RescuedHostageBPositionY != null && round.RescuedHostageBPositionZ != null)
                    {
                        hostageRescuedLocations.Add(new Vector() { X = (int)round.RescuedHostageBPositionX, Y = (int)round.RescuedHostageBPositionY, Z = (int)round.RescuedHostageBPositionZ });
                    }

                    foreach (var location in hostageRescuedLocations)
                    {
                        PointF singlePoint = heatmapLogicCenter.CreateSinglePoint(overviewInfo, location);

                        var transparency = 255 / allStatsList.Sum(x => x.roundsStats.Where(y => y.RescuedHostageA == true || y.RescuedHostageB == true).Count());

                        SolidBrush brush = BrushColours.BrushHostageRescue(transparency);
                        Pen pen = PenColours.PenHostageRescue(transparency);

                        heatmapLogicCenter.DrawFilledCircle(graphics, brush, pen, singlePoint, 4);

                        brush.Dispose();
                        pen.Dispose();
                    }
                }
            }
        }

        public void GenerateHeatmapDataGrenadeLocations(OverviewInfo overviewInfo, List<AllStats> allStatsList, Graphics graphics, string grenadeType)
        {
            foreach (var allStats in allStatsList)
            {
                foreach (var grenade in allStats.grenadesSpecificStats)
                {
                    if (grenade.NadeType.ToLower() == grenadeType)
                    {
                        Vector location = new Vector() { X = (float)grenade.XPosition, Y = (float)grenade.YPosition, Z = (float)grenade.ZPosition };

                        PointF singlePoint = heatmapLogicCenter.CreateSinglePoint(overviewInfo, location);

                        var transparency = 255 / allStatsList.Sum(x => x.grenadesSpecificStats.Where(y => y.NadeType.ToLower() == "smoke").Count());

                        SolidBrush brush = grenadeType switch
                        {
                            "smoke" => BrushColours.BrushGrenadeSmoke(transparency),
                            "flash" => BrushColours.BrushGrenadeFlash(transparency),
                            "he" => BrushColours.BrushGrenadeHE(transparency),
                            "incendiary" => BrushColours.BrushGrenadeIncendiary(transparency),
                            "decoy" => BrushColours.BrushGrenadeDecoy(transparency),
                        };
                        Pen pen = grenadeType switch
                        {
                            "smoke" => PenColours.PenGrenadeSmoke(transparency),
                            "flash" => PenColours.PenGrenadeFlash(transparency),
                            "he" => PenColours.PenGrenadeHE(transparency),
                            "incendiary" => PenColours.PenGrenadeIncendiary(transparency),
                            "decoy" => PenColours.PenGrenadeDecoy(transparency),
                        };
                        int diameter = grenadeType switch
                        {
                            "smoke" => 20,
                            "flash" => 6,
                            "he" => 12,
                            "incendiary" => 20,
                            "decoy" => 6,
                        };

                        heatmapLogicCenter.DrawFilledCircle(graphics, brush, pen, singlePoint, diameter);

                        brush.Dispose();
                        pen.Dispose();
                    }
                }
            }
        }
    }
}
