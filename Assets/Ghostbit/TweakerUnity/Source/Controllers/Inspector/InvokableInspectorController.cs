using System;
using System.Collections.Generic;
using Ghostbit.Tweaker.Core;
using UnityEngine;

namespace Ghostbit.Tweaker.UI
{
	public class InvokableInspectorController : InspectorController<InvokableNode>
	{
		public IInvokable Invokable { get; private set; }
		public override string Title { get { return Invokable.Name; } }

		public InvokableInspectorController(InspectorView view, IHexGridController gridController)
			: base(view, gridController)
		{
			
		}

		protected override void OnInspectNode()
		{
			if(CurrentNode.Type != BaseNode.NodeType.Invokable)
			{
				logger.Error("Invalid node type assigned to invokable controller: {0}", CurrentNode.Type);
				return;
			}

			Invokable = CurrentNode.Invokable;
			base.OnInspectNode();

			view.Header.TypeText.text = Invokable.MethodSignature;

			foreach (IInspectorContentView contentView in MakeContentViews())
			{
				if (contentView != null)
				{
					contentViews.Add(contentView);
				}
				else
				{
					view.Header.TypeText.text = "[Unsupported] " + Invokable.MethodSignature;
				}
			}
		}

		private IEnumerable<IInspectorContentView> MakeContentViews()
		{
			yield return contentFactory.MakeDescriptionView(Invokable.Description);
		}

		public override void Destroy()
		{
			base.Destroy();
		}
	}
}
