using Ghostbit.Tweaker.Core;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ghostbit.Tweaker.UI
{
	public class HexGrid<TCellValue>
	{
		private ITweakerLogger logger = LogManager.GetCurrentClassLogger();
		private Dictionary<string, GridCell<TCellValue>> cells;

		public HexGrid(uint width, uint height)
		{
			cells = HexGridFactory.MakeRectangleGrid<TCellValue>(width, height);
		}

		public void ClearCells()
		{
			foreach (var cell in cells.Values)
			{
				cell.Value = default(TCellValue);
			}
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

		public IEnumerable<GridCell<TCellValue>> GetRingCells(CubeCoord center, uint radius)
		{
			logger.Trace("GetRingCells({0}, {1})", center, radius);
			var cube = HexCoord.GetNeighbour(center, 4);
			for (uint i = 0; i < 6; ++i)
			{
				for (uint j = 0; j < radius; ++j)
				{
					var cell = GetCell(cube);
					logger.Trace("yeild '{0}' @ '{1}' for {2},{3}", 
						cell != null ? cell.AxialCoord.ToString() : "null", cube, i, j);
					if( cell != null)
					{
						yield return cell;
					}
					cube = HexCoord.GetNeighbour(cube, (i));
				}
			}
		}

		public IEnumerable<GridCell<TCellValue>> GetSpiralCells(CubeCoord center, uint radius)
		{
			logger.Trace("GetRingCells({0}, {1})", center, radius);
			var centerCell = GetCell(center);
			if (centerCell != null)
			{
				yield return centerCell;
			}

			for (uint i = 1; i <= radius; ++i)
			{
				foreach (var cell in GetRingCells(center, i))
				{
					if( cell != null)
					{
						yield return cell;
					}
				}
			}
		}

		public IEnumerable<TCellValue> GetRingValues(CubeCoord center, uint radius)
		{
			foreach (var cell in GetRingCells(center, radius))
			{
				yield return cell.Value;
			}
		}

		public IEnumerable<TCellValue> GetSpiralValues(CubeCoord center, uint radius)
		{
			foreach (var cell in GetSpiralCells(center, radius))
			{
				yield return cell.Value;
			}
		}
	}
}
