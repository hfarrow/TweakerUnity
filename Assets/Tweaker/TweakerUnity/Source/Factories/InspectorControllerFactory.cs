using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tweaker.Core;

namespace Tweaker.UI
{
	public static class InspectorControllerFactory
	{
		public static IInspectorController MakeController(InspectorView view, IHexGridController grid, BaseNode.NodeType type)
		{
			IInspectorController controller = null;
			switch (type)
			{
				case BaseNode.NodeType.Group:
					controller = new GroupInspectorController(view, grid);
					break;
				case BaseNode.NodeType.Invokable:
					controller = new InvokableInspectorController(view, grid);
					break;

				case BaseNode.NodeType.Tweakable:
					controller = new TweakableInspectorController(view, grid);
					break;

				case BaseNode.NodeType.Watchable:
					//controller = new WatchableTileController(console, view, cell);
					break;

				default:
					LogManager.GetCurrentClassLogger().Error("Cannot inspect node of type {0}", type);
					controller = null;
					break;
			}

			return controller;
		}
	}
}
