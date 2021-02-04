using System;
using System.Collections.Generic;
using System.Text;

namespace SourceEngine.Heatmap.Generator.Constants
{
	public static class BrushWidthMultiplierBorders
	{
		private static int border0 = 0;
		private static int border1 = 1;
		private static int border2 = 10;
		private static int border3 = 100;
		private static int border4 = 1000;
		private static int border5 = 10000;
		private static int border6 = 100000;

		private static int width0 = 0;
		private static int width1 = 15;
		private static int width2 = 10;
		private static int width3 = 6;
		private static int width4 = 4;
		private static int width5 = 2;
		private static int width6 = 1;

		public static float widthObjectiveMultiplier = 0.25f;

		public static int GetMultiplier(int dataCount)
		{
			var width = 0;

			switch (dataCount)
			{
				case var _ when dataCount > border6:
					width = width6;
					break;
				case var _ when dataCount > border5:
					width = width5;
					break;
				case var _ when dataCount > border4:
					width = width4;
					break;
				case var _ when dataCount > border3:
					width = width3;
					break;
				case var _ when dataCount > border2:
					width = width2;
					break;
				case var _ when dataCount > border1:
					width = width1;
					break;
				case var _ when dataCount > border0:
					width = width0;
					break;
			}

			return width > 0 ? width : 1;
		}
	}
}
