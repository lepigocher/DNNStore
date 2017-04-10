/*
'  DNN (formerly DotNetNuke) - http://www.dnnsoftware.com
'  Copyright (c) 2002-2016
'  by DNN Corp
' 
'  Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
'  documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
'  the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and 
'  to permit persons to whom the Software is furnished to do so, subject to the following conditions:
' 
'  The above copyright notice and this permission notice shall be included in all copies or substantial portions 
'  of the Software.
' 
'  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
'  TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
'  THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
'  CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
'  DEALINGS IN THE SOFTWARE.
*/

using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace DotNetNuke.Modules.Store.Providers
{
    public static class ProviderSettingsHelper
    {
        public static string SerializeSettings(object settings, Type type)
        {
            string xmlValue;

            StringBuilder stringBuilder = new StringBuilder();
            XmlWriterSettings writerSettings = new XmlWriterSettings { Indent = false, OmitXmlDeclaration = true, Encoding = Encoding.UTF8 };
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add(string.Empty, string.Empty);
            XmlSerializer xmlSerializer = new XmlSerializer(type);

            using (XmlWriter xmlWriter = XmlWriter.Create(stringBuilder, writerSettings))
            {
                xmlSerializer.Serialize(xmlWriter, settings, ns);
                xmlValue = stringBuilder.ToString();
                xmlWriter.Close();
            }

            return xmlValue;
        }

        public static object DeserializeSettings(string xmlValue, Type type)
        {
            object result = null;

            if (!string.IsNullOrEmpty(xmlValue))
            {
                try
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(type);
                    using (StringReader reader = new StringReader(xmlValue))
                    {
                        result = xmlSerializer.Deserialize(reader);
                        reader.Close();
                    }
                }
                catch (Exception)
                {
                    // Do nothing when the schema is not recognized!
                }
            }

            return result;
        }
    }
}
