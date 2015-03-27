﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Ghostbit.Tweaker.UI
{
	public class GroupTileController : TileController<TileView, GroupNode>
	{
		public GroupTileController(ITweakerConsole console, TileView view, HexGridCell<BaseNode> cell)
			: base(console, view, cell)
		{
		}

		protected override void ConfigureView()
		{
			base.ConfigureView();
			View.Name = Node.ShortName;
			View.TileColor = Color.white;
			View.FullName = Node.FullName;

			if (Node == console.CurrentNode)
			{
				View.TileColor = new Color(.2f, .2f, .2f);
				View.NameText.color = Color.white;
			}
		}

		protected override void ViewTapped(TileView view)
		{
			logger.Trace("Group was tapped: {0}", Node.FullName);
			if (Node == console.CurrentNode)
			{
				if (Node.Parent != null)
				{
					console.DisplayNode(Node.Parent);
				}
			}
			else
			{
				console.DisplayNode(Node);
			}
		}
	}
}