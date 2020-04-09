using SourceEngine.Demo.Stats.Models;
using SourceEngine.Heatmap.Generator.Constants;
using SourceEngine.Heatmap.Generator.Enums;
using SourceEngine.Heatmap.Generator.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace SourceEngine.Heatmap.Generator
{
	public class HeatmapDataGatherer
	{
        public HeatmapDataGatherer()
        { }

        public void GenerateKillsHeatmap(OverviewInfo overviewInfo, AllStats allStats, Graphics graphics, Sides side)
        {
            foreach (var data in allStats.killsStats)
            {
                var killerTeam = GetTeamOfPlayerInKill(allStats, data.Round, data.KillerSteamID);
                var victimTeam = GetTeamOfPlayerInKill(allStats, data.Round, data.VictimSteamID);

                var killerSide = GetSideOfPlayerInKill(allStats, data.Round);

                if (killerTeam != victimTeam && killerSide == side)
                {
                    PointsData pointsData = new PointsData()
                    {
                        DataForPoint1X = data.XPositionKill,
                        DataForPoint1Y = data.YPositionKill,
                        DataForPoint2X = data.XPositionDeath,
                        DataForPoint2Y = data.YPositionDeath,
                    };

                    LinePoints linePoints = CreateLinePoints(overviewInfo, pointsData);

                    Pen pen = side == Sides.Terrorists
                                ? PenColours.PenTerrorist
                                : PenColours.PenCounterTerrorist;

                    DrawLine(graphics, pen, linePoints);
                }
            }
        }

        private Teams GetTeamOfPlayerInKill(AllStats allStats, int round, long steamId)
        {
            return allStats.teamStats.Where(x => x.Round == round).Select(x => x.TeamAlpha.Where(y => y == steamId)).FirstOrDefault().FirstOrDefault() != 0
                        ? Teams.Alpha
                        : Teams.Beta;
        }

        private Sides GetSideOfPlayerInKill(AllStats allStats, int round)
        {
            return allStats.roundsStats.Where(x => x.Round == round).Select(x => x.Half.ToLower()).FirstOrDefault() == "first"
                        ? Sides.Terrorists
                        : Sides.CounterTerrorists;
        }

        private LinePoints CreateLinePoints(OverviewInfo overviewInfo, PointsData pointsData)
        {

            var xPoint1 = Math.Abs(Convert.ToInt32((Convert.ToSingle(pointsData.DataForPoint1X) - overviewInfo.OffsetX) / overviewInfo.Scale));
            var yPoint1 = Math.Abs(Convert.ToInt32((Convert.ToSingle(pointsData.DataForPoint1Y) - overviewInfo.OffsetY) / overviewInfo.Scale));

            var xPoint2 = Math.Abs(Convert.ToInt32((Convert.ToSingle(pointsData.DataForPoint2X) - overviewInfo.OffsetX) / overviewInfo.Scale));
            var yPoint2 = Math.Abs(Convert.ToInt32((Convert.ToSingle(pointsData.DataForPoint2Y) - overviewInfo.OffsetY) / overviewInfo.Scale));

            return new LinePoints()
            {
                Point1 = new PointF() { X = xPoint1, Y = yPoint1 },
                Point2 = new PointF() { X = xPoint2, Y = yPoint2 },
            };
        }

        private void DrawLine(Graphics graphics, Pen pen, LinePoints linePoints)
        {
            graphics.DrawLine(pen, linePoints.Point1, linePoints.Point2);
        }

        /*private void DrawCurve(Graphics graphics, Pen pen, LinePoints linePoints)
        {
            var point1 = new PointF() { X = linePoints.Point1X, Y = linePoints.Point1Y };
            var point2 = new PointF() { X = linePoints.Point2X, Y = linePoints.Point2Y };

            var points = new PointF[] { point1, point2 };

            // gradient
            /*
            using (Brush aGradientBrush = new LinearGradientBrush(new Point(0, 0), new Point(100, 0), Color.Orange, Color.OrangeRed))
            {
                using (Pen aGradientPen = new Pen(aGradientBrush))
                {
                    graphics.DrawCurve(aGradientPen, points);
                }
            }
            */

        /*    graphics.DrawCurve(pen, points);
        }*/
    }
}
