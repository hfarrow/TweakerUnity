using UnityEngine;
using System.Collections.Generic;
using Ghostbit.Tweaker.UI;
using Ghostbit.Tweaker.Core;
using System.Collections;
using System;
using UnityEngine.UI;

namespace Ghostbit.Tweaker.UI
{
	public interface ITweakerConsoleController
	{
		Tweaker Tweaker { get; }
		TweakerTree Tree { get; }
		void ShowInspector(BaseNode nodeToInspect);
	}

	public class TweakerConsoleController : MonoBehaviour, ITweakerConsoleController
	{
		private ITweakerLogger logger = LogManager.GetCurrentClassLogger();
		public Tweaker Tweaker { get; private set; }
		public TweakerTree Tree { get; private set; }

		// Must be called from awake of another script
		public void Init(Tweaker tweaker)
		{
			logger.Info("Init: " + tweaker);
			this.Tweaker = tweaker;

			Tree = new TweakerTree(this.Tweaker);
			Tree.BuildTree();
		}

		public void ShowInspector(BaseNode nodeToInspect)
		{
			throw new NotImplementedException();
		}
	}
}