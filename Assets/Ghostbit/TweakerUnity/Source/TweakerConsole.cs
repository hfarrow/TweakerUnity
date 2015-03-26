using UnityEngine;
using System.Collections.Generic;
using Ghostbit.Tweaker.UI;
using Ghostbit.Tweaker.Core;
using System.Collections;
using System;

namespace Ghostbit.Tweaker.UI
{
	public interface ITweakerConsole
	{
		BaseNode CurrentNode { get; }
		void DisplayNode(BaseNode nodeToDisplay);
	}

	public class TweakerConsole : MonoBehaviour, ITweakerConsole
	{
		public TileView DefaultTileViewPrefab;
		public TweakableTileView TweakableTileViewPrefab;

		public GameObject GridPanel; 

		private ITweakerLogger logger = LogManager.GetCurrentClassLogger();
		private Tweaker tweaker;
		private TweakerTree tree;
		private HexGrid<BaseNode> grid;
		private ITileController[] orderedControllers;
		private HexGridCell<BaseNode>[] orderedCells;
		private Dictionary<Type, TileView> tilePrefabMap;
		private TileViewFactory tileViewFactory;

		public BaseNode CurrentNode { get; private set; }

		private const uint GRID_WIDTH = 7;
		private const uint GRID_HEIGHT = 5;

		void Awake()
		{
			tilePrefabMap = new Dictionary<Type, TileView>()
			{
				{ typeof(RootNode), DefaultTileViewPrefab },
				{ typeof(GroupNode), DefaultTileViewPrefab },
				{ typeof(TweakableNode), TweakableTileViewPrefab },
				{ typeof(InvokableNode), DefaultTileViewPrefab },
				{ typeof(WatchableNode), DefaultTileViewPrefab }
			};
			tileViewFactory = new TileViewFactory(tilePrefabMap, 
				DefaultTileViewPrefab, Instantiate, GridPanel.GetComponent<RectTransform>());
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
			orderedControllers = new ITileController[GRID_WIDTH * GRID_HEIGHT];
			orderedCells = new HexGridCell<BaseNode>[GRID_WIDTH * GRID_HEIGHT];

			uint cellCounter = 0;
			foreach (var cell in grid.GetSpiralCells(CubeCoord.Origin, 5))
			{
				orderedCells[cellCounter++] = cell;
			}

			DisplayNode(tree.Tree.Root);
		}

		public void DisplayNode(BaseNode nodeToDisplay)
		{
			logger.Debug("DisplayTweakerNode: {0}", nodeToDisplay.Type);
			if(CurrentNode == nodeToDisplay)
			{
				return;
			}

			if (nodeToDisplay.Type == BaseNode.NodeType.Group || nodeToDisplay.Type == BaseNode.NodeType.Root)
			{
				//BaseNode oldNode = CurrentNode;
				CurrentNode = nodeToDisplay;

				int numChildren = nodeToDisplay.Children.Count;

				HexGridCell<BaseNode> rootCell = orderedCells[0];
				rootCell.Value = nodeToDisplay;
				ITileController rootController = orderedControllers[0];
				TileView rootView;
				TileView rootViewPrefab;
				tilePrefabMap.TryGetValue(nodeToDisplay.GetType(), out rootViewPrefab);
				// Check to see if we need to create a new view or if the previous view
				// is this cell can be reused.
				if (rootController == null ||
					rootViewPrefab.GetType() != rootController.ViewType)
				{
					// FIXME: destroy views and controllers at correct times.
					if (rootController != null)
					{
						Destroy(rootController.BaseView);
					}
					rootView = GetTileView(rootCell);
				}
				else if (rootController != null)
				{
					rootView = rootController.BaseView;
				}
				else
				{
					rootView = null;
					logger.Error("Failed to create a new tile view or recycle the previous view for this cell.");
					return;
				}

				if(rootController != null)
				{
					rootController.Dispose();
					orderedControllers[0] = null;
				}
				rootController = TileControllerFactory.MakeController(rootView, rootCell, this);
				orderedControllers[0] = rootController;

				ConfigureView(rootController);

				for (int i = 1; i < orderedCells.Length; ++i)
				{
					HexGridCell<BaseNode> childCell = orderedCells[i];
					ITileController childController = orderedControllers[i];

					if (i <= numChildren)
					{
						// Populated cells
						if (i >= orderedCells.Length)
						{
							logger.Error("Node has more children than there are available views.");
							return;
						}

						BaseNode newChildNode = nodeToDisplay.Children[i - 1];
						childCell.Value = newChildNode;
						
						TileView childView;
						TileView childViewPrefab;
						tilePrefabMap.TryGetValue(newChildNode.GetType(), out childViewPrefab);
						// Check to see if we need to create a new view or if the previous view
						// is this cell can be reused.
						if (childController == null ||
							childViewPrefab.GetType() != childController.ViewType)
						{
							if (childController != null)
							{
								Destroy(childController.BaseView);
							}
							childView = GetTileView(childCell);
						}
						else if (childController != null)
						{
							childView = childController.BaseView;
						}
						else
						{
							childView = null;
							logger.Error("Failed to create a new tile view or recycle the previous view for this cell.");
							return;
						}

						if (childController != null)
						{
							childController.Dispose();
							orderedControllers[i] = null;
						}
						childController = TileControllerFactory.MakeController(childView, childCell, this);
						orderedControllers[i] = childController;

						ConfigureView(childController);
					}
					else
					{
						if (childController != null)
						{
							Destroy(childController.BaseView.gameObject);
							childController.Dispose();
							orderedControllers[i] = null;
						}
					}
				}
			}
			//	else
			//		if invokable with args: show args as "tweakables"
		}

		private TileView GetTileView(HexGridCell<BaseNode> cell)
		{
			logger.Trace("SetupTileView: cell={0}", cell);
			
			if (cell == null || cell.Value == null)
			{
				logger.Error("A cell instance must be provided for cell: {0}", cell);
				return null;
			}			

			return tileViewFactory.MakeView<TileView>(cell);
		}

		private void ConfigureView(ITileController controller)
		{
			TileView view = controller.BaseView;
			BaseNode node = controller.BaseNode;

			logger.Trace("ConfigureView: node={0} view={1}", node, view);

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

			// For convenience, sometimes node will be a group node so check what type
			// it is first.
			// TODO: get rit of "RootNode" and make it a group named "Root"
			if(node is RootNode)
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
			if (node == CurrentNode)
			{
				// Treat the current node as a "root" node for display purposes.
				ConfigureRootView(view, node);
				view.Name = node.ShortName;
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
	}
}