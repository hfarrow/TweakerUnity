using System;
using System.Collections.Generic;
using Ghostbit.Tweaker.Core;
using UnityEngine;

namespace Ghostbit.Tweaker.UI
{
	public interface ITileController : IDisposable
	{
		Type ViewType { get; }
		BaseNode.NodeType NodeType { get; }
		TileView BaseView { get; }
		BaseNode BaseNode { get; }
		HexGridCell<BaseNode> BaseCell { get; }
	}

	public class TileController<TView, TNode> : ITileController
		where TView : TileView
		where TNode : BaseNode
	{
		private static Vector3 selectedTileScale = new Vector3(1.6f, 1.6f, 1f);
		private static Vector3 deselectedTileScale = new Vector3(.95f, .95f, 1f);

		public Type ViewType { get { return typeof(TView); } }
		public BaseNode.NodeType NodeType { get { return Node.Type; } }
		public TileView BaseView { get { return View; } }
		public BaseNode BaseNode { get { return Node; } }
		public HexGridCell<BaseNode> BaseCell { get; private set; }

		public TView View { get; private set; }
		public TNode Node { get; private set; }

		private ITweakerLogger logger = LogManager.GetCurrentClassLogger();
		private ITweakerConsole console;

		public TileController(ITweakerConsole console, TView view, HexGridCell<BaseNode> cell)
		{
			this.console = console;
			View = view;
			BaseCell = cell;
			Node = BaseCell.Value as TNode;
			Init();
		}

		private void Init()
		{
			AddListeners();

			View.Scale = deselectedTileScale;
		}
	
		public void Dispose()
		{
			RemoveListeners();
		}

		private void AddListeners()
		{
			View.Tapped += ViewTapped;
			View.Selected += ViewSelected;
			View.Deselected += ViewDeselected;
		}

		private void RemoveListeners()
		{
			View.Tapped -= ViewTapped;
			View.Selected -= ViewSelected;
			View.Deselected -= ViewDeselected;
		}

		private void ViewTapped(TileView view)
		{
			logger.Trace("OnTileTapped: {0}", NodeType.ToString());
			switch (Node.Type)
			{
				case BaseNode.NodeType.Root:
					// ?
					break;
				case BaseNode.NodeType.Group:
					GroupNode group = Node.Value as GroupNode;
					logger.Trace("Group was tapped: {0}", group.FullName);
					if (group == console.CurrentNode)
					{
						console.DisplayNode(group.Parent);
					}
					else
					{
						console.DisplayNode(group);
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

		private void ViewSelected(TileView view)
		{
			View.Scale = selectedTileScale;
			View.GetComponent<RectTransform>().SetAsLastSibling();
		}

		private void ViewDeselected(TileView view)
		{
			View.Scale = deselectedTileScale;
		}
	}
}
