using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ghostbit.Tweaker.UI
{
	public class InspectorSliderView : MonoBehaviour, IInspectorContentView
	{
		public Slider Slider;
		public event Action<float> ValueChanged;

		public void Awake()
		{
			Slider.onValueChanged.AddListener(OnValueChanged);
		}

		public void DestroySelf()
		{
			Destroy(gameObject);
		}

		public void Destroy()
		{
			Slider.onValueChanged.AddListener(OnValueChanged);
		}

		private void OnValueChanged(float value)
		{
			if (ValueChanged != null)
			{
				ValueChanged(value);
			}
		}
	}
}
