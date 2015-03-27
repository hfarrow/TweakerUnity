using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ghostbit.Tweaker.UI
{
	public class InspectorView : MonoBehaviour
	{
		public InspectorBackgroundView BackgroundPrefab;

		public InspectorBackgroundView Background { get; private set; }

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
			Background = InstantiateInspectorComponent(BackgroundPrefab);
		}

		private TComponent InstantiateInspectorComponent<TComponent>(TComponent prefab)
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
	}
}
