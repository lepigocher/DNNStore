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
using System.Collections.Specialized;
using System.Globalization;
using System.Reflection;

namespace DotNetNuke.Modules.Store.Core.Components
{
	/// <summary>
	/// Abstract class used to manage RequestForm.
	/// </summary>
	public abstract class RequestFormWrapper
	{
        #region Private Members

	    readonly CultureInfo _culture = CultureInfo.InvariantCulture;

        #endregion

		#region Constructors

	    protected RequestFormWrapper()
		{
		}

	    protected RequestFormWrapper(NameValueCollection requestForm)
		{
			LoadRequestForm(requestForm);
		}

		#endregion

        #region Public Methods

        /// <summary>
		/// Parses the Request Form parameters and sets properties, if they exist,
		/// in the derived object.
		/// </summary>
		/// <param name="requestForm"></param>
		public void LoadRequestForm(NameValueCollection requestForm)
		{
			// Iterate thru all properties for this type
			PropertyInfo[] propertyList = GetType().GetProperties();

			foreach(PropertyInfo property in propertyList)
			{
				// Do we have a value for this property?
                string value = requestForm[property.Name];
				if (!string.IsNullOrEmpty(value))
				{
					object objValue = null;

					try
					{
						// Cast to the appropriate type
						switch(property.PropertyType.Name)
						{
							case "String":
								objValue = value;
								break;
							case "Int32":
								objValue = Convert.ToInt32(value);
								break;
							case "Boolean":
								objValue = Convert.ToBoolean(value);
								break;
							case "Decimal":
                                objValue = Convert.ToDecimal(value, _culture);
								break;
						}
					}
					catch
					{
						//Cast failed - Skip this property
					}
					// Set the value
					if (objValue != null)
						property.SetValue(this, objValue, null);
				}
			}
		}

		#endregion
	}
}
