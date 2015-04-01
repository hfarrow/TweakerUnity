using Ghostbit.Tweaker.Core;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ghostbit.Tweaker.UI
{
	public class TileView : MonoBehaviour
	{
		// Prefabs (Set by editor)
		public TileBackgroundView BackgroundPrefab;
		public TileUIView UIViewPrefab;
		public FullNameView FullNamePrefab;

		public event Action<TileView> Tapped;
		public event Action<TileView> Selected;
		public event Action<TileView> Deselected;

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
				fullName.GetComponent<RectTransform>().localScale = value;
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

		public Text FullNameText
		{
			get { return fullName.FullNameText; }
		}

		public string FullName
		{
			get { return FullNameText.text; }
			set { FullNameText.text = value; }
		}

		protected TileBackgroundView background;
		protected TileUIView ui;
		protected FullNameView fullName;
		protected Vector2 scale;

		protected ITweakerLogger logger = LogManager.GetCurrentClassLogger();

		public void Awake()
		{
			InstatiatePrefabs();
			ShowFullName(false);

			// Hit area must be the top most child of the entire tile (not just the background view).
			ParentToThis(background.HitAreaImage);

			background.Tapped += OnTapped;
			background.Selected += OnSelected;
			background.Deselected += OnDeselected;

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

			OnDestroy();
			Destroy(gameObject);
		}

		private void InstatiatePrefabs()
		{
			background = InstantiateTileComponent(BackgroundPrefab);
			ui = InstantiateTileComponent(UIViewPrefab);
			fullName = InstantiateTileComponent(FullNamePrefab);
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
			ShowFullName(true);
			if (Selected != null)
			{
				Selected(this);
			}
		}

		public virtual void OnDeselected(TileBackgroundView defaultView)
		{
			ShowFullName(false);
			if (Deselected != null)
			{
				Deselected(this);
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

		private void ShowFullName(bool show)
		{
			fullName.gameObject.SetActive(show);
			ui.NameText.gameObject.SetActive(!show);
		}
	}
}
