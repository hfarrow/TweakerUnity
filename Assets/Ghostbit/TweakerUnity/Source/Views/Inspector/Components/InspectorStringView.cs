using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Tweaker.UI
{
	public class InspectorStringView : MonoBehaviour, IInspectorContentView
	{
		public InputField InputText;
		public event Action<string> ValueChanged;

		public void Awake()
		{
			InputText.onEndEdit.AddListener(OnEndEdit);
		}

		public void DestroySelf()
		{
			Destroy(gameObject);
		}

		public void Destroy()
		{
			InputText.onEndEdit.RemoveAllListeners();
			ValueChanged = null;
		}

		private void OnEndEdit(string value)
		{
			if (ValueChanged != null)
			{
				ValueChanged(value);
			}
		}
	}
}
