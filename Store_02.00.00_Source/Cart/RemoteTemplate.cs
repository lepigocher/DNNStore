/*
'  DNN (formerly DotNetNuke) - http://www.dnnsoftware.com
'  Copyright (c) 2002-2016
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

using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;

namespace DotNetNuke.Modules.Store.Cart
{
    /// <summary>
    /// Can be used by gateway providers to post a form to a payment server.
    /// The form is build from a template containing tokens.
    /// </summary>
    public sealed class RemoteTemplate
    {
        #region Private Members

        private static readonly Regex RegTokens = new Regex("\\[\\w+\\]", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        #endregion

        #region Public Methods

        public string LoadTemplate(string templatePath, string templateName)
        {
            string template = string.Empty;

            if (string.IsNullOrEmpty(templatePath) == false)
            {
                if (Directory.Exists(templatePath))
                {
                    if (string.IsNullOrEmpty(templateName) == false)
                    {
                        string file = Path.Combine(templatePath, templateName);
                        if (File.Exists(file))
                        {
                            FileInfo fileInfo = new FileInfo(file);
                            using (StreamReader reader = new StreamReader(fileInfo.FullName))
                            {
                                template = reader.ReadToEnd();
                                reader.Close();
                            }
                        }
                        else
                            throw new FileNotFoundException("Template file not found!");
                    }
                    else
                        throw new ArgumentNullException("templateName");
                }
                else
                    throw new DirectoryNotFoundException("Template folder not found!");
            }
            else
                throw new ArgumentNullException("templatePath");

            return template;
        }

        public delegate string ProcessTokenDelegate(string tokenName);

        public string TokenReplace(string template, ProcessTokenDelegate processTokenDelegate)
        {
            // Replace tokens
            MatchCollection matchCol = RegTokens.Matches(template);

            if (matchCol.Count > 0)
            {
                foreach (Match match in matchCol)
                {
                    string token = match.Value.ToUpper();
                    template = template.Replace(token, processTokenDelegate(token));
                }
            }
            return template;
        }

        public void Post(string html)
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Write(html);
            HttpContext.Current.Response.End();
        }

        #endregion
    }
}
