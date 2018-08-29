/*
'  DNN (formerly DotNetNuke) - http://www.dnnsoftware.com
'  Copyright (c) 2002-2018
'  by DNN Corp.
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

using System.Text;
using System.Web;
using System.Collections.Specialized;

namespace DotNetNuke.Modules.Store.Core.Cart
{
    /// <summary>
    /// Can be used by gateway providers to post a minimal form to a payment server.
    /// </summary>
    public sealed class RemoteForm
    {
        #region Constructors

        public RemoteForm()
        {
            Fields = new NameValueCollection();
        }

        public RemoteForm(string formName, string postURL)
        {
            FormName = formName;
            Url = postURL;
            Fields = new NameValueCollection();
        }

        public RemoteForm(string formName, string postURL, NameValueCollection fields)
        {
            FormName = formName;
            Url = postURL;
            Fields = fields;
        }

        #endregion

        #region Properties

        public string FormName { get; set; }

        public string Url { get; set; }

        public NameValueCollection Fields { get; set; }

        #endregion

        #region Public Methods

        public void Add(string fieldName, string value)
        {
            Fields.Add(fieldName, value);
        }

        public void Post()
        {
            Post(true);
        }

        public void Post(bool hidden)
        {
            // Build the HTML form
            StringBuilder form = new StringBuilder();
            form.AppendFormat("<html><head></head><body onload=\"document.{0}.submit()\">", FormName);
            form.AppendFormat("<form name=\"{0}\" method=\"POST\" action=\"{1}\" >", FormName, Url);
            foreach (string key in Fields)
            {
                if (hidden == true)
                    form.AppendFormat("<input type=\"hidden\" name=\"{0}\" value=\"{1}\" \\>", key, HttpContext.Current.Server.HtmlEncode(Fields[key]));
                else
                    form.AppendFormat("<input name=\"{0}\" value=\"{1}\" \\>", key, HttpContext.Current.Server.HtmlEncode(Fields[key]));
            }
            form.Append("</form></body></html>");
            // Clear all and return the form
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Write(form.ToString());
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }

        #endregion
    }
}
