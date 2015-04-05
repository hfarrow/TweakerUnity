using System;
using System.Collections.Generic;
using System.Text;

namespace Ghostbit.Tweaker.UI
{
	public static class TileDisplay
	{
		public static string GetFriendlyName(string name)
		{
			if(string.IsNullOrEmpty(name))
			{
				return "";
			}

			StringBuilder newName = new StringBuilder(name.Length, name.Length * 2);
			newName.Append(name[0]);
			for (int i = 1; i < name.Length; ++i)
			{
				if(char.IsUpper(name[i]))
				{
					newName.Append(" ");
				}
				newName.Append(name[i]);
			}
			return newName.ToString();
		}
	}
}
