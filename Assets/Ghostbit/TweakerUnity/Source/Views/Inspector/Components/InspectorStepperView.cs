using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ghostbit.Tweaker.UI
{
	public class InspectorStepperView : MonoBehaviour, IInspectorContentView
	{
		public Button NextButton;
		public Button PrevButton;

		public event Action NextClicked;
		public event Action PrevClicked;

		public void Awake()
		{
			NextButton.onClick.AddListener(OnNextClicked);
			PrevButton.onClick.AddListener(OnPrevClicked);
		}

		public void DestroySelf()
		{
			Destroy(gameObject);
		}

		public void Destroy()
		{
			NextButton.onClick.RemoveAllListeners();
			PrevButton.onClick.RemoveAllListeners();
			NextClicked = null;
			PrevClicked = null;
		}

		private void OnNextClicked()
		{
			if(NextClicked != null)
			{
				NextClicked();
			}
		}

		private void OnPrevClicked()
		{
			if(PrevClicked != null)
			{
				PrevClicked();
			}
		}
	}
}
