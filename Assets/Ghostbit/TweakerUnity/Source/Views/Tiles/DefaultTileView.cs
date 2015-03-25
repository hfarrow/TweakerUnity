using UnityEngine;
using System.Collections;
using Ghostbit.Tweaker.UI;
using Ghostbit.Tweaker.Core;
using UnityEngine.UI;
using System;

namespace Ghostbit.Tweaker.UI
{
	public class DefaultTileView : MonoBehaviour
	{
		public Image TileImage;
		public Text NameText;
		public GameObject TileUI;
		public FullNameView FullNameView;

		public event Action<DefaultTileView> Tapped;
		public event Action<DefaultTileView> Selected;
		public event Action<DefaultTileView> Deselected;

		// Debug Elements
		public Text XText;
		public Text YText;

		public void Start()
		{
			ShowFullNameView(false);
		}

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

		public string Name
		{
			get { return NameText.text; }
			set { NameText.text = value; }
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
				ShowFullNameView(true);
				Selected(this);
			}
		}

		public void OnDeselected()
		{
			if (Deselected != null)
			{
				ShowFullNameView(false);
				Deselected(this);
			}
		}

		private void ShowFullNameView(bool show)
		{
			FullNameView.gameObject.SetActive(show);
			NameText.gameObject.SetActive(!show);
		}
	}
}
