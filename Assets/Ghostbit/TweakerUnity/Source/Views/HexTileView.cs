using UnityEngine;
using System.Collections;
using Ghostbit.Tweaker.UI;
using Ghostbit.Tweaker.Core;
using UnityEngine.UI;

namespace Ghostbit.Tweaker.UI
{
	public class HexTileView : MonoBehaviour
	{
		public Image TileImage;
		public HexGridCell<BaseNode> Cell;
		public Text NameText;

		public Color TileColor
		{
			get { return TileImage.color; }
			set { TileImage.color = value; }
		}

		public float TileAlpha
		{
			get { return TileImage.color.a; }
			set 
			{
				Color color = TileImage.color;
				color.a = value;
				TileImage.color = color; 
			}
		}

		public string Name
		{
			get { return NameText.text; }
			set { NameText.text = value; }
		}
	}
}
