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

		public InvokableTileController(IHexGridController console, TileView view, HexGridCell<BaseNode> cell)
			: base(console, view, cell)
		{
			invokable = Node.Invokable;
		}

		protected override void ConfigureView()
		{
			base.ConfigureView();
			View.Name = invokable.ShortName;
			View.TileColor = Color.blue;
			View.NameText.color = Color.white;
			View.FullName = invokable.Name;
		}

		protected override void ViewTapped(TileView view)
		{

		}
	}
}
