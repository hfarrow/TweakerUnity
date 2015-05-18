using System;
using System.Collections.Generic;
using Tweaker.Core;
using UnityEngine;

namespace Tweaker.UI
{
	public class TileViewFactory
	{
		private Dictionary<Type, TileView> tilePrefabMap;
		private TileView defaultViewPrefab;
		private Func<UnityEngine.Object, UnityEngine.Object> InstantiateFunc;
		private RectTransform tileContainer;
		private ITweakerLogger logger = LogManager.GetCurrentClassLogger();

		public TileViewFactory(
			Dictionary<Type, TileView> tilePrefabMap, 
			TileView defaultViewPrefab,
			Func<UnityEngine.Object, UnityEngine.Object> InstantiateFunc,
			RectTransform tileContainer)
		{
			this.tilePrefabMap = tilePrefabMap;
			this.defaultViewPrefab = defaultViewPrefab;
			this.InstantiateFunc = InstantiateFunc;
			this.tileContainer = tileContainer;
		}

		public TView MakeView<TView>(HexGridCell<BaseNode> cell, uint gridWidth, uint gridHeight)
			where TView : TileView
		{
			TileView viewPrefab;
			if (!tilePrefabMap.TryGetValue(cell.Value.GetType(), out viewPrefab))
			{
				logger.Error("Could not find a prefab mapping for type '{0}'.", cell.Value.GetType().FullName);
				viewPrefab = defaultViewPrefab;
			}

			TView view = InstantiateFunc(viewPrefab) as TView;
			if (view == null)
			{
				logger.Error("Failed to Instantiate view prefab <{0}> as TileView", viewPrefab);
				return null;
			}

			var viewTransform = view.GetComponent<RectTransform>();
			viewTransform.SetParent(tileContainer, false);
			view.name = cell.AxialCoord.ToString();
			view.gameObject.SetActive(true);

			// This assumes we are using flat top hexagons and that we want to fit the grid to the screen size.
			float tileHeight = (float)Screen.height / (float)(gridHeight + 1);
			float tileWidth = tileHeight / (Mathf.Sqrt(3f) / 2f);
			float tileSize = tileWidth / 2f;

			Vector2 position = HexCoord.AxialToPixel(cell.AxialCoord, tileSize).ToVector();

			// Offset the position so that rectangle hex grid looks more centered in the screen.
			// The direction to offset depends on the grid width because the height of each
			// column is different based on being an even numbered column or not.
			float yOffset = tileHeight / 4f;
			if(gridWidth % 2 == 0)
			{
				position.y -= yOffset;
				position.x += (3f / 8f) * tileWidth;
			}
			else
			{
				position.y -= yOffset;
			}

			if(gridHeight % 2 == 0)
			{
				position.y -= tileHeight / 2;
			}

			viewTransform.anchoredPosition = position;
			viewTransform.sizeDelta = new Vector2(tileWidth, tileHeight);

			return view;
		}
	}
}
