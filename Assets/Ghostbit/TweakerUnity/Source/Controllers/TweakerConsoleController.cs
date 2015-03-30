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
		void DestroyObject(GameObject go);
	}

	public class TweakerConsoleController : MonoBehaviour, ITweakerConsoleController
	{
		public InspectorView InspectorViewPrefab;
		public HexGridController GridController;

		public Tweaker Tweaker { get; private set; }
		public TweakerTree Tree { get; private set; }

		private IInspectorController inspector;
		private ITweakerLogger logger = LogManager.GetCurrentClassLogger();

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
			// Check if the current inspector cannot be re-used.
			if(inspector != null && inspector.NodeType != nodeToInspect.Type)
			{
				inspector.Destroy();
				inspector = null;
			}

			if (inspector == null)
			{
				CreateInspector(nodeToInspect.Type);
			}

			inspector.InspectNode(nodeToInspect);
		}

		private void CreateInspector(BaseNode.NodeType type)
		{
			InspectorView view = Instantiate(InspectorViewPrefab) as InspectorView;
			view.GetComponent<RectTransform>().SetParent(GetComponent<RectTransform>(), false);
			inspector = InspectorControllerFactory.MakeController(view, GridController, type);
			inspector.Closed += InspectorClosed;
		}

		private void InspectorClosed()
		{
			inspector = null;
		}

		public void DestroyObject(GameObject go)
		{
			Destroy(go);
		}
	}
}