using Ghostbit.Tweaker.Core;
using System;
using System.Collections.Generic;

namespace Ghostbit.Tweaker.UI
{
	public static class HexGridFactory
	{
		public static Dictionary<string, GridCell<TCellValue>> MakeRectangleGrid<TCellValue>(uint width, uint height)
		{
			var logger = LogManager.GetCurrentClassLogger();
			// array[r][q + r/2]

			var cells = new Dictionary<string, GridCell<TCellValue>>();

			// Offsets are to make (0,0) the center of the grid because
			// the grid is generated with (0,0) at the top left.
			int xOffset = (int)width / 2;
			int yOffset = (int)height / 2;

			for (int currentRow = 0; currentRow < height; currentRow++)
			{
				for (int currentColumn = 0; currentColumn < width; ++currentColumn)
				{
					// See http://www.redblobgames.com/grids/hexagons/#map-storage for how this logic
					// was derived.
					// Every other row in the grid adds -1 to an offset for q. [ -(currentRow / 2) + currentColumn]
					// r is simply the currentRow

					int q = -(currentRow / 2) + currentColumn - xOffset;
					int r = currentRow - yOffset;
					var cell = new GridCell<TCellValue>(new AxialCoord(q, r), default(TCellValue));
					cells.Add(cell.AxialCoord.ToString(), cell);
					logger.Trace("Created cell at: " + cell.AxialCoord + "  (" + cell.CubeCoord + ")");
				}
			}

			return cells;
		}

		public static Dictionary<string, GridCell<TCellValue>> MakeHexagonGrid<TCellValue>(uint radius)
		{
			// array[r + N][q + N + min(0, r)]
			throw new NotImplementedException();
		}

		public static Dictionary<string, GridCell<TCellValue>> MakeTriangleGrid<TCellValue>(uint sideLength)
		{
			// Half a Rhombus - if x + y > sideLength then discard cell.
			throw new NotImplementedException();
		}

		public static Dictionary<string, GridCell<TCellValue>> MakeRhombusGrid<TCellValue>(uint sideLength)
		{
			// array[r][q] - Simplest to implement, for(x < size){ for(y < size) { cell = (x,y) } }
			throw new NotImplementedException();
		}
	}
}
