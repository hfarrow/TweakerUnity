using System;
using System.Collections.Generic;
using Ghostbit.Tweaker.Core;
using UnityEngine;

namespace Ghostbit.Tweaker.UI
{
	public class TweakableInspectorController : InspectorController<TweakableNode>
	{
		public ITweakable Tweakable { get; private set; }
		public override string Title { get { return Tweakable.Name; } }

		public TweakableInspectorController(InspectorView view, IHexGridController gridController)
			: base(view, gridController)
		{
			
		}

		protected override void OnInspectNode()
		{
			if(CurrentNode.Type != BaseNode.NodeType.Tweakable)
			{
				logger.Error("Invalid node type assigned to tweakable controller: {0}", CurrentNode.Type);
				return;
			}

			Tweakable = CurrentNode.Tweakable;
			base.OnInspectNode();

			view.Header.TypeText.text = Tweakable.TweakableType.FullName;

			foreach(IInspectorContentView contentView in contentFactory.MakeContentViews(Tweakable))
			{
				if (contentView != null)
				{
					contentViews.Add(contentView);
				}
				else
				{
					view.Header.TypeText.text = "[Unsupported] " + Tweakable.TweakableType.FullName;
				}
			}
		}

		public override void Destroy()
		{
			base.Destroy();
		}
	}
}
