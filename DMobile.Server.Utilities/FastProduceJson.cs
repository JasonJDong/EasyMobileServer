using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;

namespace DMobile.Server.Utilities
{
    [Obsolete("请使用JSONConvert的序列化方法")]
    public class FastProduceJson
    {
        private static readonly Dictionary<Type, DataContractJsonSerializer> SaveJsonSerializers =
            new Dictionary<Type, DataContractJsonSerializer>(32);

        public string GetEntityJsonString(object entity)
        {
            if (entity == null)
            {
                return string.Empty;
            }

            Type entityType = entity.GetType();

            if (entityType == typeof (string) ||
                entityType == typeof (bool))
            {
                return entity.ToString();
            }
            if (!SaveJsonSerializers.ContainsKey(entityType))
            {
                var serializer = new DataContractJsonSerializer(entityType);
                SaveJsonSerializers.Add(entityType, serializer);
            }
            return SerializerProvider.SerializeToJson(SaveJsonSerializers[entityType], entity);
        }

        public T GetEntityJsonInstance<T>(string entityJson) where T : class
        {
            Type entityType = typeof (T);
            if (!SaveJsonSerializers.ContainsKey(entityType))
            {
                var serializer = new DataContractJsonSerializer(entityType);
                SaveJsonSerializers.Add(entityType, serializer);
            }
            return (T) SerializerProvider.DeserializeByJson(SaveJsonSerializers[entityType], entityJson);
        }

        public object GetEntityJsonInstance(string entityJson, Type entityType)
        {
            if (!SaveJsonSerializers.ContainsKey(entityType))
            {
                var serializer = new DataContractJsonSerializer(entityType);
                SaveJsonSerializers.Add(entityType, serializer);
            }
            return SerializerProvider.DeserializeByJson(SaveJsonSerializers[entityType], entityJson);
        }
    }
}