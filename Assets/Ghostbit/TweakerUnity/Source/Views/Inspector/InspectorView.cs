using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ghostbit.Tweaker.UI
{
	public class InspectorView : MonoBehaviour
	{
		public InspectorBackgroundView BackgroundPrefab;
		public InspectorHeaderView HeaderPrefab;
		public InspectorStringView StringPrefab;

		public GameObject HeaderContainer;
		public GameObject ContentContainer;
		public GameObject BackgroundContainer;

		public InspectorBackgroundView Background { get; private set; }
		public InspectorHeaderView Header { get; private set; }

		public void Awake()
		{
			InstatiatePrefabs();
			OnAwake();
		}

		protected virtual void OnAwake()
		{

		}

		public void DestroySelf()
		{
			Destroy(gameObject);
		}

		private void InstatiatePrefabs()
		{
			// Order matters: Back layer to front layer
			Background = InstantiateInspectorComponent(BackgroundPrefab, BackgroundContainer);
			Header = InstantiateInspectorComponent(HeaderPrefab, HeaderContainer);
		}

		private TComponent InstantiateInspectorComponent<TComponent>(TComponent prefab, GameObject parent = null)
			where TComponent : Component
		{
			var component = Instantiate(prefab) as TComponent;
			if(parent == null)
			{
				parent = gameObject;
			}
			SetComponentParent(component, parent);
			return component;
		}

		private void SetComponentParent(Component child, GameObject parent)
		{
			child.GetComponent<RectTransform>().SetParent(parent.GetComponent<RectTransform>(), false);
			child.GetComponent<RectTransform>().SetAsLastSibling();
		}

		public InspectorStringView AddStringView()
		{
			InspectorStringView view = InstantiateInspectorComponent(StringPrefab, ContentContainer);
			// TODO: position correctly if there are multiple views in the content view.
			return view;
		}
	}
}
