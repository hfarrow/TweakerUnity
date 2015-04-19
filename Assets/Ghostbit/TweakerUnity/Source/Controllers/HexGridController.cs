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

		private uint gridWidth;
		private uint gridHeight;

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
			CalculateGridSize();
			grid = new HexGrid<BaseNode>(gridWidth, gridHeight);
			orderedControllers = new ITileController[gridWidth * gridHeight];
			orderedCells = new HexGridCell<BaseNode>[gridWidth * gridHeight];

			uint cellCounter = 0;
			foreach (var cell in grid.GetSpiralCells(CubeCoord.Origin, Math.Max(gridWidth, gridHeight)))
			{
				orderedCells[cellCounter++] = cell;
			}

			DisplayNode(Tree.Root);
		}

		//private void CalculateGridSize()
		//{
		//	const uint targetGridSize = 5;
		//	float targetTileSize;
		//	if(Screen.height < Screen.width)
		//	{
		//		gridHeight = targetGridSize;
		//		targetTileSize = (float)Screen.height / (float)gridHeight;
		//		gridWidth = (uint)((float)Screen.width / targetTileSize);

		//	}
		//	else
		//	{
		//		gridWidth = targetGridSize;
		//		targetTileSize = (float)Screen.width / (float)gridWidth;
		//		gridHeight = (uint)((float)Screen.height / targetTileSize);
		//	}

		//	logger.Info("GridSize = {0} x {1}", gridWidth, gridHeight);
		//}

		private void CalculateGridSize()
		{
			const uint targetGridSize = 5;
			logger.Info("screen = {0} x {1}", Screen.width, Screen.height);

			// landscape
			if (Screen.height < Screen.width)
			{
				gridHeight = targetGridSize;
				float targetTileHeight = (float)Screen.height / ((float)gridHeight + 1);
				float targetTileWidth = targetTileHeight / (Mathf.Sqrt(3f) / 2f);
				float horizontalDistance = targetTileWidth * 0.75f;
				gridWidth = (uint)((float)Screen.width / horizontalDistance);

				logger.Info("gridHeight={0} targetTileHeight={1} targetTileWidth={2} gridWidth={3}", gridHeight, targetTileHeight, targetTileWidth, gridWidth);

			}
			// portrait
			else
			{
				gridWidth = targetGridSize;
				float targetTileWidth = (float)Screen.width / (float)gridWidth;
				targetTileWidth *= 1.25f;
				float targetTileHeight = targetTileWidth * (Mathf.Sqrt(3f) / 2f);
				gridHeight = (uint)((float)Screen.height / targetTileHeight);
			}

			logger.Info("GridSize = {0} x {1}", gridWidth, gridHeight);
		}

		public void DisplayNode(BaseNode nodeToDisplay)
		{
			logger.Debug("DisplayTweakerNode: {0}", nodeToDisplay.Type);
			if (CurrentDisplayNode == nodeToDisplay)
			{
				return;
			}

			// Invokables are treated as a group node. The child TweakableNode's bind to the invokable's args.
			if (nodeToDisplay.Type == BaseNode.NodeType.Group ||
				nodeToDisplay.Type == BaseNode.NodeType.Invokable)
			{
				DisplayGroupNode(nodeToDisplay);
			}
		}

		private void DisplayGroupNode(BaseNode nodeToDisplay)
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

				if (orderedIndex >= orderedCells.Length)
				{
					logger.Warn("The hex grid is not large enough to fit all nodes.");
				}
				else
				{
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
				}
			}

			// Existing view could not be re-used or there was no existing view/controller
			if (view == null)
			{
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
			if (cell == null || cell.Value == null)
			{
				logger.Error("A cell instance must be provided for cell: {0}", cell);
				return null;
			}

			return tileViewFactory.MakeView<TileView>(cell, gridWidth, gridHeight);
		}
	}
}
