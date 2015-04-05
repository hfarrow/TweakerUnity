using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Ghostbit.Tweaker.UI
{
	public class InspectorDescriptionView : MonoBehaviour, IInspectorContentView
	{
		public Text DescriptionText;

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
