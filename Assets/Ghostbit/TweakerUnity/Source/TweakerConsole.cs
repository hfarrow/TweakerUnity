using UnityEngine;
using System.Collections.Generic;
using Ghostbit.Tweaker.UI;
using Ghostbit.Tweaker.Core;
using System.Collections;
using System;

namespace Ghostbit.Tweaker.Console
{
	public class TweakerConsole : MonoBehaviour
	{
		private class CellViewPair
		{
			public HexGridCell<BaseNode> cell;
			public TileView view;
			public readonly uint index;

			public CellViewPair(HexGridCell<BaseNode> cell, TileView view, uint index)
			{
				this.cell = cell;
				this.view = view;
				this.index = index;
			}
		}

		public TileView DefaultTileViewPrefab;
		public TweakableTileView TweakableTileViewPrefab;
		public Dictionary<Type, Component> tilePrefabMap;

		public GameObject GridPanel; 

		private ITweakerLogger logger = LogManager.GetCurrentClassLogger();
		private Tweaker tweaker;
		private TweakerTree tree;
		private HexGrid<BaseNode> grid;
		private CellViewPair[] orderedTiles;

		private BaseNode currentTweakerNode;

		private const uint GRID_WIDTH = 7;
		private const uint GRID_HEIGHT = 5;
		private Vector3 selectedTileScale = new Vector3(1.6f, 1.6f, 1f);
		private Vector3 deselectedTileScale = new Vector3(.95f, .95f, 1f);

		void Awake()
		{
			tilePrefabMap = new Dictionary<Type, Component>()
			{
				{ typeof(RootNode), DefaultTileViewPrefab },
				{ typeof(GroupNode), DefaultTileViewPrefab },
				{ typeof(TweakableNode), TweakableTileViewPrefab },
				{ typeof(InvokableNode), DefaultTileViewPrefab },
				{ typeof(WatchableNode), DefaultTileViewPrefab }
			};
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
			orderedTiles = new CellViewPair[GRID_WIDTH * GRID_HEIGHT];

			uint cellCounter = 0;
			foreach (var cell in grid.GetSpiralCells(CubeCoord.Origin, 5))
			{
				orderedTiles[cellCounter] = new CellViewPair(cell, null, cellCounter);
				++cellCounter;
			}

			DisplayNode(tree.Tree.Root);
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
				BaseNode oldNode = currentTweakerNode;
				currentTweakerNode = parentNode;

				int numChildren = parentNode.Children.Count;

				CellViewPair rootPair = orderedTiles[0];
				rootPair.cell.Value = parentNode;

				if (rootPair.view == null || oldNode.GetType() != parentNode.GetType())
				{
					SetupTileView(rootPair);
				}

				TileView rootView = rootPair.view;
				ConfigureView(rootPair);

				for (int i = 1; i < orderedTiles.Length; ++i)
				{
					CellViewPair pair = orderedTiles[i];
					logger.Trace("Prepare next tile: cell={0} view={1} index={2}", pair.cell, pair.view, pair.index);

					if (i <= numChildren)
					{
						// Populated cells
						if (i >= orderedTiles.Length)
						{
							logger.Error("Node has more children than there are available views.");
							return;
						}

						BaseNode childNode = parentNode.Children[i - 1];
						pair.cell.Value = childNode;
						if (pair.view == null || pair.cell.Value.GetType() != childNode.GetType())
						{
							SetupTileView(pair);
						}

						TileView view = pair.view;
						view.Scale = deselectedTileScale;
						view.gameObject.SetActive(true);
						ConfigureView(pair);
					}
					else
					{
						if (pair.view != null)
						{
							Destroy(pair.view.gameObject);
							pair.view = null;
						}
					}
				}
			}
			//	else
			//		if invokable with args: show args as "tweakables"
		}

		private void SetupTileView(CellViewPair pair)
		{
			logger.Trace("SetupTileView: cell={0}, view={1}, index={2}", pair.cell, pair.view, pair.index);
			if (pair.view != null)
			{
				Destroy(pair.view.gameObject);
				pair.view = null;
			}
			
			if (pair.cell == null)
			{
				logger.Error("A cell instance must be provided for index {0}", pair.index);
				return;
			}

			logger.Trace("pair.cell.Value = {0}", pair.cell.Value);

			HexGridCell<BaseNode> cell = pair.cell;
			Component viewPrefab;
			logger.Trace("cell.Value={0}", cell.Value);
			if (!tilePrefabMap.TryGetValue(cell.Value.GetType(), out viewPrefab))
			{
				logger.Error("Could not find a prefab mapping for type '{0}'.", cell.Value.GetType().FullName);
				viewPrefab = DefaultTileViewPrefab;
			}

			TileView view = Instantiate(viewPrefab) as TileView;
			if (view == null)
			{
				logger.Error("Failed to Instantiate view prefab <{0}> as TileView", viewPrefab);
				return;
			}

			pair.view = view;
			var viewTransform = view.GetComponent<RectTransform>();
			viewTransform.SetParent(GridPanel.GetComponent<RectTransform>(), false);
			view.name = cell.AxialCoord.ToString();
			view.Tapped += (TileView v) => { OnTileTapped(v, cell); };
			view.Selected += (TileView v) => { OnTileSelected(v, cell); };
			view.Deselected += (TileView v) => { OnTileDeselected(v, cell); };

			float tileSize = 0.09f * Screen.width;
			float tileHeight = tileSize * 2f;
			float tileWidth = Mathf.Sqrt(3f) / 2f * tileHeight;

			Vector2 position = HexCoord.AxialToPixel(cell.AxialCoord, tileSize);
			//logger.Debug("tile position = {0}  ({1},{2})  d={3}", cell.AxialCoord, position.x, position.y, position.magnitude);

			viewTransform.anchoredPosition = position;
			viewTransform.sizeDelta = new Vector2(tileWidth, tileHeight);
		}

		private void ConfigureView(CellViewPair pair)
		{
			TileView view = pair.view;
			BaseNode node = pair.cell.Value;

			logger.Trace("ConfigureView: node={0} view={1} index={2}", node, view, pair.index);

			// Reasonable Defaults
			view.TileColor = Color.white;
			view.TileAlpha = 1f;
			view.NameText.color = Color.black;

			if (node.Value == null)
			{
				ConfigureEmptyView(view, node);
			}
			else
			{
				BaseNode baseNode = node.Value;
				switch(baseNode.Type)
				{
					case BaseNode.NodeType.Root:
						ConfigureRootView(view, node);
						break;
					case BaseNode.NodeType.Group:
						ConfigureGroupView(view, node as GroupNode);
						break;
					case BaseNode.NodeType.Invokable:
						ConfigureInvokableView(view, node as InvokableNode);
						break;
					case BaseNode.NodeType.Tweakable:
						ConfigureTweakableView(view, node as TweakableNode);
						break;
					case BaseNode.NodeType.Watchable:
						ConfigureWatchableView(view, node as WatchableNode);
						break;
					default:
						ConfigureUnkownView(view, node);
						break;	
				}
			}
		}

		private void ConfigureEmptyView(TileView view, BaseNode node)
		{
			view.TileAlpha = 0.25f;
			view.Name = "";
			view.FullName = "";
		}

		private void ConfigureUnkownView(TileView view, BaseNode node)
		{
			view.TileAlpha = 0.6f;
			view.Name = "<Unknown Type>";
			view.FullName = "";
		}

		private void ConfigureRootView(TileView view, BaseNode node)
		{
			logger.Trace("ConfigureRootView: view={0}<{1}> node={2}", view, view.GetType().FullName, node);
			GroupNode groupNode = node as GroupNode;
			if (groupNode != null)
			{
				view.Name = groupNode.ShortName;
			}
			else
			{
				view.Name = "ROOT";
				view.FullName = "Root Node";
			}
			Color newColor = new Color(.2f, .2f, .2f, 1f);
			view.TileColor = newColor;
			view.NameText.color = Color.white;
		}

		private void ConfigureGroupView(TileView view, GroupNode node)
		{
			if (node == currentTweakerNode)
			{
				// Treat the current node as a "root" node for display purposes.
				ConfigureRootView(view, node);
			}
			else
			{
				view.Name = node.ShortName;
				view.TileColor = Color.white;
			}

			view.FullName = node.FullName;
		}

		private void ConfigureInvokableView(TileView view, InvokableNode node)
		{
			view.Name = node.Invokable.ShortName;
			view.TileColor = Color.blue;
			view.NameText.color = Color.white;
			view.FullName = node.Invokable.Name;
		}

		private void ConfigureTweakableView(TileView view, TweakableNode node)
		{
			view.Name = node.Tweakable.ShortName;
			view.TileColor = Color.cyan;
			view.FullName = node.Tweakable.Name;

			var tweakableView = view as TweakableTileView;
			tweakableView.Value = node.Tweakable.GetValue().ToString();
		}

		private void ConfigureWatchableView(TileView view, WatchableNode node)
		{
			view.Name = node.Watchable.ShortName;
			view.TileColor = Color.magenta;
			view.FullName = node.Watchable.Name;
		}

		private void OnTileTapped(TileView view, HexGridCell<BaseNode> node)
		{
			BaseNode baseNode = node.Value;
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

		private void OnTileSelected(TileView view, HexGridCell<BaseNode> cell)
		{
			logger.Trace("OnTileSelected: cell.Value={0}", cell.Value);
			if (cell.Value != null)
			{
				view.Scale = selectedTileScale;
				view.GetComponent<RectTransform>().SetAsLastSibling();
			}
		}

		private void OnTileDeselected(TileView view, HexGridCell<BaseNode> cell)
		{
			if (cell.Value != null)
			{
				view.Scale = deselectedTileScale;
			}
		}
	}
}