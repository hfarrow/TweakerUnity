using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ghostbit.Tweaker.UI
{
	public class TileView : MonoBehaviour
	{
		public HexGridCell<BaseNode> Cell;
		public DefaultTileView DefaultView;

		public event Action<TileView> Tapped;
		public event Action<TileView> Selected;
		public event Action<TileView> Deselected;

		public void Start()
		{
			DefaultView.Tapped += OnTapped;
			DefaultView.Selected += OnSelected;
			DefaultView.Deselected += OnDeselected;
		}

		public void OnTapped(DefaultTileView defaultView)
		{
			if (Tapped != null)
			{
				Tapped(this);
			}
		}

		public void OnSelected(DefaultTileView defaultView)
		{
			if (Selected != null)
			{
				Selected(this);
			}
		}

		public void OnDeselected(DefaultTileView defaultView)
		{
			if (Deselected != null)
			{
				Deselected(this);
			}
		}
	}
}
