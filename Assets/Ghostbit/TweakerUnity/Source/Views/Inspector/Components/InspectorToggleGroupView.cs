using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ghostbit.Tweaker.UI
{
	public class InspectorToggleGroupView : MonoBehaviour, IInspectorContentView
	{
		public ToggleGroup ToggleGroup;

		public void Awake()
		{
			
		}

		public void DestroySelf()
		{
			Destroy(gameObject);
		}

		public void Destroy()
		{
			
		}
	}
}
