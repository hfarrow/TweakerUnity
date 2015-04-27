using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ghostbit.Tweaker.Core;

namespace Ghostbit.Tweaker.UI
{
	[Flags]
	public enum TweakableFlags
	{
		None = 0,
		HideRangeSlider = 1
	}

	public class TweakableUIFlagsAttribute : Attribute, ICustomTweakerAttribute
	{
		public readonly TweakableFlags Flags;

		public TweakableUIFlagsAttribute(TweakableFlags flags)
		{
			Flags = flags;
		}
	}

	public static class UIFlagsUtil
	{
		public static bool IsSet(TweakableFlags flag, ITweakable tweakable)
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
