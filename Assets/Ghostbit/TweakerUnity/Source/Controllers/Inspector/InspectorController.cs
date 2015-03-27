using System;
using System.Collections.Generic;
using Ghostbit.Tweaker.Core;
using UnityEngine;

namespace Ghostbit.Tweaker.UI
{
	public class InspectorController
	{
		private InspectorView view;
		private IHexGridController gridController;
		private ITweakerLogger logger = LogManager.GetCurrentClassLogger();

		public event Action Closed;

		public InspectorController(InspectorView view, IHexGridController gridController)
		{
			this.view = view;
			this.gridController = gridController;
			ConfigureViews();
		}

		public void Destroy()
		{
			view.Background.DoneButton.onClick.RemoveAllListeners();
			view.DestroySelf();
		}

		private void ConfigureViews()
		{
			view.Background.DoneButton.onClick.AddListener(DoneClicked);
		}

		private void DoneClicked()
		{
			Destroy();
			if(Closed != null)
			{
				Closed();
			}
		}
	}
}
