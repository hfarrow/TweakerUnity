using System;
using System.Collections.Generic;
using Tweaker.Core;
using UnityEngine;

namespace Tweaker.UI
{
	public interface IHexGridController
	{
		ITweakerConsoleController Console { get; }
		BaseNode CurrentDisplayNode { get; }

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

		private HexGrid<BaseNode> grid;
		private ITileController[] orderedControllers;
		private HexGridCell<BaseNode>[] orderedCells;
		private Dictionary<Type, TileView> tilePrefabMap;
		private TileViewFactory tileViewFactory;
		private Tree<BaseNode> Tree;

		private uint gridWidth;
		private uint gridHeight;

		private ITweakerLogger logger = LogManager.GetCurrentClassLogger();

		private uint targetGridSize = 5;

		[Tweakable("Tweaker.UI.TargetGridSize", 
			Description="The max number of vertical tiles if in landscape or the max number of horizontal tiles if in portrait.")]
		[TweakerRange(3u, 10u)]
		[TweakableUIFlags(TweakableUIFlags.HideRangeSlider)]
		public uint TargetGridSize
		{
			get { return targetGridSize; }
			set
			{
				if(targetGridSize != value)
				{
					targetGridSize = value;
					Resize();
				}
			}
		}

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

		public void Refresh()
		{
			Tree = ConsoleController.Tree.Tree;
			Resize();
		}

		public void Resize()
		{
			BaseNode nodeToDisplay = Tree.Root;

			// Cleanup the old grid if it has already been created.
			if(grid != null)
			{
				nodeToDisplay = CurrentDisplayNode;
				CurrentDisplayNode = null;
				for(uint i = 0; i < orderedControllers.Length; ++i)
				{
					DestroyController(i, true);
				}
			}

			CalculateGridSize();
			grid = new HexGrid<BaseNode>(gridWidth, gridHeight);
			orderedControllers = new ITileController[gridWidth * gridHeight];
			orderedCells = new HexGridCell<BaseNode>[gridWidth * gridHeight];

			uint cellCounter = 0;
			foreach (var cell in grid.GetSpiralCells(CubeCoord.Origin, Math.Max(gridWidth, gridHeight)))
			{
				orderedCells[cellCounter++] = cell;
			}

			DisplayNode(nodeToDisplay);
		}

		private void CalculateGridSize()
		{
			if (TweakerConsoleController.IsLandscape())
			{
				gridHeight = targetGridSize;
				float targetTileHeight = (float)Screen.height / ((float)gridHeight + 1);
				float targetTileWidth = targetTileHeight / (Mathf.Sqrt(3f) / 2f);
				float horizontalDistance = (targetTileWidth * 0.75f);
				gridWidth = (uint)((((float)Screen.width - targetTileWidth / 4f)) / horizontalDistance);
			}
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
			// (Fewer views than available cells.)
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

		private void OnGUI()
		{
			if(Console.CurrentInspectorNode != null)
			{
				return;
			}

			int yOffset = 10;
			if(CurrentDisplayNode.Type == BaseNode.NodeType.Group)
			{
				var node = CurrentDisplayNode as GroupNode;

				AddLabel(node.FullName, 20, ref yOffset);
			}
			else if (CurrentDisplayNode.Type == BaseNode.NodeType.Invokable)
			{
				var node = CurrentDisplayNode as InvokableNode;
				AddLabel(node.Invokable.Name, 20, ref yOffset);
				AddLabel(node.Invokable.Description, Screen.height - 20, ref yOffset);
			}
		}

		private void AddLabel(string label, int height, ref int yOffset)
		{
			GUI.Label(new Rect(10, yOffset, Screen.width - 20, height), label);
			yOffset += height;
		}
	}
}
