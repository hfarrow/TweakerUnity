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
			if (Tapped != null)
			{
				Tapped(this);
			}
		}

		public void OnSelected()
		{
			if(didLongPress)
			{
				didLongPress = false;
				return;
			}

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
			yield return new WaitForSeconds(0.5f);
			didLongPress = true;
			if (LongPressed != null)
			{
				LongPressed(this);
			}
			longPressRoutine = null;
		}
	}
}
