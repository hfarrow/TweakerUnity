﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tweaker.Core
{
	public class InvokableInfo : TweakerObjectInfo
	{
		public string[] ArgDescriptions { get; private set; }
		public string ReturnDescription { get; private set; }

		public InvokableInfo(
			string name,
			uint instanceId = 0,
			ICustomTweakerAttribute[] customAttributes = null,
			string description = "",
			string[] argDescriptions = null,
			string returnDescription = "")
			: base(name, instanceId, customAttributes, description)
		{
			ArgDescriptions = argDescriptions;
			ReturnDescription = returnDescription;
		}
	}
}
