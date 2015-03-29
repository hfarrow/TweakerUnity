using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Ghostbit.Tweaker.UI
{
	public class InspectorStringView : MonoBehaviour
	{
		public InputField InputText;
		public event Action<string> ValueChanged;

		public void Awake()
		{
			InputText.onEndEdit.AddListener(OnEndEdit);
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
