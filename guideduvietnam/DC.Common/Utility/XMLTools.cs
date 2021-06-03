using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using System.IO;


namespace DC.Common.Utility
{
    public static class XMLTools
    {
        /// <summary>
        /// Read xml file and convert to model
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static T Deserialize<T>(string filePath, T data)
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(T));

            try
            {
                TextReader textReader = new StreamReader(filePath);
                data = (T)deserializer.Deserialize(textReader);
                textReader.Close();
            }
            catch
            {
            }
            return data;
        }



        /// <summary>
        /// Save model to xml format
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <param name="data"></param>
        public static void Serializer<T>(string filePath, T data)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));

            try
            {
                TextWriter txtWriter = new StreamWriter(filePath);
                serializer.Serialize(txtWriter, data);
                txtWriter.Close();
            }
            catch
            {
            }
        }
    }
}