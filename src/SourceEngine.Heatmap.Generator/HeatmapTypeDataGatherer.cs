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
        private ConsoleMessageStyler consoleMessageStyler = new ConsoleMessageStyler();

        public HeatmapTypeDataGatherer()
        { }

        public void GenerateByHeatmapType(string heatmapType, OverviewInfo overviewInfo, List<AllOutputData> allOutputDataList, Graphics graphics)
        {
            switch (heatmapType.ToLower())
            {
                // kills - team sides
                case HeatmapTypeNames.TKills:
                    GenerateHeatmapDataTeamKills(overviewInfo, allOutputDataList, graphics, Sides.Terrorists, HeatmapTypeNames.TKills, null);
                    break;
                case HeatmapTypeNames.TKillsBeforeBombplant:
                    GenerateHeatmapDataTeamKills(overviewInfo, allOutputDataList, graphics, Sides.Terrorists, HeatmapTypeNames.TKillsBeforeBombplant, null);
                    break;
                case HeatmapTypeNames.TKillsAfterBombplantASite:
                    GenerateHeatmapDataTeamKills(overviewInfo, allOutputDataList, graphics, Sides.Terrorists, HeatmapTypeNames.TKillsAfterBombplantASite, "A");
                    break;
                case HeatmapTypeNames.TKillsAfterBombplantBSite:
                    GenerateHeatmapDataTeamKills(overviewInfo, allOutputDataList, graphics, Sides.Terrorists, HeatmapTypeNames.TKillsAfterBombplantBSite, "B");
                    break;
                case HeatmapTypeNames.TKillsBeforeHostageTaken:
                    GenerateHeatmapDataTeamKills(overviewInfo, allOutputDataList, graphics, Sides.Terrorists, HeatmapTypeNames.TKillsBeforeHostageTaken, null);
                    break;
                case HeatmapTypeNames.TKillsAfterHostageTaken:
                    GenerateHeatmapDataTeamKills(overviewInfo, allOutputDataList, graphics, Sides.Terrorists, HeatmapTypeNames.TKillsAfterHostageTaken, null);
                    break;
                case HeatmapTypeNames.CTKills:
                    GenerateHeatmapDataTeamKills(overviewInfo, allOutputDataList, graphics, Sides.CounterTerrorists, HeatmapTypeNames.CTKills, null);
                    break;
                case HeatmapTypeNames.CTKillsBeforeBombplant:
                    GenerateHeatmapDataTeamKills(overviewInfo, allOutputDataList, graphics, Sides.CounterTerrorists, HeatmapTypeNames.CTKillsBeforeBombplant, null);
                    break;
                case HeatmapTypeNames.CTKillsAfterBombplantASite:
                    GenerateHeatmapDataTeamKills(overviewInfo, allOutputDataList, graphics, Sides.CounterTerrorists, HeatmapTypeNames.CTKillsAfterBombplantASite, "A");
                    break;
                case HeatmapTypeNames.CTKillsAfterBombplantBSite:
                    GenerateHeatmapDataTeamKills(overviewInfo, allOutputDataList, graphics, Sides.CounterTerrorists, HeatmapTypeNames.CTKillsAfterBombplantBSite, "B");
                    break;
                case HeatmapTypeNames.CTKillsBeforeHostageTaken:
                    GenerateHeatmapDataTeamKills(overviewInfo, allOutputDataList, graphics, Sides.CounterTerrorists, HeatmapTypeNames.CTKillsBeforeHostageTaken, null);
                    break;
                case HeatmapTypeNames.CTKillsAfterHostageTaken:
                    GenerateHeatmapDataTeamKills(overviewInfo, allOutputDataList, graphics, Sides.CounterTerrorists, HeatmapTypeNames.CTKillsAfterHostageTaken, null);
                    break;

                // kills - weapon types
                case HeatmapTypeNames.PistolKills:
                    GenerateHeatmapDataWeaponTypeKills(overviewInfo, allOutputDataList, graphics, "pistol");
                    break;
                case HeatmapTypeNames.SmgKills:
                    GenerateHeatmapDataWeaponTypeKills(overviewInfo, allOutputDataList, graphics, "smg");
                    break;
                case HeatmapTypeNames.LmgKills:
                    GenerateHeatmapDataWeaponTypeKills(overviewInfo, allOutputDataList, graphics, "lmg");
                    break;
                case HeatmapTypeNames.ShotgunKills:
                    GenerateHeatmapDataWeaponTypeKills(overviewInfo, allOutputDataList, graphics, "shotgun");
                    break;
                case HeatmapTypeNames.AssaultRifleKills:
                    GenerateHeatmapDataWeaponTypeKills(overviewInfo, allOutputDataList, graphics, "assaultrifle");
                    break;
                case HeatmapTypeNames.SniperKills:
                    GenerateHeatmapDataWeaponTypeKills(overviewInfo, allOutputDataList, graphics, "sniper");
                    break;
                case HeatmapTypeNames.GrenadeKills:
                    GenerateHeatmapDataWeaponTypeKills(overviewInfo, allOutputDataList, graphics, "grenade");
                    break;
                case HeatmapTypeNames.ZeusKills:
                    GenerateHeatmapDataWeaponTypeKills(overviewInfo, allOutputDataList, graphics, "zeus");
                    break;
                case HeatmapTypeNames.KnifeKills:
                    GenerateHeatmapDataWeaponTypeKills(overviewInfo, allOutputDataList, graphics, "knife");
                    break;
                case HeatmapTypeNames.EquipmentKills:
                    GenerateHeatmapDataWeaponTypeKills(overviewInfo, allOutputDataList, graphics, "equipment");
                    break;

                // kills - random
                case HeatmapTypeNames.WallbangKills:
                    GenerateHeatmapDataWallbangKills(overviewInfo, allOutputDataList, graphics);
                    break;

                // positions - players by team
                case HeatmapTypeNames.PlayerPositionsByTeam:
                    GenerateHeatmapDataPlayerPositions(overviewInfo, allOutputDataList, graphics);
                    break;
                case HeatmapTypeNames.CampingSpotsByTeam:
                    GenerateHeatmapDataCampingSpotPositions(overviewInfo, allOutputDataList, graphics);
                    break;
                case HeatmapTypeNames.FirstKillPositionsByTeam:
                    GenerateHeatmapDataFirstKillPositions(overviewInfo, allOutputDataList, graphics);
                    break;

                // locations - objectives
                case HeatmapTypeNames.BombPlantLocations:
                    GenerateHeatmapDataBombPlantLocations(overviewInfo, allOutputDataList, graphics);
                    break;
                case HeatmapTypeNames.HostageRescueLocations:
                    GenerateHeatmapDataHostageRescueLocations(overviewInfo, allOutputDataList, graphics);
                    break;

                // locations - grenades
                case HeatmapTypeNames.SmokeGrenadeLocations:
                    GenerateHeatmapDataGrenadeLocations(overviewInfo, allOutputDataList, graphics, "smoke");
                    break;
                case HeatmapTypeNames.FlashGrenadeLocations:
                    GenerateHeatmapDataGrenadeLocations(overviewInfo, allOutputDataList, graphics, "flash");
                    break;
                case HeatmapTypeNames.HEGrenadeLocations:
                    GenerateHeatmapDataGrenadeLocations(overviewInfo, allOutputDataList, graphics, "he");
                    break;
                case HeatmapTypeNames.IncendiaryGrenadeLocations:
                    GenerateHeatmapDataGrenadeLocations(overviewInfo, allOutputDataList, graphics, "incendiary");
                    break;
                case HeatmapTypeNames.DecoyGrenadeLocations:
                    GenerateHeatmapDataGrenadeLocations(overviewInfo, allOutputDataList, graphics, "decoy");
                    break;
            }
        }

        public void GenerateHeatmapDataTeamKills(OverviewInfo overviewInfo, List<AllOutputData> allOutputDataList, Graphics graphics, Sides side, string heatmapTypeName, string bombsitePlantedAt)
        {
            // parameter error checks
            if (bombsitePlantedAt != null)
            {
                if (heatmapTypeName != HeatmapTypeNames.TKillsAfterBombplantASite && heatmapTypeName != HeatmapTypeNames.TKillsAfterBombplantBSite &&
                    heatmapTypeName != HeatmapTypeNames.CTKillsAfterBombplantASite && heatmapTypeName != HeatmapTypeNames.CTKillsAfterBombplantBSite
                )
                {
                    var errorMessage = "bombsitePlantedAt value was provided to GenerateHeatmapDataTeamKills(), yet the heatmap type was not a heatmap type that uses this.";
                    consoleMessageStyler.PrintErrorMessage(errorMessage);
                    return;
                }
                else if ((bombsitePlantedAt.ToUpper() == "A" && (heatmapTypeName == HeatmapTypeNames.TKillsAfterBombplantBSite || heatmapTypeName == HeatmapTypeNames.CTKillsAfterBombplantBSite)) ||
                        (bombsitePlantedAt.ToUpper() == "B" && (heatmapTypeName == HeatmapTypeNames.TKillsAfterBombplantASite || heatmapTypeName == HeatmapTypeNames.CTKillsAfterBombplantASite))
                )
                {
                    var errorMessage = "bombsitePlantedAt value was provided to GenerateHeatmapDataTeamKills(), seems to be the wrong value for what is expected with the heatmap type specified.";
                    consoleMessageStyler.PrintErrorMessage(errorMessage);
                    return;
                }
            }
            
            
            List<LinePoints> linePointsList = new List<LinePoints>();

            foreach (var allOutputData in allOutputDataList)
            {
                foreach (var kill in allOutputData.AllStats.killsStats)
                {
                    var round = allOutputData.AllStats.roundsStats.Where(x => x.Round == kill.Round).FirstOrDefault();

                    if (round != null)
                    {
                        var bombPlantedA = round.BombsitePlantedAt?.ToUpper() == "A" ? true : false;
                        var bombPlantedB = round.BombsitePlantedAt?.ToUpper() == "B" ? true : false;
                        var hostageRescuedA = round.RescuedHostageA;
                        var hostageRescuedB = round.RescuedHostageB;
                        
                        if (heatmapTypeName == HeatmapTypeNames.TKills ||
                            heatmapTypeName == HeatmapTypeNames.CTKills ||
                            (heatmapTypeName == HeatmapTypeNames.TKillsBeforeBombplant && !bombPlantedA && !bombPlantedB) ||
                            (heatmapTypeName == HeatmapTypeNames.TKillsAfterBombplantASite && bombPlantedA) ||
                            (heatmapTypeName == HeatmapTypeNames.TKillsAfterBombplantBSite && bombPlantedB) ||
                            (heatmapTypeName == HeatmapTypeNames.CTKillsBeforeBombplant && !bombPlantedA && !bombPlantedB) ||
                            (heatmapTypeName == HeatmapTypeNames.CTKillsAfterBombplantASite && bombPlantedA) ||
                            (heatmapTypeName == HeatmapTypeNames.CTKillsAfterBombplantBSite && bombPlantedB) ||
                            (heatmapTypeName == HeatmapTypeNames.TKillsBeforeHostageTaken && !hostageRescuedA && !hostageRescuedB) ||
                            (heatmapTypeName == HeatmapTypeNames.TKillsAfterHostageTaken && (hostageRescuedA || hostageRescuedB)) ||
                            (heatmapTypeName == HeatmapTypeNames.CTKillsBeforeHostageTaken && !hostageRescuedA && !hostageRescuedB) ||
                            (heatmapTypeName == HeatmapTypeNames.CTKillsAfterHostageTaken && (hostageRescuedA || hostageRescuedB))
                        )
                        {
                            var killerTeam = heatmapLogicCenter.GetTeamOfPlayerInKill(allOutputData.AllStats, kill.Round, kill.KillerSteamID);
                            var victimTeam = heatmapLogicCenter.GetTeamOfPlayerInKill(allOutputData.AllStats, kill.Round, kill.VictimSteamID);

                            var killerSide = heatmapLogicCenter.GetSideOfPlayerInKill(allOutputData.AllStats, kill.Round);

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

                                linePointsList.Add(linePoints);
                            }
                        }
                    }
                }
            }

            // draw onto the graphics
            var dataCount = linePointsList.Count();
            Pen pen = null;
            foreach (var linePoints in linePointsList)
            {
                pen = side == Sides.Terrorists
                                               ? heatmapTypeName == HeatmapTypeNames.TKills
                                                   ? PenColours.PenTerroristAllKills(dataCount)
                                                   : PenColours.PenTerrorist(dataCount)
                                               : heatmapTypeName == HeatmapTypeNames.CTKills
                                                   ? PenColours.PenCounterTerroristAllKills(dataCount)
                                                   : PenColours.PenCounterTerrorist(dataCount);

                heatmapLogicCenter.DrawLine(graphics, pen, linePoints);
            }
            pen?.Dispose();
        }

        public void GenerateHeatmapDataWeaponTypeKills(OverviewInfo overviewInfo, List<AllOutputData> allOutputDataList, Graphics graphics, string weaponType)
        {
            List<LinePoints> linePointsList = new List<LinePoints>();

            foreach (var allOutputData in allOutputDataList)
            {
                foreach (var kill in allOutputData.AllStats.killsStats)
                {
                    var killerWeaponType = kill.WeaponType?.ToLower();

                    if (killerWeaponType == weaponType)
                    {
                        PointsData pointsData = new PointsData()
                        {
                            DataForPoint1X = kill.XPositionKill,
                            DataForPoint1Y = kill.YPositionKill,
                            DataForPoint2X = kill.XPositionDeath,
                            DataForPoint2Y = kill.YPositionDeath,
                        };

                        LinePoints linePoints = heatmapLogicCenter.CreateLinePoints(overviewInfo, pointsData);

                        linePointsList.Add(linePoints);
                    }
                }
            }

            // draw onto the graphics
            var dataCount = linePointsList.Count();
            Pen pen = null;
            foreach (var linePoints in linePointsList)
            {
                pen = weaponType switch
                {
                    "pistol" => PenColours.PenWeaponPistol(dataCount),
                    "smg" => PenColours.PenWeaponSMG(dataCount),
                    "lmg" => PenColours.PenWeaponLMG(dataCount),
                    "shotgun" => PenColours.PenWeaponShotgun(dataCount),
                    "assaultrifle" => PenColours.PenWeaponAssaultRifle(dataCount),
                    "sniper" => PenColours.PenWeaponSniper(dataCount),
                    "grenade" => PenColours.PenWeaponGrenade(dataCount),
                    "zeus" => PenColours.PenWeaponZeus(dataCount),
                    "knife" => PenColours.PenWeaponKnife(dataCount),
                    _ => PenColours.PenWeaponEquipment(dataCount),
                };

                heatmapLogicCenter.DrawLine(graphics, pen, linePoints);
            }
            pen?.Dispose();
        }

        public void GenerateHeatmapDataWallbangKills(OverviewInfo overviewInfo, List<AllOutputData> allOutputDataList, Graphics graphics)
        {
            List<LinePoints> linePointsList = new List<LinePoints>();
            List<int> penetrationCounts = new List<int>();

            foreach (var allOutputData in allOutputDataList)
            {
                foreach (var kill in allOutputData.AllStats.killsStats)
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

                        linePointsList.Add(linePoints);
                        penetrationCounts.Add(kill.PenetrationsCount);
                    }
                }
            }

            // draw onto the graphics
            var dataCount = linePointsList.Count();
            Pen pen = null;
            for (int i = 0; i < linePointsList.Count(); i++)
            {
                pen = penetrationCounts.ElementAt(i) switch
                {
                    0 => PenColours.PenWallbangCountOne(-1), // shouldn't happen if data in linePointsList is correct
                    1 => PenColours.PenWallbangCountOne(dataCount),
                    2 => PenColours.PenWallbangCountTwo(dataCount),
                    _ => PenColours.PenWallbangCountThreePlus(dataCount),
                };

                heatmapLogicCenter.DrawLine(graphics, pen, linePointsList.ElementAt(i));
            }
            pen?.Dispose();
        }

        public void GenerateHeatmapDataPlayerPositions(OverviewInfo overviewInfo, List<AllOutputData> allOutputDataList, Graphics graphics)
        {
            List<LinePoints> linePointsList = new List<LinePoints>();
            List<string> teamValues = new List<string>();

            int allStatsParsedCount = 1;

            foreach (var allOutputData in allOutputDataList)
            {
                if (allOutputData.PlayerPositionsStats != null) // skips when no json file containing PlayerPositionsStats has been provided
                {
                    var playerSteamIds = allOutputData.AllStats.playerStats.Select(x => x.SteamID).Distinct().ToList();

                    foreach (var round in allOutputData.PlayerPositionsStats.PlayerPositionByRound)
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
                                        from roundList in allOutputData.PlayerPositionsStats.PlayerPositionByRound
                                        from timeInRoundList in roundList.PlayerPositionByTimeInRound
                                        from playerList in timeInRoundList.PlayerPositionBySteamID
                                        where roundList.Round == round.Round &&
                                                timeInRoundList.TimeInRound < timeInRound.TimeInRound &&
                                                playerList.SteamID == player
                                        orderby timeInRoundList.TimeInRound descending
                                        select playerList
                                    ).FirstOrDefault();

                                    var closestUpcoming = (
                                        from roundList in allOutputData.PlayerPositionsStats.PlayerPositionByRound
                                        from timeInRoundList in roundList.PlayerPositionByTimeInRound
                                        from playerList in timeInRoundList.PlayerPositionBySteamID
                                        where roundList.Round == round.Round &&
                                                timeInRoundList.TimeInRound > timeInRound.TimeInRound &&
                                                playerList.SteamID == player
                                        orderby timeInRoundList.TimeInRound ascending
                                        select playerList
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
                                    DataForPoint2X = playerPositionsInRound[i - 1].XPosition,
                                    DataForPoint2Y = playerPositionsInRound[i - 1].YPosition,
                                };

                                LinePoints linePoints = heatmapLogicCenter.CreateLinePoints(overviewInfo, pointsData);

                                linePointsList.Add(linePoints);
                                teamValues.Add(playerPositionsInRound[i].TeamSide.ToLower());
                            }
                        }
                    }

                    Console.WriteLine(string.Concat("Finished parsing player positions data for demo: ", allOutputData.AllStats.mapInfo.DemoName, " - ", allStatsParsedCount, "/", allOutputDataList.Count(), " done."));
                }
                else
                {
                    Console.WriteLine(string.Concat("Skipped parsing player positions data (no data provided) for demo: ", allOutputData.AllStats.mapInfo.DemoName, " - ", allStatsParsedCount, "/", allOutputDataList.Count(), " done."));
                }

                allStatsParsedCount++;
            }

            // draw onto the graphics
            var dataCount = linePointsList.Count();
            Pen pen = null;
            for (int i = 0; i < linePointsList.Count(); i++)
            {
                var maxLineLengthValid = 75;
                if (Math.Abs(linePointsList.ElementAt(i).Point1.X - linePointsList.ElementAt(i).Point2.X) <= maxLineLengthValid && Math.Abs(linePointsList.ElementAt(i).Point1.Y - linePointsList.ElementAt(i).Point2.Y) <= maxLineLengthValid) // do not render anomalies
                {
                    pen = teamValues.ElementAt(i) == "t"
                                ? PenColours.PenTerroristPlayerPosition(dataCount)
                                : PenColours.PenCounterTerroristPlayerPosition(dataCount);

                    /*Pen pen = side == Sides.Terrorists
                                ? PenColours.PenTerrorist
                                : PenColours.PenCounterTerrorist;*/

                    heatmapLogicCenter.DrawCurve(graphics, pen, linePointsList.ElementAt(i));
                }
            }
            pen?.Dispose();
        }

        public void GenerateHeatmapDataCampingSpotPositions(OverviewInfo overviewInfo, List<AllOutputData> allOutputDataList, Graphics graphics)
        {
            List<PointF> singlePointList = new List<PointF>();
            List<string> teamValues = new List<string>();

            foreach (var allOutputData in allOutputDataList)
            {
                if (allOutputData.PlayerPositionsStats != null) // skips when no json file containing PlayerPositionsStats has been provided
                {
                    var playerSteamIds = allOutputData.AllStats.playerStats.Select(x => x.SteamID).Distinct().ToList();

                    foreach (var round in allOutputData.PlayerPositionsStats.PlayerPositionByRound)
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

                                        singlePointList.Add(singlePoint);
                                        teamValues.Add(playerPos.TeamSide.ToLower());
                                    }
                                }
                            }
                        }
                    }
                }
            }

            // draw onto the graphics
            var dataCount = singlePointList.Count();
            SolidBrush brush = null;
            Pen pen = null;
            for (int i = 0; i < singlePointList.Count(); i++)
            {
                brush = teamValues.ElementAt(i) == "t"
                            ? BrushColours.BrushTerrorist(dataCount)
                            : BrushColours.BrushCounterTerrorist(dataCount);
                pen = teamValues.ElementAt(i) == "t"
                            ? PenColours.PenTerrorist(dataCount)
                            : PenColours.PenCounterTerrorist(dataCount);

                heatmapLogicCenter.DrawFilledCircle(graphics, brush, pen, singlePointList.ElementAt(i), 10);
            }
            brush?.Dispose();
            pen?.Dispose();
        }

        public void GenerateHeatmapDataFirstKillPositions(OverviewInfo overviewInfo, List<AllOutputData> allOutputDataList, Graphics graphics)
        {
            List<LinePoints> linePointsList = new List<LinePoints>();
            List<string> teamValues = new List<string>();

            foreach (var allOutputData in allOutputDataList)
            {
                foreach (var round in allOutputData.AllStats.firstDamageStats)
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

                        linePointsList.Add(linePoints);
                        teamValues.Add(firstDamage.TeamSideShooter.ToLower());
                    }
                }
            }

            // draw onto the graphics
            var dataCount = linePointsList.Count();
            Pen pen = null;
            for (int i = 0; i < linePointsList.Count(); i++)
            {
                pen = teamValues.ElementAt(i) == "terrorist"
                                ? PenColours.PenTerrorist(dataCount)
                                : PenColours.PenCounterTerrorist(dataCount);

                heatmapLogicCenter.DrawLine(graphics, pen, linePointsList.ElementAt(i));
            }
            pen?.Dispose();
        }

        public void GenerateHeatmapDataBombPlantLocations(OverviewInfo overviewInfo, List<AllOutputData> allOutputDataList, Graphics graphics)
        {
            List<PointF> singlePointList = new List<PointF>();

            foreach (var allOutputData in allOutputDataList)
            {
                foreach (var round in allOutputData.AllStats.roundsStats)
                {
                    if (round.BombsitePlantedAt != null && round.BombPlantPositionX != null && round.BombPlantPositionY != null && round.BombPlantPositionZ != null)
                    {
                        var bombPlantLocation = new Vector() { X = (int)round.BombPlantPositionX, Y = (int)round.BombPlantPositionY, Z = (int)round.BombPlantPositionZ };

                        PointF singlePoint = heatmapLogicCenter.CreateSinglePoint(overviewInfo, bombPlantLocation);

                        singlePointList.Add(singlePoint);
                    }
                }
            }

            // draw onto the graphics
            var dataCount = singlePointList.Count();
            Pen pen = null;
            SolidBrush brush = null;
            foreach (var singlePoint in singlePointList)
            {
                brush = BrushColours.BrushBombplant(dataCount);
                pen = PenColours.PenBombplant(dataCount);

                heatmapLogicCenter.DrawFilledCircle(graphics, brush, pen, singlePoint, 4);
            }
            brush?.Dispose();
            pen?.Dispose();
        }

        public void GenerateHeatmapDataHostageRescueLocations(OverviewInfo overviewInfo, List<AllOutputData> allOutputDataList, Graphics graphics)
        {
            List<PointF> singlePointList = new List<PointF>();

            foreach (var allOutputData in allOutputDataList)
            {
                foreach (var round in allOutputData.AllStats.roundsStats)
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

                        singlePointList.Add(singlePoint);
                    }
                }
            }

            // draw onto the graphics
            var dataCount = singlePointList.Count();
            Pen pen = null;
            SolidBrush brush = null;
            foreach (var singlePoint in singlePointList)
            {
                brush = BrushColours.BrushHostageRescue(dataCount);
                pen = PenColours.PenHostageRescue(dataCount);

                heatmapLogicCenter.DrawFilledCircle(graphics, brush, pen, singlePoint, 4);
            }
            brush?.Dispose();
            pen?.Dispose();
        }

        public void GenerateHeatmapDataGrenadeLocations(OverviewInfo overviewInfo, List<AllOutputData> allOutputDataList, Graphics graphics, string grenadeType)
        {
            List<PointF> singlePointList = new List<PointF>();

            foreach (var allOutputData in allOutputDataList)
            {
                foreach (var grenade in allOutputData.AllStats.grenadesSpecificStats)
                {
                    if (grenade.NadeType.ToLower() == grenadeType)
                    {
                        Vector location = new Vector() { X = (float)grenade.XPosition, Y = (float)grenade.YPosition, Z = (float)grenade.ZPosition };

                        PointF singlePoint = heatmapLogicCenter.CreateSinglePoint(overviewInfo, location);

                        singlePointList.Add(singlePoint);
                    }
                }
            }

            // draw onto the graphics
            var dataCount = singlePointList.Count();
            Pen pen = null;
            SolidBrush brush = null;
            foreach (var singlePoint in singlePointList)
            {
                brush = grenadeType switch
                {
                    "smoke" => BrushColours.BrushGrenadeSmoke(dataCount),
                    "flash" => BrushColours.BrushGrenadeFlash(dataCount),
                    "he" => BrushColours.BrushGrenadeHE(dataCount),
                    "incendiary" => BrushColours.BrushGrenadeIncendiary(dataCount),
                    "decoy" => BrushColours.BrushGrenadeDecoy(dataCount),
                };
                pen = grenadeType switch
                {
                    "smoke" => PenColours.PenGrenadeSmoke(dataCount),
                    "flash" => PenColours.PenGrenadeFlash(dataCount),
                    "he" => PenColours.PenGrenadeHE(dataCount),
                    "incendiary" => PenColours.PenGrenadeIncendiary(dataCount),
                    "decoy" => PenColours.PenGrenadeDecoy(dataCount),
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
            }
            brush?.Dispose();
            pen?.Dispose();
        }
    }
}
