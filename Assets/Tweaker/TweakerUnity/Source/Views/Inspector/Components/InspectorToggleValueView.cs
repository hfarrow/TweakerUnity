using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tweaker.UI
{
	public class InspectorToggleValueView : MonoBehaviour, IInspectorContentView
	{
		public Toggle Toggle;
		public Text ToggleText;
		public event Action<bool> ValueChanged;

		public void Awake()
		{
			Toggle.onValueChanged.AddListener(OnValueChanged);
		}

		public void DestroySelf()
		{
			Destroy(gameObject);
		}

		public void Destroy()
		{
			Toggle.onValueChanged.RemoveAllListeners();
			ValueChanged = null;
		}

		private void OnValueChanged(bool value)
		{
			if (ValueChanged != null)
			{
				ValueChanged(value);
			}
		}
	}
}
