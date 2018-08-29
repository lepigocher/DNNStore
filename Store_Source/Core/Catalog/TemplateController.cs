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
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;

using DotNetNuke.Common.Utilities;
using DotNetNuke.Services.Cache;

namespace DotNetNuke.Modules.Store.Core.Catalog
{
    public delegate Control ProcessTokenDelegate(string tokenName);

	/// <summary>
    /// Template business class used to manage catalog templates.
    /// </summary>
    public sealed class TemplateController
	{
		#region Public Methods

        public static List<TemplateInfo> GetTemplates(string templatePath)
		{
            List<TemplateInfo> templateList = null;

            if (Directory.Exists(templatePath))
			{
                string[] fileList = Directory.GetFiles(templatePath, "*.htm");
                templateList = new List<TemplateInfo>(fileList.Length);
				foreach(string file in fileList)
				{
                    try
                    {
                        TemplateInfo templateInfo = new TemplateInfo(file, false, false);
                        templateList.Add(templateInfo);
                    }
                    catch (FileNotFoundException)
                    {
                        // Do nothing if the file is not found
                    }
				}
			}

			return templateList;
		}

		public static TemplateInfo GetTemplate(string templatePath, string templateName, bool parse, bool isLogged)
		{
            string file = Path.Combine(templatePath, templateName);
            string cacheKey = file + parse + isLogged;

            TemplateInfo templateInfo = (TemplateInfo)DataCache.GetCache(cacheKey);
            if (templateInfo == null)
            {
                templateInfo = new TemplateInfo(file, parse, isLogged);
                DNNCacheDependency cacheDep = new DNNCacheDependency(file);
                DataCache.SetCache(cacheKey, templateInfo, cacheDep);
            }
			return templateInfo;
		}

        public static Control ParseTemplate(string templatePath, string templateName, string templateError, bool isLogged, ProcessTokenDelegate processTokenDelegate)
		{
            Control templateControl;
            TemplateInfo templateInfo = GetTemplate(templatePath, templateName, true, isLogged);
            if (templateInfo != null)
            {
                templateControl = new Control();

                List<Token> tokens = templateInfo.Tokens;
                if (tokens != null)
                {
                    foreach (Token token in templateInfo.Tokens)
                    {
                        string tokenValue = token.Value;
                        if (!string.IsNullOrEmpty(tokenValue))
                        {
                            Control tokenControl = processTokenDelegate(tokenValue);
                            if (tokenControl != null)
                            {
                                string[] properties = token.Properties;
                                if (properties != null)
                                {
                                    foreach (string property in properties)
                                    {
                                        tokenControl = SetProperty(tokenControl, property);
                                    }
                                }
                                templateControl.Controls.Add(tokenControl);
                            }
                        }
                    }
                }
            }
            else
            {
                templateControl = new LiteralControl(string.Format(templateError, templateName));
            }
			return templateControl;
		}

		#endregion

		#region Private Methods

		private static Control SetProperty(Control control, string keyValuePair)
		{
            string[] keyValue = keyValuePair.Split(new char[] { '=' });
            string key = keyValue[0];
            string value = keyValue[1];

            // Get the corresponding property
            PropertyInfo property = control.GetType().GetProperty(key);

            // If the property exist
            if (property != null)
            {
			    object objValue = null;

			    switch(property.PropertyType.Name)
			    {
				    case "Boolean":
					    objValue = Convert.ToBoolean(value);
					    break;

                    case "Color":
                        ColorConverter colorConverter = new ColorConverter();
                        objValue = colorConverter.ConvertFrom(value);
                        break;

                    case "CssStyleCollection":
                        int valuePos = keyValuePair.IndexOf('\"') + 1;
                        string[] attributes = keyValuePair.Substring(valuePos, keyValuePair.Length - valuePos - 1).Replace(':', '=').Split(new char[] { ';' });
                        foreach (string attrib in attributes)
                        {
                            if (attrib.Length > 0)
                            {
                                string[] attribKeyValue = attrib.Split(new char[] { '=' });
                                string attribKey = attribKeyValue[0].Trim();
                                string attribValue = attribKeyValue[1].Trim();

                                CssStyleCollection cssStyleColl = (CssStyleCollection)property.GetValue(control, null);
                                cssStyleColl.Add(attribKey, attribValue);
                            }
                        }
					    break;

				    case "Int32":
					    objValue = Convert.ToInt32(value);
					    break;

				    case "Unit":
					    UnitConverter unitConverter = new UnitConverter();
                        objValue = unitConverter.ConvertFromString(value);
					    break;

				    case "String":
					    objValue = value;
					    break;
			    }

			    if (objValue != null)
				    property.SetValue(control, objValue, null);
            }

			return control;
		}

		#endregion
	}
}
