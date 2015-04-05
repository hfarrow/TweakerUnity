﻿using Ghostbit.Tweaker.Core;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ghostbit.Tweaker.UI
{
	public class TweakableTileView : TileView
	{
		public TweakableUIView TweakableView { get { return ui as TweakableUIView; } }

		protected override void OnAwake()
		{
		}

		protected override void OnDestroy()
		{
			
		}

		public Color ValueColor
		{
			get { return TweakableView.ValueText.color; }
			set
			{
				TweakableView.ValueText.color = value;
			}
		}

		public string Value
		{
			get { return TweakableView.ValueText.text; }
			set
			{
				TweakableView.ValueText.text = value;
			}
		}

		public override void OnTapped(TileBackgroundView defaultView)
		{
			base.OnTapped(defaultView);
		}

		public override void OnSelected(TileBackgroundView defaultView)
		{
			base.OnSelected(defaultView);
		}

		public override void OnDeselected(TileBackgroundView defaultView)
		{
			base.OnDeselected(defaultView);
		}
	}
}
