/*
'  DNN (formerly DotNetNuke) - http://www.dnnsoftware.com
'  Copyright (c) 2002-2018
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
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Xml;

namespace DotNetNuke.Modules.Store.Core.Cart
{
    /// <summary>
    /// Summary description for GatewaySettings.
    /// </summary>
    public class GatewaySettings
	{
		#region Constructors

		public GatewaySettings()
		{
		}

		public GatewaySettings(string xml)
		{
			FromString(xml);
		}

		#endregion

		#region Public Methods

		public override string ToString()
		{
            string settings;
            using (StringWriter stringWriter = new StringWriter())
            {
                XmlWriterSettings writerSettings = new XmlWriterSettings
                {
                    OmitXmlDeclaration = true,
                    ConformanceLevel = ConformanceLevel.Fragment,
                    Indent = true,
                    CloseOutput = true
                };
                using (XmlWriter xmlWriter = XmlWriter.Create(stringWriter, writerSettings))
                {
                    try
                    {
                        xmlWriter.WriteStartElement(GetType().Name);
                        PropertyInfo[] propertyList = GetType().GetProperties();
                        foreach (PropertyInfo property in propertyList)
                        {
                            // Add "node" for this property
                            object objValue = property.GetValue(this, null);
                            if (objValue != null)
                            {
                                string value;
                                switch (property.PropertyType.Name)
                                {
                                    case "Decimal":
                                        decimal Value = (decimal)objValue;
                                        value = Value.ToString("0.00", CultureInfo.InvariantCulture.NumberFormat);
                                        break;
                                    default:
                                        value = objValue.ToString();
                                        break;
                                }
                                if (string.IsNullOrEmpty(value) == false)
                                    xmlWriter.WriteElementString(property.Name, value);
                            }
                        }
                        xmlWriter.WriteEndElement();
                        xmlWriter.Close();
                    }
                    catch
                    {
                    }
                }
                settings = stringWriter.ToString();
            }
            return settings;
		}

		public virtual void FromString(string xml)
		{
			try
			{
				XmlDocument xmlDoc = new XmlDocument();
				xmlDoc.LoadXml(xml);

				// Iterate thru all "public" properties for this type
				PropertyInfo[] propertyList = GetType().GetProperties();
				foreach(PropertyInfo property in propertyList)
				{
                    if (property.CanWrite && xmlDoc.DocumentElement[property.Name] != null)
					{
						string xmlValue = xmlDoc.DocumentElement[property.Name].InnerText;
						object objValue;

						// Cast to the appropriate type
						switch(property.PropertyType.Name)
						{
							case "String":
								objValue = xmlValue;
								break;
							case "Int32":
								objValue = Convert.ToInt32(xmlValue);
								break;
                            case "Decimal":
                                objValue = Convert.ToDecimal(xmlValue, CultureInfo.InvariantCulture.NumberFormat);
                                break;
							case "Boolean":
								objValue = Convert.ToBoolean(xmlValue);
								break;
							default:
                                objValue = GetCustomType(property.PropertyType.Name, xmlValue);
                                break;
						}

						// Set the value
						if (objValue != null)
							property.SetValue(this, objValue, null);
					}
				}
			}
			catch
			{
				// FAILURE - stop loading from string
			}
		}

        public virtual object GetCustomType(string typeName, string stringValue)
		{
			return null;
		}

		public virtual bool IsValid()
		{
			return false;
		}

		#endregion
	}
}
