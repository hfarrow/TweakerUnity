using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ghostbit.Tweaker.UI
{
	public interface ITweakerSerializer
	{
		string Serialize(object objectToSerialize, Type objectType);
		object Deserialize(string stringToDeserialize, Type objectType);
	}
}
