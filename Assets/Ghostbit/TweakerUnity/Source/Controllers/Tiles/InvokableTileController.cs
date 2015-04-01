using Ghostbit.Tweaker.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Ghostbit.Tweaker.UI
{
	public class InvokableTileController : TileController<TileView, InvokableNode>
	{
		private IInvokable invokable;
		private ITweakerLogger logger = LogManager.GetCurrentClassLogger();

		public InvokableTileController(IHexGridController console, TileView view, HexGridCell<BaseNode> cell)
			: base(console, view, cell)
		{
			invokable = Node.Invokable;
		}

		protected override void ConfigureView()
		{
			base.ConfigureView();

			// Special case for the child invokable tile that actually invokes the parent invokable.
			if (invokable == null)
			{
				const string name = "Invoke";
				View.Name = name;
				View.FullName = name;
			}
			else
			{
				View.Name = invokable.ShortName;
				View.FullName = invokable.Name;
			}
			View.TileColor = Color.blue;
			View.NameText.color = Color.white;
		}

		protected override void ViewTapped(TileView view)
		{
			logger.Trace("Invokable was tapped: {0}", View.FullName);
			if (Node == grid.CurrentDisplayNode)
			{
				if (Node.Parent != null)
				{
					grid.DisplayNode(Node.Parent);
				}
			}
			else
			{
				if (invokable == null && grid.CurrentDisplayNode.Type == UI.BaseNode.NodeType.Invokable)
				{
					var parentInvokableNode = grid.CurrentDisplayNode as InvokableNode;
					try
					{
						parentInvokableNode.Invoke();
						view.ShowSuccess();
						if (parentInvokableNode.Parent != null)
						{
							grid.DisplayNode(parentInvokableNode.Parent);
						}
					}
					catch(Exception ex)
					{
						view.ShowError();
						logger.Error(ex.Message);
					}
				}
				else
				{
					grid.DisplayNode(Node);
				}
			}
		}
	}
}
