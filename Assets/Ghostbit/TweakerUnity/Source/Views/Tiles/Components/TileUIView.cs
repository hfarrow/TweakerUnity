using UnityEngine;
using System.Collections;
using Ghostbit.Tweaker.UI;
using Ghostbit.Tweaker.Core;
using UnityEngine.UI;
using System;

namespace Ghostbit.Tweaker.UI
{
	public class TileUIView : MonoBehaviour
	{
		public Text NameText;

		public string Name
		{
			get { return NameText.text; }
			set { NameText.text = value; }
		}
	}
}
