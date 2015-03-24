using UnityEngine;
using System.Collections.Generic;
using Ghostbit.Tweaker.UI;
using Ghostbit.Tweaker.Core;

namespace Ghostbit.Tweaker.Console
{
	public class TweakerConsole : MonoBehaviour
	{
		public HexTileView HexTilePrefab;
		public GameObject GridPanel; 

		private ITweakerLogger logger = LogManager.GetCurrentClassLogger();
		private Tweaker tweaker;
		private TweakerTree tree;
		private HexGrid<BaseNode> grid;
		private HexTileView[] orderedViews;

		private BaseNode currentTweakerNode;

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
			
			const uint gridWidth = 7;
			const uint gridHeight = 5;
			grid = new HexGrid<BaseNode>(gridWidth, gridHeight);
			logger.Debug("grid(5x5) created");

			orderedViews = new HexTileView[gridWidth * gridHeight];

			int cellCounter = 0;
			foreach (var cell in grid.GetSpiralCells(CubeCoord.Origin, 5))
			{
				HexTileView view = Instantiate(HexTilePrefab) as HexTileView;
				var rectTransform = view.GetComponent<RectTransform>();
				rectTransform.SetParent(GridPanel.GetComponent<RectTransform>(), false);
				view.name = cell.AxialCoord.ToString();

				float tileSize = 0.14f * (Screen.width / 2f);
				float tileHeight = tileSize * 2f;
				float tileWidth = Mathf.Sqrt(3f) / 2f * tileHeight;

				Vector2 position = HexCoord.AxialToPixel(cell.AxialCoord, tileSize);
				//logger.Debug("tile position = {0}  ({1},{2})  d={3}", cell.AxialCoord, position.x, position.y, position.magnitude);

				rectTransform.anchoredPosition = position;

				// width and height are swapped because cell is rotated 90 degrees in order to create a pointy top cell
				// instead of a flat top cell. (visual preference)
				rectTransform.sizeDelta = new Vector2(tileHeight, tileWidth);
				rectTransform.localScale = new Vector3(0.95f, 0.95f, 0.95f);

				view.Cell = cell;
				orderedViews[cellCounter++] = view;
			}

			DisplayTweakerNode(tree.Tree.Root);
		}

		private void DisplayTweakerNode(BaseNode parentNode)
		{
			if(currentTweakerNode == parentNode)
			{
				return;
			}

			var oldNode = currentTweakerNode;
			currentTweakerNode = parentNode;

			int numChildren = parentNode.Children.Count;
			HexTileView rootView = orderedViews[0];
			rootView.Cell.Value = parentNode;
			ConfigureView(rootView);

			for (int i = 1; i < numChildren + 1; ++i)
			{
				if (numChildren + 1 > orderedViews.Length)
				{
					logger.Error("Node has more children than there are available views.");
					return;
				}

				HexTileView view = orderedViews[i];
				BaseNode childNode = parentNode.Children[i-1];
				view.Cell.Value = childNode;
				ConfigureView(view);
			}

			for(int i = numChildren + 1; i < orderedViews.Length; ++i)
			{
				HexTileView view = orderedViews[i];
				view.Cell.Value = null;
				ConfigureView(view);
			}
		}

		private void ConfigureView(HexTileView view)
		{
			if (view.Cell.Value == null)
			{
				ConfigureEmptyView(view);
			}
			else
			{
				// Reasonable Defaults
				view.TileColor = Color.white;
				view.TileAlpha = 1f;

				BaseNode baseNode = view.Cell.Value;
				switch(baseNode.Type)
				{
					case BaseNode.NodeType.Root:
						ConfigureRootView(view);
						break;
					case BaseNode.NodeType.Group:
						ConfigureGroupView(view);
						break;
					case BaseNode.NodeType.Invokable:
						ConfigureInvokableView(view);
						break;
					case BaseNode.NodeType.Tweakable:
						ConfigureTweakableView(view);
						break;
					case BaseNode.NodeType.Watchable:
						ConfigureWatchableView(view);
						break;
					default:
						ConfigureUnkownView(view);
						break;
					
				}
				
			}
		}

		private void ConfigureEmptyView(HexTileView view)
		{
			view.TileAlpha = 0.25f;
			view.Name = "";
		}

		private void ConfigureUnkownView(HexTileView view)
		{
			view.TileAlpha = 0.6f;
			view.Name = "<Unknown Type>";
		}

		private void ConfigureRootView(HexTileView view)
		{
			view.Name = "ROOT";
			Color newColor = new Color(.2f, .2f, .2f, 1f);
			view.TileColor = newColor;
			view.NameText.color = Color.white;
		}

		private void ConfigureGroupView(HexTileView view)
		{
			var node = view.Cell.Value as GroupNode;
			view.Name = node.ShortName;
			view.TileColor = Color.white;
		}

		private void ConfigureInvokableView(HexTileView view)
		{
			var node = view.Cell.Value as InvokableNode;
			view.Name = node.Invokable.ShortName;
			view.TileColor = Color.blue;
			view.NameText.color = Color.white;
		}

		private void ConfigureTweakableView(HexTileView view)
		{
			var node = view.Cell.Value as TweakableNode;
			view.Name = node.Tweakable.ShortName;
			view.TileColor = Color.cyan;
		}

		private void ConfigureWatchableView(HexTileView view)
		{
			var node = view.Cell.Value as WatchableNode;
			view.Name = node.Watchable.ShortName;
			view.TileColor = Color.magenta;
		}
	}
}