using Ghostbit.Tweaker.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Ghostbit.Tweaker.UI
{
	public class TweakableTileController : TileController<TweakableTileView, TweakableNode>
	{
		private ITweakable tweakable;

		public TweakableTileController(ITweakerConsole console, TweakableTileView view, HexGridCell<BaseNode> cell)
			: base(console, view, cell)
		{
			tweakable = Node.Tweakable;
		}

		protected override void ConfigureView()
		{
			base.ConfigureView();
			View.Name = tweakable.ShortName;
			View.TileColor = Color.cyan;
			View.FullName = tweakable.Name;
			View.Value = tweakable.GetValue().ToString();
		}

		protected override void ViewTapped(TileView view)
		{
			
		}
	}
}
