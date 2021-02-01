using SourceEngine.Demo.Parser;
using SourceEngine.Demo.Stats.Models;
using SourceEngine.Heatmap.Generator.Constants;
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


        /*
        
        /// <summary>
        /// Scales the position values to line up with the overview correctly when exported.
        /// Returns a single point.
        /// </summary>
        /// <param name="overviewInfo"></param>
        /// <param name="pointData"></param>
        /// <returns></returns>
        public PointF CreateSinglePoint(OverviewInfo overviewInfo, Vector pointData)
        {
            var xPoint = Math.Abs(Convert.ToInt32(Math.Abs(Math.Abs((Convert.ToSingle(pointData.X)) - overviewInfo.OffsetX)) / overviewInfo.Scale));
            var yPoint = Math.Abs(Convert.ToInt32(Math.Abs(Math.Abs((Convert.ToSingle(pointData.Y)) - overviewInfo.OffsetY)) / overviewInfo.Scale));

            return new PointF()
            {
                X = xPoint,
                Y = yPoint,
            };
        }
        
        */





        /// <summary>
        /// Scales the position values to line up with the overview correctly when exported for objective heatmaps.
        /// Returns a rectangle used for cropping images.
        /// </summary>
        /// <param name="overviewInfo"></param>
        /// <param name="pointData"></param>
        /// <param name="marginMultiplier"></param>
        /// <returns></returns>
        public RectangleBorders CreateRectangleBorders(OverviewInfo overviewInfo, PointsData pointsData, Bitmap overviewImage, float marginMultiplier = 5, bool squarePadding = false)
        {
            var point1 = CreateSinglePoint(overviewInfo, new Vector() { X = Convert.ToSingle(pointsData.DataForPoint1X), Y = Convert.ToSingle(pointsData.DataForPoint1Y) });
            var point2 = CreateSinglePoint(overviewInfo, new Vector() { X = Convert.ToSingle(pointsData.DataForPoint2X), Y = Convert.ToSingle(pointsData.DataForPoint2Y) });

            var xPointLeft = point1.X <= point2.X ? Math.Abs(point1.X) : Math.Abs(point2.X);
            var yPointTop = point1.Y <= point2.Y ? Math.Abs(point1.Y) : Math.Abs(point2.Y);
            var xPointRight = point1.X <= point2.X ? Math.Abs(point2.X) : Math.Abs(point1.X);
            var yPointBottom = point1.Y <= point2.Y ? Math.Abs(point2.Y) : Math.Abs(point1.Y);

            var marginX = (xPointRight - xPointLeft) / marginMultiplier;
            var marginY = (yPointBottom - yPointTop) / marginMultiplier;

            var xPointLeftMargined = (int)Math.Floor(xPointLeft - marginX);
            var yPointTopMargined = (int)Math.Ceiling(yPointTop - marginY);
            var xPointRightMargined = (int)Math.Ceiling(xPointRight + marginX);
            var yPointBottomMargined = (int)Math.Floor(yPointBottom + marginY);

            // add padding to make the output image square
            if (squarePadding)
            {
                var width = xPointRightMargined - xPointLeftMargined;
                var height = yPointBottomMargined - yPointTopMargined;
                var difference = Math.Abs(width - height);

                if (difference > 0)
                {
                    var differenceHalved = (double)difference / 2;

                    var padding1 = (int)Math.Ceiling(differenceHalved);
                    var padding2 = (int)Math.Floor(differenceHalved);

                    if (width > height)
                    {
                        yPointTopMargined -= padding1;
                        yPointBottomMargined += padding2;
                    }
                    else
                    {
                        xPointLeftMargined -= padding1;
                        xPointRightMargined += padding2;
                    }
                }

                // check if all points are within the image's border, if not add the removed amount onto the other side
                if (xPointLeftMargined < 0)
                {
                    xPointRightMargined += -xPointLeftMargined;
                    xPointLeftMargined = 0;
                }

                else if (yPointTopMargined < 0)
                {
                    yPointBottomMargined += -yPointTopMargined;
                    yPointTopMargined = 0;
                }
                else if (xPointRightMargined > overviewImage.Width)
                {
                    xPointLeftMargined += -xPointRightMargined;
                    xPointRightMargined = overviewImage.Width;
                }

                else if (yPointBottomMargined > overviewImage.Height)
                {
                    yPointTopMargined += -yPointBottomMargined;
                    yPointBottomMargined = overviewImage.Height;
                }
            }

            // final check to ensure all points are within the image's border
            xPointLeftMargined = (xPointLeftMargined < 0) ? 0 : xPointLeftMargined;
            yPointTopMargined = (yPointTopMargined < 0) ? 0 : yPointTopMargined;
            xPointRightMargined = (xPointRightMargined > overviewImage.Width) ? overviewImage.Width : xPointRightMargined;
            yPointBottomMargined = (yPointBottomMargined > overviewImage.Height) ? overviewImage.Height : yPointBottomMargined;

            return new RectangleBorders()
            {
                PointLeft = xPointLeftMargined,
                PointTop = yPointTopMargined,
                PointRight = xPointRightMargined,
                PointBottom = yPointBottomMargined,
            };
        }

        /// <summary>
        /// Scales the position values to line up with the overview correctly when exported for objective heatmaps.
        /// Returns a rectangle used for cropping images.
        /// </summary>
        /// <param name="overviewInfo"></param>
        /// <param name="pointData"></param>
        /// <param name="marginMultiplier"></param>
        /// <returns></returns>
        public Rectangle CreateRectangleObjective(OverviewInfo overviewInfo, PointsData pointsData, Bitmap overviewImage, float marginMultiplier)
        {
            RectangleBorders rectangleBorders = CreateRectangleBorders(overviewInfo, pointsData, overviewImage, marginMultiplier, false);

            return Rectangle.FromLTRB((int)rectangleBorders.PointLeft, (int)rectangleBorders.PointTop, (int)rectangleBorders.PointRight, (int)rectangleBorders.PointBottom);
        }

        /// <summary>
        /// Scales the position values to line up with the overview correctly when exported for objective heatmaps.
        /// Returns a rectangle used for cropping images, with additional padding to the margins to become squared.
        /// </summary>
        /// <param name="overviewInfo"></param>
        /// <param name="pointData"></param>
        /// <param name="marginMultiplier"></param>
        /// <returns></returns>
        public Rectangle CreateRectangleObjectiveSquarePadding(OverviewInfo overviewInfo, PointsData pointsData, Bitmap overviewImage, float marginMultiplier)
        {
            RectangleBorders rectangleBorders = CreateRectangleBorders(overviewInfo, pointsData, overviewImage, marginMultiplier, true);

            return Rectangle.FromLTRB((int)rectangleBorders.PointLeft, (int)rectangleBorders.PointTop, (int)rectangleBorders.PointRight, (int)rectangleBorders.PointBottom);
        }

        public void DrawLine(Graphics graphics, Pen pen, LinePoints linePoints)
        {
            var point1 = new PointF(linePoints.Point1.X * Resolution.resolutionMultiplier, linePoints.Point1.Y * Resolution.resolutionMultiplier);
            var point2 = new PointF(linePoints.Point2.X * Resolution.resolutionMultiplier, linePoints.Point2.Y * Resolution.resolutionMultiplier);

            graphics.DrawLine(pen, point1, point2);
        }

        public void DrawCurve(Graphics graphics, Pen pen, LinePoints linePoints)
        {
            var point1 = new PointF() { X = linePoints.Point1.X * Resolution.resolutionMultiplier, Y = linePoints.Point1.Y * Resolution.resolutionMultiplier };
            var point2 = new PointF() { X = linePoints.Point2.X * Resolution.resolutionMultiplier, Y = linePoints.Point2.Y * Resolution.resolutionMultiplier };

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


        public void DrawCircle(Graphics graphics, Pen pen, PointF point, int diameter = 10)
        {
            var diameterMultiplied = diameter * Resolution.resolutionMultiplier;
            RectangleF rect = new RectangleF((point.X * Resolution.resolutionMultiplier) - (diameterMultiplied / 2), (point.Y * Resolution.resolutionMultiplier) - (diameterMultiplied / 2), diameterMultiplied, diameterMultiplied);

            graphics.DrawEllipse(pen, rect);
        }

        public void DrawFilledCircle(Graphics graphics, SolidBrush brush, Pen pen, PointF point, int diameter = 10)
        {
            var diameterMultiplied = diameter * Resolution.resolutionMultiplier;
            RectangleF rect = new RectangleF((point.X * Resolution.resolutionMultiplier) - (diameterMultiplied / 2), (point.Y * Resolution.resolutionMultiplier) - (diameterMultiplied / 2), diameterMultiplied, diameterMultiplied);

            graphics.DrawEllipse(pen, rect);
            graphics.FillEllipse(brush, rect);
        }


        /* Objective ones do not take resolutionMultiplier into account as the images are cropped */

        public void DrawCircleObjective(Graphics graphics, Pen pen, PointF point, int diameter = 10)
        {
            RectangleF rect = new RectangleF(point.X - (diameter / 2), point.Y - (diameter / 2), diameter, diameter);

            graphics.DrawEllipse(pen, rect);
        }

        public void DrawFilledCircleObjective(Graphics graphics, SolidBrush brush, Pen pen, PointF point, int diameter = 10)
        {
            RectangleF rect = new RectangleF(point.X - (diameter / 2), point.Y - (diameter / 2), diameter, diameter);

            graphics.DrawEllipse(pen, rect);
            graphics.FillEllipse(brush, rect);
        }
    }
}
