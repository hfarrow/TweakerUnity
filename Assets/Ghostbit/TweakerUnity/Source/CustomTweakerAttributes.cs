using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tweaker.Core;

namespace Tweaker.UI
{
	[Flags]
	public enum TweakableUIFlags
	{
		None = 0,
		HideRangeSlider = 1
	}

	public class TweakableUIFlagsAttribute : Attribute, ICustomTweakerAttribute
	{
		public readonly TweakableUIFlags Flags;

		public TweakableUIFlagsAttribute(TweakableUIFlags flags)
		{
			Flags = flags;
		}
	}

	public static class TweakerFlagsUtil
	{
		public static bool IsSet(TweakableUIFlags flag, ITweakable tweakable)
		{
			var result = false;
			var attribute = tweakable.GetCustomAttribute<TweakableUIFlagsAttribute>();
			if (attribute != null)
			{
				result = (attribute.Flags & flag) == flag;
			}
			return result;
		}
	}
}
