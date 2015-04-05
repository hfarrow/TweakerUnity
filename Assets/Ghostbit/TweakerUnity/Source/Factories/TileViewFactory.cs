using System;
using System.Collections.Generic;
using Ghostbit.Tweaker.Core;
using UnityEngine;

namespace Ghostbit.Tweaker.UI
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

		public TView MakeView<TView>(HexGridCell<BaseNode> cell, uint gridHeight)
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

			// This assumes we are using flat top hexagons
			float tileHeight = (float)Screen.height / (float)(gridHeight + 1);
			float tileWidth = tileHeight / (Mathf.Sqrt(3f) / 2f);
			float tileSize = tileWidth / 2f;

			Vector2 position = HexCoord.AxialToPixel(cell.AxialCoord, tileSize);

			// Offset the position slighlty so that rectangle hex grid looks more centered in the screen.
			position.y += tileSize / 2f;

			viewTransform.anchoredPosition = position;
			viewTransform.sizeDelta = new Vector2(tileWidth, tileHeight);

			return view;
		}
	}
}
