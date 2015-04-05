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

			foreach(IInspectorContentView contentView in MakeContentViews(Tweakable))
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

		private IEnumerable<IInspectorContentView> MakeContentViews(ITweakable tweakable)
		{
			yield return contentFactory.MakeDescriptionView(tweakable.Description);

			if (tweakable.HasToggle)
			{
				// TODO: show toggles
			}
			else if (tweakable.TweakableType.IsEnum)
			{
				// TODO: show enum values (same view as toggles);
			}
			else if (tweakable.TweakableType == typeof(string))
			{
				yield return contentFactory.MakeEditStringView(tweakable);
			}
			else if (tweakable.TweakableType == typeof(bool))
			{
				yield return contentFactory.MakeEditBoolView(tweakable);
			}
			else if (tweakable.TweakableType.IsNumericType())
			{
				yield return contentFactory.MakeEditNumericView(tweakable);
				// TODO: Show constraints as slider
				// TODO: show stepper if tweakable.IsStep
			}
			else
			{
				yield return null;
				// TODO: use input text and attempt json deserialization to tweakable.TweakableType
			}
		}

		public override void Destroy()
		{
			base.Destroy();
		}
	}
}
