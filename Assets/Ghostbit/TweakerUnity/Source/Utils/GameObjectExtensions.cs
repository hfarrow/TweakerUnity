using UnityEngine;
using System.Collections;

namespace UnityEngine
{
	public static class GameObjectExtensions
	{
		public static string GetPath(this Transform current)
		{
			if (current.parent == null)
				return "/" + current.name;
			return current.parent.GetPath() + "/" + current.name;
		}

		public static string GetPath(this GameObject go)
		{
			return go.transform.GetPath();
		}

		public static string GetPath(this Component component)
		{
			return component.transform.GetPath() + "/" + component.GetType().ToString();
		}
	}
}