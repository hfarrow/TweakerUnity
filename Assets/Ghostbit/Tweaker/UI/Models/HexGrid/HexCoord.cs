using System;
using System.Collections.Generic;

namespace Ghostbit.Tweaker.UI
{
	public struct CubeCoord
	{
		public static CubeCoord[] DIRECTIONS = 
		{
			new CubeCoord(1, -1, 0), new CubeCoord(1, 0, -1), new CubeCoord(0, 1, -1),
			new CubeCoord(-1, 1, 0), new CubeCoord(-1, 0, 1), new CubeCoord(0, -1, 1)
		};

		public int x;
		public int y;
		public int z;

		public CubeCoord()
		{
			x = 0;
			y = 0;
			z = 0;
		}

		public CubeCoord(int x, int y, int z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}

		public static CubeCoord FromFraction(double x, double y, double z)
		{
			double rx = Math.Round(x);
			double ry = Math.Round(y);
			double rz = Math.Round(z);

			double x_diff = Math.Abs(rx - x);
			double y_diff = Math.Abs(ry - y);
			double z_diff = Math.Abs(rz - z);

			if (x_diff > y_diff && x_diff > z_diff)
			{
				rx = -ry - rz;
			}
			else if(y_diff > z_diff)
			{
				ry = -rx - rz;
			}
			else
			{
				rz = -rx - ry;
			}

			return new CubeCoord((int)Math.Round((decimal)rx),
				(int)Math.Round((decimal)ry),
				(int)Math.Round((decimal)rz));
		}

		public int Distance(CubeCoord other)
		{
			return (Math.Abs(x - other.x) + Math.Abs(y - other.y) + Math.Abs(z - other.z)) / 2;
		}

		public static CubeCoord operator +(CubeCoord a, CubeCoord b)
		{
			return new CubeCoord(a.x + b.x, a.y + b.y, a.z + b.z);
		}

		public static CubeCoord operator -(CubeCoord a, CubeCoord b)
		{
			return new CubeCoord(a.x - b.x, a.y - b.y, a.z - b.z);
		}
	}

	public struct AxialCoord
	{
		public const AxialCoord[] DIRECTIONS = 
		{
			 new AxialCoord(1,0), new AxialCoord(1, -1), new AxialCoord(0, -1),
			 new AxialCoord(-1,0), new AxialCoord(-1, 1), new AxialCoord(0, 1)
		};

		public int q;
		public int r;

		public AxialCoord()
		{
			q = 0;
			r = 0;
		}

		public AxialCoord(int q, int r)
		{
			this.q = q;
			this.r = r;
		}

		public static AxialCoord operator +(AxialCoord a, AxialCoord b)
		{
			return new AxialCoord(a.q + b.q, a.r + b.r);
		}

		public static AxialCoord operator -(AxialCoord a, AxialCoord b)
		{
			return new AxialCoord(a.q - b.q, a.r - b.r);
		}

		public int Distance(AxialCoord other)
		{
			// Convert to cube coord and use CubeCoord.Distance
			CubeCoord ac;
			CubeCoord bc;
			HexCoord.AxialToCube(ref this, out ac);
			HexCoord.AxialToCube(ref other, out bc);
			return ac.Distance(bc);
		}
	}

	public static class HexCoord
	{
		public static void CubeToAxial(ref CubeCoord coord, out AxialCoord outCoord)
		{
			outCoord =  new AxialCoord(coord.x, coord.y);
		}

		public static AxialCoord CubeToAxial(CubeCoord coord)
		{
			return new AxialCoord(coord.x, coord.y);
		}

		public static void AxialToCube(ref AxialCoord coord, out CubeCoord outCoord)
		{
			outCoord = new CubeCoord(coord.q, -coord.q - coord.r, coord.r);
		}

		public static CubeCoord AxialToCube(AxialCoord coord)
		{
			return new CubeCoord(coord.q, -coord.q - coord.r, coord.r);
		}

		public static CubeCoord GetNeighbour(CubeCoord direction, CubeCoord cubeCoord)
		{
			return cubeCoord + direction;
		}

		public static AxialCoord GetNeighbour(AxialCoord direction, AxialCoord axialCoord)
		{
			return axialCoord + direction;
		}
	}
}
