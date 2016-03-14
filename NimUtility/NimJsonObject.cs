using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace NimUtility
{
    public interface IJsonObject
    {
        string Serialize();
    }

    public class NimJsonObject<T> : IJsonObject
    {
        public virtual string Serialize()
        {
            return NimUtility.Json.JsonParser.Serialize(this);
        }

        public static T Deserialize(string json)
        {
            if (string.IsNullOrEmpty(json))
                return default(T);
            try
            {
                Newtonsoft.Json.JsonSerializerSettings setting = new Newtonsoft.Json.JsonSerializerSettings();
                setting.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                setting.DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Ignore;
                var jObj = JObject.Parse(json);
                T ret = jObj.ToObject<T>();
                return ret;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("SDKJsonObject.Deserialize\r\n" + e.ToString() + "\r\njson:" + json);
                return default(T);
            }
        }
    }
}
