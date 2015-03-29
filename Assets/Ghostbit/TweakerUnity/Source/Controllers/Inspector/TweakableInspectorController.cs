using System;
using System.Collections.Generic;
using Ghostbit.Tweaker.Core;
using UnityEngine;

namespace Ghostbit.Tweaker.UI
{
	public class TweakableInspectorController : InspectorController<TweakableNode>
	{
		public TweakableNode TweakableNode { get; private set; }
		public ITweakable Tweakable { get; private set; }

		public override string Title { get { return TweakableNode.Tweakable.Name; } }

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

			TweakableNode = CurrentNode as TweakableNode;
			Tweakable = TweakableNode.Tweakable;
			base.OnInspectNode();

			if(Tweakable.TweakableType == typeof(string))
			{
				InspectorStringView stringView = view.AddStringView();
				stringView.InputText.text = Tweakable.GetValue().ToString();
				stringView.ValueChanged += ValueChanged;
			}
		}

		private void ValueChanged(object value)
		{
			TweakableNode.SetValue(value);
		}

		public override void Destroy()
		{
			base.Destroy();
		}
	}
}
