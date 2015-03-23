using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ghostbit.Tweaker.UI
{
	public class HexGrid<TCellValue>
	{
		private Dictionary<string, GridCell<TCellValue>> cells;

		public HexGrid()
		{
			cells = new Dictionary<string, GridCell<TCellValue>>();

			// TODO: build the grid (Rectangular shape to fit screen)
		}

		public GridCell<TCellValue> GetCell(AxialCoord axialCoord)
		{
			GridCell<TCellValue> cell = null;
			cells.TryGetValue(axialCoord.ToString(), out cell);
			return cell;
		}

		public GridCell<TCellValue> GetCell(CubeCoord coord)
		{
			AxialCoord axialCoord;
			HexCoord.CubeToAxial(ref coord, out axialCoord);
			return GetCell(axialCoord);
		}

		public TCellValue GetCellValue(AxialCoord coord)
		{
			return GetCell(coord).Value;
		}

		public TCellValue GetCellValue(CubeCoord coord)
		{
			return GetCell(coord).Value;
		}

		public void SetCellValue(TCellValue value, CubeCoord coord)
		{
			var cell = GetCell(coord);
			cell.Value = value;
		}

		public void SetCellValue(TCellValue value, AxialCoord coord)
		{
			var cell = GetCell(coord);
			cell.Value = value;
		}
	}
}
