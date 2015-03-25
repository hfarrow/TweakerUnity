using UnityEngine;
using System.Collections.Generic;
using Ghostbit.Tweaker.UI;
using Ghostbit.Tweaker.Core;
using System.Collections;

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

		private const uint GRID_WIDTH = 7;
		private const uint GRID_HEIGHT = 5;
		private Vector3 selectedTileScale = new Vector3(1.6f, 1.6f, 1f);
		private Vector3 deselectedTileScale = new Vector3(.95f, .95f, 1f);

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
			grid = new HexGrid<BaseNode>(GRID_WIDTH, GRID_HEIGHT);
			orderedViews = new HexTileView[GRID_WIDTH * GRID_HEIGHT];

			int cellCounter = 0;
			foreach (var cell in grid.GetSpiralCells(CubeCoord.Origin, 5))
			{
				orderedViews[cellCounter++] = SetupCell(cell);
			}

			DisplayNode(tree.Tree.Root);
		}

		private HexTileView SetupCell(HexGridCell<BaseNode> cell)
		{
			HexTileView view = Instantiate(HexTilePrefab) as HexTileView;
			var viewTransform = view.GetComponent<RectTransform>();
			viewTransform.SetParent(GridPanel.GetComponent<RectTransform>(), false);
			view.name = cell.AxialCoord.ToString();
			view.Tapped += OnTileTapped;
			view.Selected += OnTileSelected;
			view.Deselected += OnTileDeselected;

			float tileSize = 0.18f * (Screen.width / 2f);
			float tileHeight = tileSize * 2f;
			float tileWidth = Mathf.Sqrt(3f) / 2f * tileHeight;

			Vector2 position = HexCoord.AxialToPixel(cell.AxialCoord, tileSize);
			//logger.Debug("tile position = {0}  ({1},{2})  d={3}", cell.AxialCoord, position.x, position.y, position.magnitude);

			viewTransform.anchoredPosition = position;
			viewTransform.sizeDelta = new Vector2(tileWidth, tileHeight);
			view.Cell = cell;

			return view;
		}

		private void DisplayNode(BaseNode parentNode)
		{
			logger.Debug("DisplayTweakerNode: {0}", parentNode.Type);
			if(currentTweakerNode == parentNode)
			{
				return;
			}

			if (parentNode.Type == BaseNode.NodeType.Group || parentNode.Type == BaseNode.NodeType.Root)
			{
				var oldNode = currentTweakerNode;
				currentTweakerNode = parentNode;

				int numChildren = parentNode.Children.Count;
				HexTileView rootView = orderedViews[0];
				rootView.Cell.Value = parentNode;
				ConfigureView(rootView);

				for (int i = 1; i < orderedViews.Length; ++i)
				{
					HexTileView view = orderedViews[i];
					var uiTransform = view.TileUI.GetComponent<RectTransform>();
					if (i <= numChildren)
					{
						// Populated cells
						if (i >= orderedViews.Length)
						{
							logger.Error("Node has more children than there are available views.");
							return;
						}

						BaseNode childNode = parentNode.Children[i - 1];
						view.Cell.Value = childNode;
						uiTransform.localScale = deselectedTileScale;
						view.gameObject.SetActive(true);
					}
					else
					{
						// Empty cells
						view.Cell.Value = null;
						view.gameObject.SetActive(false);
					}

					ConfigureView(view);
				}
			}
			//	else
			//		if invokable with args: show args as "tweakables"
		}

		private void ConfigureView(HexTileView view)
		{
			// Reasonable Defaults
			view.TileColor = Color.white;
			view.TileAlpha = 1f;
			view.NameText.color = Color.black;
			view.XText.text = view.Cell.AxialCoord.q.ToString();
			view.YText.text = view.Cell.AxialCoord.r.ToString();

			if (view.Cell.Value == null)
			{
				ConfigureEmptyView(view);
			}
			else
			{
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
			view.FullNameView.FullNameText.text = "";
		}

		private void ConfigureUnkownView(HexTileView view)
		{
			view.TileAlpha = 0.6f;
			view.Name = "<Unknown Type>";
			view.FullNameView.FullNameText.text = "";
		}

		private void ConfigureRootView(HexTileView view)
		{
			GroupNode node = view.Cell.Value as GroupNode;
			if (node != null)
			{
				view.Name = node.ShortName;
			}
			else
			{
				view.Name = "ROOT";
				view.FullNameView.FullNameText.text = "Root Node";
			}
			Color newColor = new Color(.2f, .2f, .2f, 1f);
			view.TileColor = newColor;
			view.NameText.color = Color.white;
		}

		private void ConfigureGroupView(HexTileView view)
		{
			var node = view.Cell.Value as GroupNode;
			if (node == currentTweakerNode)
			{
				// Treat the current node as a "root" node for display purposes.
				ConfigureRootView(view);
			}
			else
			{
				view.Name = node.ShortName;
				view.TileColor = Color.white;
			}

			view.FullNameView.FullNameText.text = node.FullName;
		}

		private void ConfigureInvokableView(HexTileView view)
		{
			var node = view.Cell.Value as InvokableNode;
			view.Name = node.Invokable.ShortName;
			view.TileColor = Color.blue;
			view.NameText.color = Color.white;
			view.FullNameView.FullNameText.text = node.Invokable.Name;
		}

		private void ConfigureTweakableView(HexTileView view)
		{
			var node = view.Cell.Value as TweakableNode;
			view.Name = node.Tweakable.ShortName;
			view.TileColor = Color.cyan;
			view.FullNameView.FullNameText.text = node.Tweakable.Name;
		}

		private void ConfigureWatchableView(HexTileView view)
		{
			var node = view.Cell.Value as WatchableNode;
			view.Name = node.Watchable.ShortName;
			view.TileColor = Color.magenta;
			view.FullNameView.FullNameText.text = node.Watchable.Name;
		}

		private void OnTileTapped(HexTileView view)
		{
			BaseNode baseNode = view.Cell.Value;
			if (baseNode == null)
			{
				// Tapped an empy cell
				return;
			}

			logger.Trace("OnTileTapped: {0}", baseNode.Type.ToString());
			switch (baseNode.Type)
			{
				case BaseNode.NodeType.Root:
					// ?
					break;
				case BaseNode.NodeType.Group:
					GroupNode group = baseNode.Value as GroupNode;
					logger.Trace("Group was tapped: {0}", group.FullName);
					if (group == currentTweakerNode)
					{
						DisplayNode(group.Parent);
					}
					else
					{
						DisplayNode(group);
					}
					break;
				case BaseNode.NodeType.Invokable:
					// invoke
					break;
				case BaseNode.NodeType.Tweakable:
					// edit
					break;
				case BaseNode.NodeType.Watchable:
					// ?
					break;
				default:
					logger.Warn("Unhandled node type tapped.");
					break;
			}
		}

		private void OnTileSelected(HexTileView view)
		{
			if (view.Cell.Value != null)
			{
				view.TileUI.GetComponent<RectTransform>().localScale = selectedTileScale;
				view.GetComponent<RectTransform>().SetAsLastSibling();
			}
		}

		private void OnTileDeselected(HexTileView view)
		{
			if (view.Cell.Value != null)
			{
				view.TileUI.GetComponent<RectTransform>().localScale = deselectedTileScale;
			}
		}
	}
}