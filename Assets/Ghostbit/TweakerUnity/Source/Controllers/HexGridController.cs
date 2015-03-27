using System;
using System.Collections.Generic;
using Ghostbit.Tweaker.Core;
using UnityEngine;

namespace Ghostbit.Tweaker.UI
{
	public interface IHexGridController
	{
		ITweakerConsoleController Console { get; }
		BaseNode CurrentDisplayNode { get; }
		BaseNode CurrentInspectorNode { get; }

		void DisplayNode(BaseNode nodeToDisplay);
	}

	public class HexGridController : MonoBehaviour, IHexGridController
	{
		public TileView DefaultTileViewPrefab;
		public TweakableTileView TweakableTileViewPrefab;
		public GameObject GridPanel;
		public TweakerConsoleController ConsoleController;

		public ITweakerConsoleController Console { get { return ConsoleController; } }
		public BaseNode CurrentDisplayNode { get; private set; }
		public BaseNode CurrentInspectorNode { get; private set; }

		private HexGrid<BaseNode> grid;
		private ITileController[] orderedControllers;
		private HexGridCell<BaseNode>[] orderedCells;
		private Dictionary<Type, TileView> tilePrefabMap;
		private TileViewFactory tileViewFactory;
		private Tree<BaseNode> Tree;

		private const uint GRID_WIDTH = 7;
		private const uint GRID_HEIGHT = 5;

		private ITweakerLogger logger = LogManager.GetCurrentClassLogger();

		public void Awake()
		{
			tilePrefabMap = new Dictionary<Type, TileView>()
			{
				{ typeof(GroupNode), DefaultTileViewPrefab },
				{ typeof(TweakableNode), TweakableTileViewPrefab },
				{ typeof(InvokableNode), DefaultTileViewPrefab },
				{ typeof(WatchableNode), DefaultTileViewPrefab }
			};
			tileViewFactory = new TileViewFactory(tilePrefabMap,
				DefaultTileViewPrefab, Instantiate, GridPanel.GetComponent<RectTransform>());
		}

		public void Start()
		{
			Tree = ConsoleController.Tree.Tree;
			grid = new HexGrid<BaseNode>(GRID_WIDTH, GRID_HEIGHT);
			orderedControllers = new ITileController[GRID_WIDTH * GRID_HEIGHT];
			orderedCells = new HexGridCell<BaseNode>[GRID_WIDTH * GRID_HEIGHT];

			uint cellCounter = 0;
			foreach (var cell in grid.GetSpiralCells(CubeCoord.Origin, 5))
			{
				orderedCells[cellCounter++] = cell;
			}

			DisplayNode(Tree.Root);
		}

		public void DisplayNode(BaseNode nodeToDisplay)
		{
			logger.Debug("DisplayTweakerNode: {0}", nodeToDisplay.Type);
			if (CurrentDisplayNode == nodeToDisplay)
			{
				return;
			}

			if (nodeToDisplay.Type == BaseNode.NodeType.Group)
			{
				// BaseNode oldNode = CurrentNode;
				CurrentDisplayNode = nodeToDisplay;
				int numChildren = nodeToDisplay.Children.Count;
				List<BaseNode> displayList = new List<BaseNode>(numChildren + 1);

				displayList.Add(nodeToDisplay);
				displayList.AddRange(nodeToDisplay.Children);

				// First let's destroy controllers that will no longer be needed.
				// (Fewer views than available cells.
				for (uint orderedIndex = (uint)displayList.Count; orderedIndex < orderedControllers.Length; ++orderedIndex)
				{
					ITileController controller = orderedControllers[orderedIndex];
					if (controller != null)
					{
						DestroyController(orderedIndex, true);
					}
				}

				for (uint orderedIndex = 0; orderedIndex < displayList.Count; ++orderedIndex)
				{
					BaseNode node = displayList[(int)orderedIndex];
					SetupTileController(orderedIndex, node);
				}
			}
		}

		private void SetupTileController(uint orderedIndex, BaseNode node)
		{
			HexGridCell<BaseNode> cell = orderedCells[orderedIndex];
			cell.Value = node;
			ITileController controller = orderedControllers[orderedIndex];

			//logger.Trace("Setup next tile: node={0}, cell={1}, controller={2}", node, cell, controller);

			TileView viewPrefab;
			TileView view = null;
			if (!tilePrefabMap.TryGetValue(node.GetType(), out viewPrefab))
			{
				logger.Error("No tile view prefab mapping exists for type {0}.", node.GetType());
				DestroyController(orderedIndex, true);
				return;
			}

			// Is there an existing controller and view that we can re-use? 
			// (Does the old view type match the new view type?)
			if (controller != null)
			{
				if (controller.ViewType != viewPrefab.GetType())
				{
					DestroyController(orderedIndex, true);
				}
				else
				{
					view = controller.BaseView;
					view.Name = view.Name + "+";
				}
			}

			// Existing view could not be re-used or there was no existing view/controller
			if (view == null)
			{
				logger.Trace("Get new tile!");
				view = GetTileView(cell);
				if (view == null)
				{
					logger.Error("Failed to get a tile view for node of type {0}", node.Type);
					return;
				}
			}

			// Always recreate the controller.
			// Note: it is possible to recycle controllers as well but it isn't very expensive 
			// to always create a new controller instance.
			// Note: Destroy the old controller if it was not already destroyed above but do
			// not allow the view that may be getting re-used to be destroyed
			DestroyController(orderedIndex, false);
			controller = TileControllerFactory.MakeController(view, cell, this);
			orderedControllers[orderedIndex] = controller;
		}

		private void DestroyController(uint orderedIndex, bool destroyView)
		{
			ITileController controller = orderedControllers[orderedIndex];
			if (controller != null)
			{
				controller.Destroy(destroyView);
				orderedControllers[orderedIndex] = null;
			}
		}

		private TileView GetTileView(HexGridCell<BaseNode> cell)
		{
			logger.Trace("GetTileView: cell={0}", cell);

			if (cell == null || cell.Value == null)
			{
				logger.Error("A cell instance must be provided for cell: {0}", cell);
				return null;
			}

			return tileViewFactory.MakeView<TileView>(cell);
		}
	}
}
