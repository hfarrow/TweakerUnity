using System;
using System.Collections.Generic;
using Ghostbit.Tweaker.Core;
using UnityEngine;

namespace Ghostbit.Tweaker.UI
{
	public interface ITileController : IViewController
	{
		Type ViewType { get; }
		BaseNode.NodeType NodeType { get; }
		TileView BaseView { get; }
		BaseNode BaseNode { get; }
		HexGridCell<BaseNode> BaseCell { get; }

		void Init();
		void Destroy(bool destroyView);
	}

	public class TileController<TView, TNode> : ITileController
		where TView : TileView
		where TNode : BaseNode
	{
		protected static Vector3 selectedTileScale = new Vector3(1.6f, 1.6f, 1f);
		protected static Vector3 deselectedTileScale = new Vector3(.95f, .95f, 1f);

		public Type ViewType { get { return typeof(TView); } }
		public BaseNode.NodeType NodeType { get { return Node.Type; } }
		public TileView BaseView { get { return View; } }
		public BaseNode BaseNode { get { return Node; } }
		public HexGridCell<BaseNode> BaseCell { get; private set; }

		public TView View { get; private set; }
		public TNode Node { get; private set; }

		protected ITweakerLogger logger = LogManager.GetCurrentClassLogger();
		protected IHexGridController grid;

		public TileController(IHexGridController grid, TView view, HexGridCell<BaseNode> cell)
		{
			this.grid = grid;
			View = view;
			BaseCell = cell;
			Node = BaseCell.Value as TNode;
		}

		public void Init()
		{
			AddListeners();
			ConfigureView();
		}

		protected virtual void ConfigureView()
		{
			View.Scale = deselectedTileScale;

			if(NodeType == UI.BaseNode.NodeType.Unknown)
			{
				View.TileAlpha = 0.6f;
				View.Name = "<Unknown Type>";
				View.FullName = "<Unknown Type>";
			}
			else
			{
				// Reasonable Defaults
				View.TileColor = Color.white;
				View.TileAlpha = 1f;
				View.NameText.color = Color.black;
			}
		}

		public void Destroy()
		{
			Destroy(true);
		}
	
		public virtual void Destroy(bool destroyView)
		{
			RemoveListeners();
			if (View != null)
			{
				if (destroyView)
				{
					View.DestroySelf();
				}
			}
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

		protected virtual void ViewTapped(TileView view)
		{
			logger.Trace("OnTileTapped: {0}", NodeType.ToString());
		}

		protected virtual void ViewSelected(TileView view)
		{
			View.Scale = selectedTileScale;
			View.GetComponent<RectTransform>().SetAsLastSibling();
		}

		protected virtual void ViewDeselected(TileView view)
		{
			View.Scale = deselectedTileScale;
		}
	}
}
