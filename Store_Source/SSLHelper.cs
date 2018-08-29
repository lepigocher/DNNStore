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
using System.Web;

namespace DotNetNuke.Modules.Store.WebControls
{
	/// <summary>
	/// Provides static methods for ensuring that a page is rendered 
	/// securely via SSL or unsecurely.
	/// </summary>
	public static class SSLHelper
	{
		// Protocol prefixes.
		private const string UnsecureProtocolPrefix = "http";
		private const string SecureProtocolPrefix = "https";

	    /// <summary>
		/// Determines the secure page that should be requested if a redirect occurs.
		/// </summary>
		/// <param name="ignoreCurrentProtocol">
		/// A flag indicating whether or not to ingore the current protocol when determining.
		/// </param>
		/// <returns>A string containing the absolute URL of the secure page to redirect to.</returns>
		public static string DetermineSecurePage(bool ignoreCurrentProtocol) 
		{
			string result = null;

            UriBuilder ub = new UriBuilder(HttpContext.Current.Request.Url.AbsoluteUri);
            // Is this request already secure?
            if (ignoreCurrentProtocol || ub.Scheme == UnsecureProtocolPrefix)
            {
                // Replace the protocol of the requested URL with "https".
                ub.Scheme = SecureProtocolPrefix;
                result = ub.ToString();
            }

			return result;
		}

		/// <summary>
		/// Requests the current page over a secure connection, if it is not already.
		/// </summary>
		public static void RequestSecurePage() 
		{
			// Determine the response path, if any.
			string responsePath = DetermineSecurePage(false);
            if (string.IsNullOrEmpty(responsePath) == false && responsePath != HttpContext.Current.Request.Url.AbsoluteUri)
			{
				// Redirect to the secure page.
				HttpContext.Current.Response.Redirect(responsePath, true);
			}
		}
	}
}
