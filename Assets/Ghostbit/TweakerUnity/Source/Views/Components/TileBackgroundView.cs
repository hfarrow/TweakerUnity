using UnityEngine;
using System.Collections;
using Ghostbit.Tweaker.UI;
using Ghostbit.Tweaker.Core;
using UnityEngine.UI;
using System;

namespace Ghostbit.Tweaker.UI
{
	public class TileBackgroundView : MonoBehaviour
	{
		public Image TileImage;
		public Image HitAreaImage;

		public event Action<TileBackgroundView> Tapped;
		public event Action<TileBackgroundView> Selected;
		public event Action<TileBackgroundView> Deselected;

		public Color TileColor
		{
			get { return TileImage.color; }
			set { TileImage.color = value; }
		}

		public float TileAlpha
		{
			get { return TileImage.color.a; }
			set
			{
				Color color = TileImage.color;
				color.a = value;
				TileImage.color = color;
			}
		}

		public void OnTapped()
		{
			if (Tapped != null)
			{
				Tapped(this);
			}
		}

		public void OnSelected()
		{
			if (Selected != null)
			{
				Selected(this);
			}
		}

		public void OnDeselected()
		{
			if (Deselected != null)
			{
				Deselected(this);
			}
		}
	}
}
