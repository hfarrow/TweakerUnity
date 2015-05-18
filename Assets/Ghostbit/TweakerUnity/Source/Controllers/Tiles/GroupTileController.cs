using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Tweaker.UI
{
	public class GroupTileController : TileController<TileView, GroupNode>
	{
		public static Color GroupRootTileColor = new Color(.2f, .2f, .2f);
		public static Color GroupRootNameTextColor = Color.white;

		public GroupTileController(IHexGridController console, TileView view, HexGridCell<BaseNode> cell)
			: base(console, view, cell)
		{
		}

		protected override void ConfigureView()
		{
			base.ConfigureView();
			View.Name = TileDisplay.GetFriendlyName(Node.ShortName);
			View.TileColor = Color.white;

			if (Node == grid.CurrentDisplayNode)
			{
				View.TileColor = GroupRootTileColor;
				View.NameText.color = GroupRootNameTextColor;
			}
		}

		protected override void ViewTapped(TileView view)
		{
			logger.Trace("Group was tapped: {0}", Node.FullName);
			if (Node == grid.CurrentDisplayNode)
			{
				if (Node.Parent != null)
				{
					grid.DisplayNode(Node.Parent);
				}
			}
			else
			{
				grid.DisplayNode(Node);
			}
		}
	}
}
