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
using System.IO;
using System.Text.RegularExpressions;

namespace DotNetNuke.Modules.Store.Catalog
{
	/// <summary>
	/// Template class info.
	/// </summary>
    [Serializable]
    public sealed class TemplateInfo
    {
        #region Private Members

        private static readonly Regex RegLogged = new Regex(@"\[IfLogged\](.+)\[/IfLogged\]", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);
        private static readonly Regex RegNotLogged = new Regex(@"\[IfNotLogged\](.+)\[/IfNotLogged\]", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);
        private readonly string _name = string.Empty;
        private readonly string _path = string.Empty;
        private readonly string _content = string.Empty;
        private readonly List<Token> _tokens;

        #endregion

        #region Constructor

        public TemplateInfo(string file, bool parse, bool isLogged)
        {
            if (File.Exists(file))
            {
                string fileContent;
                FileInfo fileInfo = new FileInfo(file);
                using (StreamReader reader = new StreamReader(fileInfo.FullName))
                {
                    fileContent = reader.ReadToEnd();
                    reader.Close();
                }
                _name = fileInfo.Name;
                _path = fileInfo.FullName;
                if (parse)
                {
                    _content = ParseCondition(fileContent, isLogged);
                    string[] tokenContent = _content.Split(new char[] { '[', ']' });
                    _tokens = new List<Token>(tokenContent.Length);
                    foreach (string token in tokenContent)
                    {
                        Token tokenTemplate = new Token(token);
                        _tokens.Add(tokenTemplate);
                    }
                }
                else
                    _content = fileContent;
            }
            else
                throw new FileNotFoundException();
        }

        #endregion

        #region Properties

        public string Name
        {
            get { return _name; }
        }

        public string Path
        {
            get { return _path; }
        }

        public string Content
        {
            get { return _content; }
        }

        public List<Token> Tokens
        {
            get { return _tokens; }
        }

        #endregion

        #region Private Methods

        private static string ParseCondition(string fileContent, bool isLogged)
        {
            string temp = RegLogged.Replace(fileContent, isLogged ? "$1" : "");
            return RegNotLogged.Replace(temp, !isLogged ? "$1" : "");
        }

        #endregion
    }
}
