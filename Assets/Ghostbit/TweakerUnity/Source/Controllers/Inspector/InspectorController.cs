using System;
using System.Collections.Generic;
using Ghostbit.Tweaker.Core;
using UnityEngine;

namespace Ghostbit.Tweaker.UI
{
	public interface IInspectorController
	{
		event Action Closed;
		BaseNode.NodeType NodeType { get; }
		BaseNode CurrentBaseNode { get; }
		string Title { get; }
		void InspectNode(BaseNode node);
		void Destroy();
		
	}

	public abstract class InspectorController<TNode> : IInspectorController
		where TNode : BaseNode
	{
		public event Action Closed;
		public BaseNode.NodeType NodeType { get { return CurrentBaseNode.Type; } }
		public BaseNode CurrentBaseNode { get; private set; }
		public TNode CurrentNode { get; private set; }
		public abstract string Title { get; }

		protected InspectorView view;
		protected IHexGridController gridController;
		protected ITweakerLogger logger = LogManager.GetCurrentClassLogger();

		public InspectorController(InspectorView view, IHexGridController gridController)
		{
			this.view = view;
			this.gridController = gridController;
			ConfigureViews();
		}

		public void InspectNode(BaseNode node)
		{
			if(!(node is TNode))
			{
				logger.Error("Invalid node tpye '{0}' passed to controller of type {1}", node.GetType().Name, GetType().Name);
				return;
			}

			CurrentBaseNode = node;
			CurrentNode = node as TNode;
			OnInspectNode();	
		}

		protected virtual void OnInspectNode()
		{
			view.Header.TitleText.text = Title;
		}

		public virtual void Destroy()
		{
			if (Closed != null)
			{
				Closed();
				Closed = null;
			}

			view.Background.DoneButton.onClick.RemoveAllListeners();
			view.DestroySelf();
		}

		protected virtual void ConfigureViews()
		{
			view.Background.DoneButton.onClick.AddListener(DoneClicked);
		}

		private void DoneClicked()
		{
			Destroy();
		}
	}
}
