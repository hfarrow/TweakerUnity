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

		public TweakableTileController(IHexGridController console, TweakableTileView view, HexGridCell<BaseNode> cell)
			: base(console, view, cell)
		{
			tweakable = Node.Tweakable;
		}

		protected override void ConfigureView()
		{
			base.ConfigureView();
			View.Name = TileDisplay.GetFriendlyName(tweakable.ShortName);
			View.TileColor = Color.cyan;

			object value = tweakable.GetValue();

			if (value != null)
			{
				View.Value = value.ToString();
			}
			else
			{
				View.Value = "null";
			}

			Node.Tweakable.ValueChanged += ValueChanged;
		}

		public override void Destroy(bool destroyView)
		{
			Node.Tweakable.ValueChanged -= ValueChanged;
			base.Destroy(destroyView);
		}

		public void ValueChanged(object oldValue, object newValue)
		{
			View.Value = newValue.ToString();
		}

		protected override void ViewTapped(TileView view)
		{
			grid.Console.ShowInspector(Node);
		}
	}
}
