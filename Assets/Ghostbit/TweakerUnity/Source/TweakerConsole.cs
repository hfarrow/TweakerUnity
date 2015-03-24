using UnityEngine;
using System.Collections;
using Ghostbit.Tweaker.UI;
using Ghostbit.Tweaker.Core;

namespace Ghostbit.Tweaker.Console
{
	public class TweakerConsole : MonoBehaviour
	{
		public GameObject HexTilePrefab;
		public GameObject GridPanel; 

		private ITweakerLogger logger = LogManager.GetCurrentClassLogger();
		private Tweaker tweaker;
		private TweakerTree tree;
		private HexGrid<BaseNode> grid;

		void Awake()
		{
			
		}

		// Use this for initialization
		void Start()
		{

		}

		public void Init(Tweaker tweaker)
		{
			logger.Info("Init: " + tweaker);
			this.tweaker = tweaker;

			tree = new TweakerTree(this.tweaker);
			tree.BuildTree();

			grid = new HexGrid<BaseNode>(5, 5);
			logger.Debug("grid(5x5) created");

			foreach (var cell in grid.GetRingCells(CubeCoord.Origin, 2))
			{
				GameObject cellObj = Instantiate(HexTilePrefab) as GameObject;
				var rectTransform = cellObj.GetComponent<RectTransform>();
				rectTransform.SetParent(GridPanel.GetComponent<RectTransform>(), false);
				cellObj.name = cell.AxialCoord.ToString();

				float tileSize = 0.125f * (Screen.width / 2f);
				float tileHeight = tileSize * 2f;
				float tileWidth = Mathf.Sqrt(3f) / 2f * tileHeight;

				logger.Debug("tileSize={0}", tileSize);
				logger.Debug("tileHeight={0}", tileHeight);
				logger.Debug("tileWidth={0}", tileWidth);

				Vector2 position = HexCoord.AxialToPixel(cell.AxialCoord, tileSize);
				logger.Debug("tile position = {0}  ({1},{2})  d={3}", cell.AxialCoord, position.x, position.y, position.magnitude);

				rectTransform.anchoredPosition = position;

				// width and height are swapped because cell is rotated 90 degrees.
				// TODO: rotate the png and reimport
				rectTransform.sizeDelta = new Vector2(tileHeight, tileWidth);
				rectTransform.localScale = new Vector3(0.95f, 0.95f, 0.95f);
			}
		}
	}
}