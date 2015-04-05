using Ghostbit.Tweaker.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Ghostbit.Tweaker.UI
{
	public class WatchableTileController : TileController<TileView, WatchableNode>
	{
		private IWatchable watchable;

		public WatchableTileController(IHexGridController console, TileView view, HexGridCell<BaseNode> cell)
			: base(console, view, cell)
		{
			watchable = Node.Watchable;
		}

		protected override void ConfigureView()
		{
			base.ConfigureView();
			View.Name = TileDisplay.GetFriendlyName(watchable.ShortName);
			View.TileColor = Color.magenta;
		}

		protected override void ViewTapped(TileView view)
		{

		}
	}
}
