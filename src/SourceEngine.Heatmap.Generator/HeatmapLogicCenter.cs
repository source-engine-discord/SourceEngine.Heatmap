using SourceEngine.Demo.Parser;
using SourceEngine.Demo.Stats.Models;
using SourceEngine.Heatmap.Generator.Enums;
using SourceEngine.Heatmap.Generator.Models;
using System;
using System.Drawing;
using System.Linq;

namespace SourceEngine.Heatmap.Generator
{
	public class HeatmapLogicCenter
	{
		public HeatmapLogicCenter()
		{ }

        public Teams GetTeamOfPlayerInKill(AllStats allStats, int round, long steamId)
        {
            return allStats.teamStats.Where(x => x.Round == round).Select(x => x.TeamAlpha.Where(y => y == steamId)).FirstOrDefault().FirstOrDefault() != 0
                        ? Teams.Alpha
                        : Teams.Beta;
        }

        public Sides GetSideOfPlayerInKill(AllStats allStats, int round)
        {
            return allStats.roundsStats.Where(x => x.Round == round).Select(x => x.Half.ToLower()).FirstOrDefault() == "first"
                        ? Sides.Terrorists
                        : Sides.CounterTerrorists;
        }

        /// <summary>
        /// Scales the position values to line up with the overview correctly when exported.
        /// Returns two points that are used to join a line between.
        /// </summary>
        /// <param name="overviewInfo"></param>
        /// <param name="pointsData"></param>
        /// <returns></returns>
        public LinePoints CreateLinePoints(OverviewInfo overviewInfo, PointsData pointsData)
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

        /// <summary>
        /// Scales the position values to line up with the overview correctly when exported.
        /// Returns a single point.
        /// </summary>
        /// <param name="overviewInfo"></param>
        /// <param name="pointData"></param>
        /// <returns></returns>
        public PointF CreateSinglePoint(OverviewInfo overviewInfo, Vector pointData)
        {
            var xPoint = Math.Abs(Convert.ToInt32((Convert.ToSingle(pointData.X) - overviewInfo.OffsetX) / overviewInfo.Scale));
            var yPoint = Math.Abs(Convert.ToInt32((Convert.ToSingle(pointData.Y) - overviewInfo.OffsetY) / overviewInfo.Scale));

            return new PointF()
            {
                X = xPoint,
                Y = yPoint,
            };
        }

        public void DrawLine(Graphics graphics, Pen pen, LinePoints linePoints)
        {
            graphics.DrawLine(pen, linePoints.Point1, linePoints.Point2);
        }

        public void DrawCurve(Graphics graphics, Pen pen, LinePoints linePoints)
        {
            var point1 = new PointF() { X = linePoints.Point1.X, Y = linePoints.Point1.Y }; // are these 3 lines even needed ????
            var point2 = new PointF() { X = linePoints.Point2.X, Y = linePoints.Point2.Y };

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

            graphics.DrawCurve(pen, points);
        }

        public void DrawCircle(Graphics graphics, Pen pen, PointF point, int radius = 1)
        {
            RectangleF rect = new RectangleF(point.X, point.Y, radius, radius);

            graphics.DrawEllipse(pen, rect);
        }
    }
}
