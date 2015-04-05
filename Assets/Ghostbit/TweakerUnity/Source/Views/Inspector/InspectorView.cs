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
		public InspectorFooterView FooterPrefab;
		public InspectorStringView StringEditPrefab;
		public InspectorStringView StringSmallEditPrefab;
		public InspectorBoolView BoolEditPrefab;
		public InspectorDescriptionView DescriptionPrefab;

		public GameObject ContentContainer;
		public GameObject BodyContainer;

		public InspectorBackgroundView Background { get; private set; }
		public InspectorHeaderView Header { get; private set; }
		public InspectorFooterView Footer { get; private set; }

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
			Background = InstantiateInspectorComponent(BackgroundPrefab, gameObject);
			Header = InstantiateInspectorComponent(HeaderPrefab, ContentContainer);
			Footer = InstantiateInspectorComponent(FooterPrefab, ContentContainer);

			Header.GetComponent<RectTransform>().SetAsFirstSibling();
			Footer.GetComponent<RectTransform>().SetAsLastSibling();
			Background.GetComponent<RectTransform>().SetAsFirstSibling();
		}

		public TComponent InstantiateInspectorComponent<TComponent>(TComponent prefab, GameObject parent = null)
			where TComponent : Component
		{
			var component = Instantiate(prefab) as TComponent;
			if(parent == null)
			{
				parent = BodyContainer;
			}
			SetComponentParent(component, parent);
			return component;
		}

		private void SetComponentParent(Component child, GameObject parent)
		{
			child.GetComponent<RectTransform>().SetParent(parent.GetComponent<RectTransform>(), false);
			child.GetComponent<RectTransform>().SetAsLastSibling();
		}
	}
}
