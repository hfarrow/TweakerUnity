using System;
using System.Collections.Generic;
using Ghostbit.Tweaker.Core;
using UnityEngine;

namespace Ghostbit.Tweaker.UI
{
	public class GroupInspectorController : InspectorController<GroupNode>
	{
		public override string Title { get { return CurrentNode.FullName; } }

		public GroupInspectorController(InspectorView view, IHexGridController gridController)
			: base(view, gridController)
		{
			
		}

		protected override void OnInspectNode()
		{
			if(CurrentNode.Type != BaseNode.NodeType.Group)
			{
				logger.Error("Invalid node type assigned to group controller: {0}", CurrentNode.Type);
				return;
			}

			base.OnInspectNode();
		}

		public override void Destroy()
		{
			base.Destroy();
		}
	}
}
