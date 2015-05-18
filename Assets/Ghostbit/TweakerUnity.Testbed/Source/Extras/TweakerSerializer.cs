using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Tweaker.AssemblyScanner;
using Newtonsoft.Json;
using UnityEngine;

namespace Tweaker.UI.Testbed
{
	public class TweakerSerializer : ITweakerSerializer
	{
		private Dictionary<Type, CustomTypeSerializer> customSerializers;

		public TweakerSerializer(IScanner scanner)
		{
			customSerializers = new Dictionary<Type, CustomTypeSerializer>();

			var processor = new CustomSerializerProcessor();
			scanner.AddProcessor(processor);
			var provider = scanner.GetResultProvider<CustomSerializerResult>();
			provider.ResultProvided += CustomSerializerFound;
		}

		private void CustomSerializerFound(object sender, ScanResultArgs<CustomSerializerResult> e)
		{
			CustomTypeSerializer serializer = Activator.CreateInstance(e.result.type, this) as CustomTypeSerializer;
			customSerializers.Add(serializer.CustomType, serializer);
		}

		public string Serialize(object objectToSerialize, Type objectType)
		{
			CustomTypeSerializer customSerializer;
			customSerializers.TryGetValue(objectType, out customSerializer);
			if (customSerializer != null)
			{
				return customSerializer.Serialize(objectToSerialize);
			}
			else
			{
				StringWriter writer = new StringWriter();
				JsonSerializer ser = new JsonSerializer();
				ser.Serialize(writer, objectToSerialize);

				string result = writer.GetStringBuilder().ToString();
				writer.Close();
				return result;
			}
		}

		public object Deserialize(string stringToDeserialize, Type objectType)
		{
			CustomTypeSerializer customSerializer;
			customSerializers.TryGetValue(objectType, out customSerializer);
			if (customSerializer != null)
			{
				return customSerializer.Deserialize(stringToDeserialize);
			}
			else
			{
				StringReader reader = new StringReader(stringToDeserialize);
				JsonSerializer ser = new JsonSerializer();

				object  result = ser.Deserialize(reader, objectType);
				reader.Close();
				return result;
			}
		}
	}

	public class CustomSerializerResult
	{
		public Type type;
		public CustomSerializerResult(Type type)
		{
			this.type = type;
		}
	}

	public class CustomSerializerProcessor : ITypeScanProcessor<CustomTypeSerializer, CustomSerializerResult>
	{
		public void ProcessType(Type type, IBoundInstance instance = null)
		{
			if(ResultProvided != null)
			{
				ResultProvided(this, new ScanResultArgs<CustomSerializerResult>(new CustomSerializerResult(type)));
			}
		}

		public event EventHandler<ScanResultArgs<CustomSerializerResult>> ResultProvided;
	}

	public abstract class CustomTypeSerializer
	{
		public ITweakerSerializer BaseSerializer { get; private set; }
		public Type CustomType { get; private set; }

		public CustomTypeSerializer(ITweakerSerializer baseSerializer, Type customType)
		{
			BaseSerializer = baseSerializer;
			CustomType = customType;
		}

		public abstract string Serialize(object objectToSerialize);
		public abstract object Deserialize(string stringToDeserialize);
	}

	public class Vector3Serializer : CustomTypeSerializer
	{
		[Serializable]
		private class SerializableVector3
		{
			public float x;
			public float y;
			public float z;
		}

		public Vector3Serializer(ITweakerSerializer baseSerializer)
			: base(baseSerializer, typeof(Vector3))
		{

		}

		public override string Serialize(object objectToSerialize)
		{
			var unityVector = (Vector3)objectToSerialize;
			var vector = new SerializableVector3();
			vector.x = unityVector.x;
			vector.y = unityVector.y;
			vector.z = unityVector.z;
			return BaseSerializer.Serialize(vector, typeof(SerializableVector3));
		}

		public override object Deserialize(string stringToDeserialize)
		{
			SerializableVector3 vector = (SerializableVector3)BaseSerializer.Deserialize(stringToDeserialize, typeof(SerializableVector3));
			var unityVector = new Vector3(vector.x, vector.y, vector.z);
			return unityVector;
		}
	}

	public class Vector2Serializer : CustomTypeSerializer
	{
		[Serializable]
		private class SerializableVector2
		{
			public float x;
			public float y;
		}

		public Vector2Serializer(ITweakerSerializer baseSerializer)
			: base(baseSerializer, typeof(Vector2))
		{

		}

		public override string Serialize(object objectToSerialize)
		{
			var unityVector = (Vector2)objectToSerialize;
			var vector = new SerializableVector2();
			vector.x = unityVector.x;
			vector.y = unityVector.y;
			return BaseSerializer.Serialize(vector, typeof(SerializableVector2));
		}

		public override object Deserialize(string stringToDeserialize)
		{
			SerializableVector2 vector = (SerializableVector2)BaseSerializer.Deserialize(stringToDeserialize, typeof(SerializableVector2));
			var unityVector = new Vector2(vector.x, vector.y);
			return unityVector;
		}
	}
}
