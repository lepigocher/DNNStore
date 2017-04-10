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
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reflection;

using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Services.Localization;

namespace DotNetNuke.Modules.Store.Components
{
	/// <summary>
	/// Abtract class used to manage querystring parameters.
	/// </summary>
	public abstract class NavigateWrapper
    {
        #region Private Members

        private const string ModuleSharedResources = "~/DesktopModules/Store/App_LocalResources/SharedResources.resx";

        #endregion

        #region Constructors

        protected NavigateWrapper()
        {
            LoadQueryString(new NameValueCollection());
        }

        protected NavigateWrapper(NameValueCollection queryString)
        {
            LoadQueryString(queryString);
        }

        protected NavigateWrapper(int moduleId, NameValueCollection queryString)
        {
            LoadQueryString(queryString);
            ModuleID = moduleId;
        }

        #endregion

        #region Properties

        public int TabID { get; set; }

        public int ModuleID { get; set; }

        public int UserID { get; set; }

        public int CurrentPage { get; set; }

        public string ControlKey { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Parses the queryString parameters and sets properties, if they exist,
        /// in the derived object.
        /// </summary>
        /// <param name="queryString">Querystring</param>
        public void LoadQueryString(NameValueCollection queryString)
        {
            PropertyInfo[] propertyList = GetProperties();

            // Iterate thru all properties for this type
            foreach (PropertyInfo property in propertyList)
            {
                // Do we have a value for this property?
                object value = queryString[property.Name];
                if (value != null)
                    property.SetValue(this, Convert.ChangeType(value, property.PropertyType), null);
                else
                    property.SetValue(this, Null.SetNull(property), null);
            }
        }

        /// <summary>
        /// Gets an array of querystring parameters to be used with the 
        /// DotNetNuke NavigateUrl(), EditUrl() etc.
        /// </summary>
        /// <returns>Querystring parameters</returns>
        public string[] GetNavigateParameters()
        {
            return GetNavigateParameters(new StringDictionary());
        }

        /// <summary>
        /// Gets an array of querystring parameters to be used with the 
        /// DotNetNuke NavigateUrl(), EditUrl() etc.
        /// </summary>
        /// <param name="replaceParams">List of parameters to use as replacements for existing parameters.</param>
        /// <returns>Querystring parameters</returns>
        public string[] GetNavigateParameters(StringDictionary replaceParams)
        {
            PropertyInfo[] propertyList = GetProperties();
            List<string> settingList = new List<string>(propertyList.Length);
            object propertyValue;

            // Iterate thru all properties for this type
            foreach (PropertyInfo property in propertyList)
            {
                string propertyName = property.Name;
                if (replaceParams.ContainsKey(propertyName))
                {
                    string replaceValue = replaceParams[propertyName];
                    if (!string.IsNullOrEmpty(replaceValue))
                    {
                        // Add name and value pair for NavigateUrl() parameters
                        settingList.Add(BuildParameter(propertyName, replaceValue));
                    }
                }
                else
                {
                    if (propertyName != "TabID" && propertyName != "ControlKey")
                    {
                        propertyValue = property.GetValue(this, null);
                        if (!IsNull(propertyValue))
                        {
                            // Add name and value pair for NavigateUrl() parameters
                            settingList.Add(BuildParameter(propertyName, propertyValue.ToString()));
                        }
                    }
                }
            }
            // Cast to string array and return
            return (settingList.ToArray());
        }

        /// <summary>
        /// Gets the url for navigation including the appropriate parameters.
        /// </summary>
        /// <returns>URL</returns>
        public string GetNavigationUrl()
        {
            return Globals.NavigateURL(TabID, ControlKey, GetNavigateParameters()).ToLower();
        }

        /// <summary>
        /// Gets the url for navigation including the appropriate parameters.
        /// </summary>
        /// <param name="replaceParams">List of parameters to use as replacements for existing parameters.</param>
        /// <returns>URL</returns>
        public string GetNavigationUrl(StringDictionary replaceParams)
        {
            return Globals.NavigateURL(TabID, ControlKey, GetNavigateParameters(replaceParams)).ToLower();
        }

        /// <summary>
        /// Gets the url for navigation including the appropriate parameters.
        /// </summary>
        /// <param name="tabID">Current tab ID.</param>
        /// <param name="replaceParams">List of parameters to use as replacements for existing parameters.</param>
        /// <returns>URL</returns>
        public string GetNavigationUrl(int tabID, StringDictionary replaceParams)
        {
            return Globals.NavigateURL(tabID, ControlKey, GetNavigateParameters(replaceParams)).ToLower();
        }

        public string GetEditUrl()
        {
            return GetEditUrl("", "", ControlKey);
        }

        public string GetEditUrl(int moduleId)
        {
            return GetEditUrl(moduleId, "", "", ControlKey);
        }

        public string GetEditUrl(string controlKey)
        {
            return GetEditUrl("", "", controlKey);
        }

        public string GetEditUrl(int moduleId, string controlKey)
        {
            return GetEditUrl(moduleId, "", "", controlKey);
        }

        public string GetEditUrl(string keyName, string keyValue)
        {
            return GetEditUrl(keyName, keyValue, ControlKey);
        }

        public string GetEditUrl(int moduleId, string keyName, string keyValue)
        {
            return GetEditUrl(moduleId, keyName, keyValue, ControlKey);
        }

        public string GetEditUrl(string keyName, string keyValue, string controlKey)
        {
            if (ModuleID == Null.NullInteger)
                throw new Exception("GetEditUrl: ModuleID is required!");

            StringDictionary replaceParams = new StringDictionary();
            replaceParams.Add("mid", ModuleID.ToString());

            if (!string.IsNullOrEmpty(keyName) && !string.IsNullOrEmpty(keyValue))
                replaceParams.Add(keyName, keyValue);

            if (string.IsNullOrEmpty(controlKey))
                controlKey = "Edit";

            return Globals.NavigateURL(TabID, controlKey, GetNavigateParameters(replaceParams));
        }

        public string GetEditUrl(int moduleId, string keyName, string keyValue, string controlKey)
        {
            ModuleID = moduleId;
            return GetEditUrl(keyName, keyValue, controlKey);
        }

        #endregion

        #region Private Methods

        private PropertyInfo[] GetProperties()
        {
            string cacheKey = GetType().FullName;
            PropertyInfo[] propertyList = (PropertyInfo[])DataCache.GetCache(cacheKey);
            if (propertyList == null)
            {
                propertyList = GetType().GetProperties();
                DataCache.SetCache(cacheKey, propertyList);
            }
            return propertyList;
        }

        private bool IsNull(object value)
        {
            if (value is long && value.Equals((object)(Convert.ToInt64(Null.NullInteger))))
                return true;

            return Null.IsNull(value);
        }

        private string LocalizeSharedString(string key)
        {
            return Localization.GetString(key, Globals.ResolveUrl(ModuleSharedResources));
        }

        private string BuildParameter(string name, string value)
        {
            string setting = string.Empty;

            switch (name)
            {
                case "Product":
                    // Replace product property by product slug
                    setting = string.Format("{0}={1}", LocalizeSharedString("ProductSlug"), value);
                    break;
                case "Category":
                    // Replace category property by category slug
                    setting = string.Format("{0}={1}", LocalizeSharedString("CategorySlug"), value);
                    break;
                case "ModuleID":
                    setting = string.Format("mid={0}", value);
                    break;
                default:
                    setting = string.Format("{0}={1}", name, value);
                    break;
            }

            return setting;
        }

        #endregion
    }
}
