using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ghostbit.Tweaker.UI
{
	public class GridCell<T>
	{
		private T value;
		public T Value { get { return value; } set { this.value = value; } }

		public Vector3 CubeCoord { get; private set; }
		public Vector2 AxialCoord { get; private set; }

		public GridCell(Vector2 axialCoord)
		{
			AxialCoord = axialCoord;
			CubeCoord = HexCoord.AxialToCube(axialCoord);
		}

		public GridCell(Vector2 axialCoord, T value)
		{
			AxialCoord = axialCoord;
			CubeCoord = HexCoord.AxialToCube(axialCoord);
			Value = value;
		}
	}

	public static class HexCoord
	{
		public static Vector2 CubeToAxial(Vector3 cube)
		{
			return new Vector2(cube.x, cube.y);
		}

		public static Vector3 AxialToCube(Vector2 axial)
		{
			return new Vector3(axial.x, axial.y, -axial.x - axial.y);
		}
	}

	public class HexGrid<TCellValue>
	{
		private Dictionary<string, GridCell<TCellValue>> cells;

		public HexGrid()
		{
			cells = new Dictionary<string, GridCell<TCellValue>>();

			// TODO: build the grid (Rectangular shape to fit screen)
		}

		public GridCell<TCellValue> GetCell(Vector2 axialCoord)
		{
			GridCell<TCellValue> cell = null;
			cells.TryGetValue(axialCoord.ToString(), out cell);
			// TODO: exception on invalid cell?
			return cell;
		}

		public GridCell<TCellValue> GetCell(Vector3 cubeCoord)
		{
			return GetCell(HexCoord.CubeToAxial(cubeCoord));
		}

		public TCellValue GetCellValue(Vector2 axialCoord)
		{
			return GetCell(axialCoord).Value;
		}

		public TCellValue GetCellValue(Vector3 cubeCoord)
		{
			return GetCell(cubeCoord).Value;
		}

		public void SetCellValue(TCellValue value, Vector3 cubeCoord)
		{
			var cell = GetCell(cubeCoord);
			cell.Value = value;
		}

		public void SetCellValue(TCellValue value, Vector2 axialCoord)
		{
			var cell = GetCell(axialCoord);
			cell.Value = value;
		}
	}
}
