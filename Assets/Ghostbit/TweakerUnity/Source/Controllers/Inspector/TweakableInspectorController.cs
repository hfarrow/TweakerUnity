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

			if (tweakable.TweakableType == typeof(string))
			{
				yield return contentFactory.MakeEditStringView(tweakable);
			}
			else if (tweakable.TweakableType == typeof(bool))
			{
				yield return contentFactory.MakeEditBoolView(tweakable);
			}
			else if (tweakable.TweakableType.IsEnum)
			{
				// TODO: allow enum value to be entered directly via input field.
				// Enum will always display as Toggle.
			}
			else if (tweakable.TweakableType.IsNumericType())
			{
				yield return contentFactory.MakeEditNumericView(tweakable);
				if (tweakable.HasRange && !UIFlagsUtil.IsSet(TweakableFlags.HideRangeSlider, tweakable))
				{
					yield return contentFactory.MakeSliderView(tweakable);
				}
			}
			else
			{
				yield return null;
				// TODO: use input text and attempt json deserialization to tweakable.TweakableType
				// OR
				// TODO: use reflection to parse object into additional hex grid pages.
			}

			if(tweakable.HasStep)
			{
				yield return contentFactory.MakeStepperView(tweakable);
			}

			if(tweakable.HasToggle)
			{
				InspectorToggleGroupView groupView = contentFactory.MakeToggleGroupView();
				yield return groupView;
				IToggleTweakable toggle = tweakable.Toggle;
				for (int i = 0; i < toggle.ToggleCount; ++i)
				{
					yield return contentFactory.MakeToggleValueView(tweakable, toggle, i, groupView.ToggleGroup);
				}
			}
		}

		public override void Destroy()
		{
			base.Destroy();
		}
	}
}
