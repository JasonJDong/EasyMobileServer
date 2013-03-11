using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace DMobile.Server.Utilities
{
    public class SerializerProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlSerializer"></param>
        /// <param name="xml"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static object DeserializeByXml(XmlSerializer xmlSerializer, string xml, Encoding encoding)
        {
            object t;
            if (string.IsNullOrWhiteSpace(xml))
            {
                return null;
            }

            byte[] dataBuffer = encoding.GetBytes(xml);

            using (var stream = new MemoryStream(dataBuffer))
            {
                try
                {
                    t = xmlSerializer.Deserialize(stream);
                }
                catch
                {
                    t = null;
                }
            }

            return t;
        }

        /// <summary>
        /// 默认使用UTF-8编码
        /// </summary>
        /// <param name="xmlSerializer"></param>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static object DeserializeByXml(XmlSerializer xmlSerializer, string xml)
        {
            return DeserializeByXml(xmlSerializer, xml, Encoding.UTF8);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlSerializer"></param>
        /// <param name="obj"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string SerializeToXml(XmlSerializer xmlSerializer, object obj, Encoding encoding)
        {
            if (obj == null)
            {
                return string.Empty;
            }
            try
            {
                using (var ms = new MemoryStream())
                {
                    xmlSerializer.Serialize(ms, obj);
                    ms.Flush();
                    ms.Position = 0;
                    using (TextReader reader = new StreamReader(ms, encoding))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 默认使用UTF-8编码
        /// </summary>
        /// <param name="xmlSerializer"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string SerializeToXml(XmlSerializer xmlSerializer, object obj)
        {
            return SerializeToXml(xmlSerializer, obj, Encoding.UTF8);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jsonSerializer"></param>
        /// <param name="obj"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string SerializeToJson(DataContractJsonSerializer jsonSerializer, object obj, Encoding encoding)
        {
            if (obj == null)
            {
                return string.Empty;
            }
            try
            {
                using (var ms = new MemoryStream())
                {
                    jsonSerializer.WriteObject(ms, obj);
                    ms.Flush();
                    ms.Position = 0;
                    using (TextReader reader = new StreamReader(ms, encoding))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 默认使用UTF-8编码
        /// </summary>
        /// <param name="jsonSerializer"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string SerializeToJson(DataContractJsonSerializer jsonSerializer, object obj)
        {
            return SerializeToJson(jsonSerializer, obj, Encoding.UTF8);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jsonSerializer"></param>
        /// <param name="json"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static object DeserializeByJson(DataContractJsonSerializer jsonSerializer, string json, Encoding encoding)
        {
            object retValue;
            if (string.IsNullOrWhiteSpace(json))
            {
                return null;
            }
            byte[] bytes = encoding.GetBytes(json);
            using (var ms = new MemoryStream(bytes))
            {
                retValue = jsonSerializer.ReadObject(ms);
            }

            return retValue;
        }

        /// <summary>
        /// 默认使用UTF-8编码
        /// </summary>
        /// <param name="jsonSerializer"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        public static object DeserializeByJson(DataContractJsonSerializer jsonSerializer, string json)
        {
            return DeserializeByJson(jsonSerializer, json, Encoding.UTF8);
        }

        #region Nested type: SerializerHelper

        /// <summary>
        /// SerializerHelper
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static class SerializerHelper<T> where T : new()
        {
            /// <summary>
            /// Saves as XML.
            /// </summary>
            /// <param name="t">The t.</param>
            /// <param name="fileName">Name of the file.</param>
            /// <returns></returns>
            public static bool SaveAsXml(T t, string fileName)
            {
                return SerializerHelper.SaveAsXml(t, fileName);
            }

            public static bool SaveAsString(T t, StringBuilder stringBuilder)
            {
                return SerializerHelper.SaveAsString(t, stringBuilder);
            }

            /// <summary>
            /// Loads from XML.
            /// </summary>
            /// <param name="fileName">Name of the file.</param>
            /// <returns></returns>
            public static T LoadFromXmlFile(string fileName)
            {
                return (T) SerializerHelper.LoadFromXml(fileName, typeof (T));
            }

            public static T LoadFromString(string data)
            {
                return (T) SerializerHelper.LoadFromString(data, typeof (T));
            }

            /// <summary>
            /// Saves as binary.
            /// </summary>
            /// <param name="t">The t.</param>
            /// <param name="fileName">Name of the file.</param>
            /// <returns></returns>
            public static bool SaveAsBinary(T t, string fileName)
            {
                return SerializerHelper.SaveAsBinary(t, fileName);
            }

            /// <summary>
            /// Loads from binary.
            /// </summary>
            /// <param name="fileName">Name of the file.</param>
            /// <returns></returns>
            public static T LoadFromBinary(string fileName)
            {
                return (T) SerializerHelper.LoadFromBinary(fileName, typeof (T));
            }

            /// <summary>
            /// Equalses the specified t.
            /// </summary>
            /// <param name="t">The t.</param>
            /// <param name="fileName">Name of the file.</param>
            /// <returns>Return true,if equ</returns>
            public static bool Equals(T t, string fileName)
            {
                return SerializerHelper.Equals(t, fileName);
            }
        }

        /// <summary>
        /// SerializerHelper
        /// </summary>
        public static class SerializerHelper
        {
            private static readonly object compareLocker = new object();

            /// <summary>
            /// Saves as XML.
            /// </summary>
            /// <param name="obj">The obj.</param>
            /// <param name="fileName">Name of the file.</param>
            /// <returns></returns>
            public static bool SaveAsXml(object obj, string fileName)
            {
                if (fileName == null) throw new ArgumentNullException("fileName");
                if (string.IsNullOrEmpty(fileName))
                    return false;

                Stream fs = null;

                try
                {
                    var xs = new XmlSerializer(obj.GetType());
                    fs = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite, FileShare.None);
                    var sw = new StreamWriter(fs, Encoding.Default);
                    var xsn = new XmlSerializerNamespaces();
                    xsn.Add("", "");
                    xs.Serialize(sw, obj, xsn);
                }
                catch
                {
                    return false;
                }
                finally
                {
                    if (fs != null)
                        fs.Close();
                }

                return true;
            }

            public static bool SaveAsString(object obj, StringBuilder stringBuilder)
            {
                if (stringBuilder == null) throw new ArgumentNullException("stringBuilder");
                if (obj == null)
                {
                    return false;
                }
                try
                {
                    var xs = new XmlSerializer(obj.GetType());
                    using (TextWriter tw = new StringWriter(stringBuilder))
                    {
                        var xsn = new XmlSerializerNamespaces();
                        xsn.Add("", "");
                        xs.Serialize(tw, obj, xsn);
                    }
                }
                catch
                {
                    return false;
                }

                return true;
            }

            /// <summary>
            /// Loads from XML.
            /// </summary>
            /// <param name="fileName">Name of the file.</param>
            /// <param name="type">The type.</param>
            /// <returns></returns>
            public static object LoadFromXml(string fileName, Type type)
            {
                object t;
                if (string.IsNullOrEmpty(fileName))
                    return null;

                if (!File.Exists(fileName))
                {
                    t = Activator.CreateInstance(type);
                    SaveAsXml(t, fileName);
                }

                Stream stream = null;
                var xs = new XmlSerializer(type);

                try
                {
                    stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                    t = xs.Deserialize(stream);
                }
                catch
                {
                    t = null;
                }
                finally
                {
                    if (stream != null)
                        stream.Close();
                }

                return t;
            }

            public static object LoadFromString(string data, Type type)
            {
                object t;
                if (string.IsNullOrEmpty(data))
                    return null;

                Stream stream = null;
                var xs = new XmlSerializer(type);

                byte[] dataBuffer = Encoding.Unicode.GetBytes(data);
                try
                {
                    stream = new MemoryStream(dataBuffer);
                    t = xs.Deserialize(stream);
                }
                catch
                {
                    t = null;
                }
                finally
                {
                    if (stream != null)
                        stream.Close();
                }

                return t;
            }

            ///// <summary>
            ///// Loads from json.
            ///// </summary>
            ///// <param name="fileName">Name of the file.</param>
            ///// <param name="type">The type.</param>
            ///// <returns></returns>
            //public static object LoadFromJson(string fileName, Type type)
            //{
            //    object t = null;
            //    if (string.IsNullOrEmpty(fileName))
            //        return t;

            //    if (!File.Exists(fileName))
            //    {
            //        t = Activator.CreateInstance(type);
            //        SaveAsJson(t, fileName);
            //    }

            //    try
            //    {
            //        string txt = File.ReadAllText(fileName, Encoding.Default);
            //        //JavaScriptSerializer ser = new JavaScriptSerializer();
            //        //return ser.DeserializeObject(txt);
            //        return Newtonsoft.Json.JsonConvert.DeserializeObject(txt, type);
            //    }
            //    catch
            //    {
            //        t = null;
            //    }

            //    return t;
            //}

            ///// <summary>
            ///// Saves as json.
            ///// </summary>
            ///// <param name="obj">The obj.</param>
            ///// <param name="fileName">Name of the file.</param>
            ///// <returns></returns>
            //public static bool SaveAsJson(object obj, string fileName)
            //{
            //    if (fileName == null) throw new ArgumentNullException("fileName");
            //    if (string.IsNullOrEmpty(fileName))
            //        return false;

            //    try
            //    {
            //        //JavaScriptSerializer ser = new JavaScriptSerializer();
            //        //string txt = ser.DeserializeByXml(obj);
            //        string txt = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            //        File.WriteAllText(fileName, txt, Encoding.Default);
            //    }
            //    catch (Exception ex)
            //    {
            //        Debug.Fail(ex.Message);
            //        return false;
            //    }

            //    return true;
            //}

            /// <summary>
            /// Loads from binary.
            /// </summary>
            /// <param name="fileName">Name of the file.</param>
            /// <param name="type">The type.</param>
            /// <returns></returns>
            public static object LoadFromBinary(string fileName, Type type)
            {
                object t;
                if (string.IsNullOrEmpty(fileName))
                    return null;

                if (!File.Exists(fileName))
                {
                    t = Activator.CreateInstance(type);
                    SaveAsBinary(t, fileName);
                }

                Stream fs = null;

                try
                {
                    var ser = new BinaryFormatter();
                    fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                    t = ser.Deserialize(fs);
                }
                catch
                {
                    t = null;
                }
                finally
                {
                    if (fs != null)
                        fs.Close();
                }

                return t;
            }

            /// <summary>
            /// Equalses the specified obj.
            /// </summary>
            /// <param name="obj">The obj.</param>
            /// <param name="fileName">Name of the file.</param>
            /// <returns></returns>
            public static bool Equals(object obj, string fileName)
            {
                lock (compareLocker)
                {
                    bool equals;
                    StreamReader streamReader = null;
                    StreamWriter streamWriter = null;
                    MemoryStream memoryStream = null;
                    try
                    {
                        if (!File.Exists(fileName))
                            return false;

                        streamReader = new StreamReader(fileName);
                        string oldString = streamReader.ReadToEnd();

                        var serialzer = new XmlSerializer(obj.GetType());
                        memoryStream = new MemoryStream();
                        //streamWriter = new StreamWriter(memoryStream);
                        streamWriter = new StreamWriter(memoryStream, Encoding.Default);
                        var xns = new XmlSerializerNamespaces();
                        xns.Add("", "");
                        serialzer.Serialize(streamWriter, obj, xns);
                        string newString = Encoding.Default.GetString(memoryStream.GetBuffer());
                        equals = EqualIgnoreWhiteSpace(oldString, newString);
                    }
                    finally
                    {
                        if (streamReader != null)
                            streamReader.Close();

                        if (streamWriter != null)
                            streamWriter.Close();

                        if (memoryStream != null)
                            memoryStream.Close();
                    }

                    return equals;
                }
            }

            /// <summary>
            /// Equals the ignore white space.
            /// </summary>
            /// <param name="s1">The s1.</param>
            /// <param name="s2">The s2.</param>
            /// <returns></returns>
            private static bool EqualIgnoreWhiteSpace(string s1, string s2)
            {
                const string regexStr = @"<\?xml version=.*\?>";

                var regex = new Regex(regexStr);

                if (regex.IsMatch(s1))
                {
                    s1 = s1.Replace(regex.Match(s1).Value, "");
                }

                if (regex.IsMatch(s2))
                {
                    s2 = s2.Replace(regex.Match(s2).Value, "");
                }

                int n1 = 0;
                int n2 = 0;
                bool equal = true;

                while (n1 < s1.Length && n2 < s2.Length)
                {
                    if (char.IsWhiteSpace(s1[n1]))
                    {
                        n1++;
                        continue;
                    }

                    if (char.IsWhiteSpace(s2[n2]))
                    {
                        n2++;
                        continue;
                    }

                    if (s1[n1] != s2[n2])
                    {
                        equal = false;
                        break;
                    }

                    n1++;
                    n2++;
                }

                return equal;
            }

            /// <summary>
            /// Saves as binary.
            /// </summary>
            /// <param name="obj">The obj.</param>
            /// <param name="fileName">Name of the file.</param>
            /// <returns></returns>
            public static bool SaveAsBinary(object obj, string fileName)
            {
                if (fileName == null) throw new ArgumentNullException("fileName");
                if (string.IsNullOrEmpty(fileName))
                    return false;

                Stream fs = null;
                try
                {
                    var ser = new BinaryFormatter();
                    fs = new FileStream(fileName, FileMode.Create);
                    ser.Serialize(fs, obj);
                }
                catch
                {
                    return false;
                }
                finally
                {
                    if (fs != null)
                        fs.Close();
                }

                return true;
            }
        }

        #endregion
    }

    public static class JSONConvert
    {
        public static T ConvertToObject<T>(string jsonString) where T : class, new()
        {
            return JsonConvert.DeserializeObject<T>(jsonString);
        }

        public static object ConvertToObject(string jsonString, Type forType)
        {
            return JsonConvert.DeserializeObject(jsonString, forType);
        }

        public static string ConvertToString(object instance)
        {
            return JsonConvert.SerializeObject(instance);
        }
    }
}