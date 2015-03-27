using System;
using System.Collections.Generic;
using Ghostbit.Tweaker.Core;
using UnityEngine;

namespace Ghostbit.Tweaker.UI
{
	public class InspectorController
	{
		private InspectorView view;
		private IHexGridController console;
		private ITweakerLogger logger = LogManager.GetCurrentClassLogger();

		public event Action Closed;

		public InspectorController(InspectorView view, IHexGridController console)
		{
			this.view = view;
			this.console = console;
			ConfigureViews();
		}

		public void Destroy()
		{
			view.Background.DoneButton.onClick.RemoveAllListeners();
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
