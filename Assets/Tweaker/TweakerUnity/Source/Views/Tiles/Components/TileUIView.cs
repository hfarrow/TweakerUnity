using UnityEngine;
using System.Collections;
using Tweaker.UI;
using Tweaker.Core;
using UnityEngine.UI;
using System;

namespace Tweaker.UI
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
