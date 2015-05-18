using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Ghostbit.Tweaker.UI
{
	public static class CoordExtensions
	{
		public static Vector2 ToVector(this PixelCoord coord)
		{
			return new Vector2(coord.x, coord.y);
		}
	}
}
