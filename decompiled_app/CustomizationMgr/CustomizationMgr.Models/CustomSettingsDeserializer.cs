using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace CustomizationMgr.Models;

internal static class CustomSettingsDeserializer
{
	internal class CustomSettingsKnownTypesBinder : ISerializationBinder
	{
		public IList<Type> KnownTypes { get; set; }

		public Type BindToType(string assemblyName, string typeName)
		{
			return KnownTypes.SingleOrDefault((Type t) => t.Name == typeName);
		}

		public void BindToName(Type serializedType, out string assemblyName, out string typeName)
		{
			assemblyName = null;
			typeName = serializedType.Name;
		}
	}

	private static CustomSettingsKnownTypesBinder KnownTypesBinder = new CustomSettingsKnownTypesBinder();

	public static CustomAppSettings DeserializeCustomSettings(string File, List<Type> KnownTypes, EventHandler<Newtonsoft.Json.Serialization.ErrorEventArgs> errorHandler = null)
	{
		KnownTypesBinder.KnownTypes = KnownTypes;
		CustomAppSettings result = null;
		if (!System.IO.File.Exists(File))
		{
			return result;
		}
		try
		{
			return JsonConvert.DeserializeObject<CustomAppSettings>(System.IO.File.ReadAllText(File), new JsonSerializerSettings
			{
				TypeNameHandling = TypeNameHandling.All,
				MetadataPropertyHandling = MetadataPropertyHandling.ReadAhead,
				SerializationBinder = KnownTypesBinder,
				Error = errorHandler
			});
		}
		catch (Exception)
		{
			return null;
		}
	}
}
