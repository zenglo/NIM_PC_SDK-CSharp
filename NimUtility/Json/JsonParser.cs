using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace NimUtility.Json
{
    public class JsonParser
    {
        /// <summary>
        /// 依据对象反序列化json字串;
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T Deserialize<T>(string json)
        {
            if (string.IsNullOrEmpty(json))
                return default(T);
            return JsonConvert.DeserializeObject<T>(json);
        }

        /// <summary>
        ///  反序列化JSON对象为经典的字典模式
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static IDictionary<string, object> FromJson(string json)
        {
            return Deserialize(json) as IDictionary<string, object>;
        }

        /// <summary>
        /// 反序列化对象;
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static object Deserialize(string json)
        {
            if (string.IsNullOrEmpty(json))
                throw new ArgumentException("JSON string can not be null or empty", "json");
            var converters = new Newtonsoft.Json.JsonConverter[] { new JsonKeyValuePairConverter() };
            var v = JsonConvert.DeserializeObject<object>(json, converters);
            return v;
        }

        public static object DeserializeObject(string json)
        {
            if (string.IsNullOrEmpty(json))
                throw new ArgumentException("JSON string can not be null or empty", "json");
            return JsonConvert.DeserializeObject(json);
        }

        /// <summary>
        /// Deserializes the JSON to the given anonymous type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="anonymousType"></param>
        /// <returns></returns>
        public static T DeserializeAnonymousType<T>(string value, T anonymousType)
        {
            return JsonConvert.DeserializeAnonymousType<T>(value, anonymousType);
        }

        /// <summary>
        /// 序列化对象;
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        /// <summary>
        /// 序列化字典对象;
        /// </summary>
        /// <param name="bag"></param>
        /// <returns></returns>
        public static string ToJson(IDictionary<string, object> bag)
        {
            return Serialize(bag);
        }
    }
}
