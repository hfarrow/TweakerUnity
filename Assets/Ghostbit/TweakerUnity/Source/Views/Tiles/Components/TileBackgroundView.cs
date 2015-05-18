using UnityEngine;
using System.Collections;
using Tweaker.UI;
using Tweaker.Core;
using UnityEngine.UI;
using System;

namespace Tweaker.UI
{
	public class TileBackgroundView : MonoBehaviour
	{
		[Tweakable("Tweaker.UI.LongPressDelay",
			Description="How long a tile must be pressed before triggering a long press event.")]
		[TweakerRange(0.2f, 3f)]
		public static float LongPressDelay = 0.5f;

		public Image TileImage;
		public Image HitAreaImage;

		public event Action<TileBackgroundView> Tapped;
		public event Action<TileBackgroundView> Selected;
		public event Action<TileBackgroundView> Deselected;
		public event Action<TileBackgroundView> LongPressed;

		private Coroutine longPressRoutine;
		private bool didLongPress;

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

		public void OnDestroy()
		{
			Tapped = null;
			Selected = null;
			Deselected = null;
		}

		public void OnTapped()
		{
			if (!didLongPress && Tapped != null)
			{
				Tapped(this);
			}
			didLongPress = false;
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

		public void OnPress()
		{
			longPressRoutine = StartCoroutine(WaitForLongPress());
		}

		public void OnRelease()
		{
			if (longPressRoutine != null)
			{
				StopCoroutine(longPressRoutine);
			}
		}

		private IEnumerator WaitForLongPress()
		{
			yield return new WaitForSeconds(LongPressDelay);
			didLongPress = true;
			if (LongPressed != null)
			{
				LongPressed(this);
			}
			longPressRoutine = null;
		}
	}
}
