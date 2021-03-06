﻿using Tweaker.Core;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Tweaker.UI
{
	public class TileView : MonoBehaviour
	{
		// Prefabs (Set by editor)
		public TileBackgroundView BackgroundPrefab;
		public TileUIView UIViewPrefab;

		public event Action<TileView> Tapped;
		public event Action<TileView> Selected;
		public event Action<TileView> Deselected;
		public event Action<TileView> LongPressed;

		private Color successColor = Color.green;
		private Color errorColor = Color.red;

		public virtual Vector2 Scale
		{
			get { return scale; }
			set
			{
				scale = value;
				ui.GetComponent<RectTransform>().localScale = value;
				background.TileImage.GetComponent<RectTransform>().localScale = value;
			}
		}

		public Color TileColor
		{
			get { return background.TileImage.color; }
			set
			{
				background.TileImage.color = value;
			}
		}

		public float TileAlpha
		{
			get { return background.TileImage.color.a; }
			set 
			{
				Color newColor = TileColor;
				newColor.a = value;
				TileColor = newColor;
			}
		}

		public Text NameText
		{
			get { return ui.NameText; }
		}

		public string Name
		{
			get { return NameText.text; }
			set { NameText.text = value; }
		}

		protected TileBackgroundView background;
		protected TileUIView ui;
		protected Vector2 scale;

		protected ITweakerLogger logger = LogManager.GetCurrentClassLogger();

		public void Awake()
		{
			InstatiatePrefabs();

			// Hit area must be the top most child of the entire tile (not just the background view).
			ParentToThis(background.HitAreaImage);

			background.Tapped += OnTapped;
			background.Selected += OnSelected;
			background.Deselected += OnDeselected;
			background.LongPressed += OnLongPressed;

			OnAwake();
		}

		protected virtual void OnDestroy()
		{
			
		}

		protected virtual void OnAwake()
		{
			
		}

		public void DestroySelf()
		{
			Tapped = null;
			Selected = null;
			Deselected = null;

			background.Tapped -= OnTapped;
			background.Selected -= OnSelected;
			background.Deselected -= OnDeselected;
			background.LongPressed -= OnLongPressed;

			OnDestroy();
			Destroy(gameObject);
		}

		private void InstatiatePrefabs()
		{
			background = InstantiateTileComponent(BackgroundPrefab);
			ui = InstantiateTileComponent(UIViewPrefab);
		}

		private TComponent InstantiateTileComponent<TComponent>(TComponent prefab)
			where TComponent : Component
		{
			var instance = Instantiate(prefab) as TComponent;
			ParentToThis(instance);
			return instance;
		}

		private void ParentToThis(Component component)
		{
			component.GetComponent<RectTransform>().SetParent(GetComponent<RectTransform>(), false);
		}

		public virtual void OnTapped(TileBackgroundView defaultView)
		{
			if (Tapped != null)
			{
				Tapped(this);
			}
		}

		public virtual void OnSelected(TileBackgroundView defaultView)
		{
			if (Selected != null)
			{
				Selected(this);
			}
		}

		public virtual void OnDeselected(TileBackgroundView defaultView)
		{
			if (Deselected != null)
			{
				Deselected(this);
			}
		}

		public virtual void OnLongPressed(TileBackgroundView defaultView)
		{
			if(LongPressed != null)
			{
				LongPressed(this);
			}
		}

		public void ShowSuccess()
		{
			TileColor = successColor;
		}

		public void ShowError()
		{
			TileColor = errorColor;
		}
	}
}
