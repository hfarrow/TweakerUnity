using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ghostbit.Tweaker.Core;

namespace Ghostbit.Tweaker.UI
{
	public static class TileControllerFactory
	{
		public static ITileController MakeController(TileView view, HexGridCell<BaseNode> cell, ITweakerConsole console)
		{
			ITileController controller;
			switch (cell.Value.Type)
			{
				case BaseNode.NodeType.Group:
					controller = new GroupTileController(console, view, cell);
					break;

				case BaseNode.NodeType.Invokable:
					controller = new InvokableTileController(console, view, cell);
					break;

				case BaseNode.NodeType.Tweakable:
					controller = new TweakableTileController(console, view as TweakableTileView, cell);
					break;

				case BaseNode.NodeType.Watchable:
					controller = new TileController<TileView, WatchableNode>(console, view, cell);
					break;

				case BaseNode.NodeType.Unknown:
					controller = new TileController<TileView, BaseNode>(console, view, cell);
					break;

				default:
					LogManager.GetCurrentClassLogger().Error("Invalid or unsupported BaseNode.Type value: {0}", cell.Value.Type);
					controller = null;
					break;
			}

			controller.Init();
			return controller;
		}
	}
}
