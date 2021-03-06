﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SourceEngine.Heatmap.Generator.Constants
{
	public static class TransparencyMultiplierBorders
	{
		private static int border0 = 0;
		private static int border1 = 1;
		private static int border2 = 10;
		private static int border3 = 100;
		private static int border4 = 1000;
		private static int border5 = 10000;
		private static int border6 = 100000;

		private static int opacityMultiplier0 = 100;
		private static int opacityMultiplier1 = 50;
		private static int opacityMultiplier2 = 20;
		private static int opacityMultiplier3 = 10;
		private static int opacityMultiplier4 = 5;
		private static int opacityMultiplier5 = 2;
		private static int opacityMultiplier6 = 1;

		public static int GetMultiplier(int dataCount)
		{
			switch (dataCount)
			{
				case var _ when dataCount > border6:
					return opacityMultiplier6;
				case var _ when dataCount > border5:
					return opacityMultiplier5;
				case var _ when dataCount > border4:
					return opacityMultiplier4;
				case var _ when dataCount > border3:
					return opacityMultiplier3;
				case var _ when dataCount > border2:
					return opacityMultiplier2;
				case var _ when dataCount > border1:
					return opacityMultiplier1;
				case var _ when dataCount > border0:
					return opacityMultiplier0;
				default:
					return 0;
			}
		}
	}
}
