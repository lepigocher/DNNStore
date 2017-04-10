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
using System.Xml.Serialization;

namespace DotNetNuke.Modules.Store.Providers
{
	/// <summary>
	/// Summary description for ProviderInfo.
	/// </summary>
	[XmlRoot("provider")]
    [Serializable]
    public sealed class ProviderInfo
	{
		#region Private Declarations

		private string _name = string.Empty;
        private string _description = string.Empty;
        private string _class = string.Empty;
        private string _assembly = string.Empty;
        private string _path = string.Empty;
        private string _virtualPath = string.Empty;
        private StoreProviderType _type;

		#endregion

		#region Properties

		[XmlElement("name")]
		public string Name
		{
			get { return _name; }
			set { _name = value; }
		}

		[XmlElement("description")]
		public string Description
		{
			get { return _description; }
			set { _description = value; }
		}

		[XmlElement("class")]
		public string Class
		{
			get { return _class; }
			set { _class = value; }
		}

		[XmlElement("assembly")]
		public string Assembly
		{
			get { return _assembly; }
			set { _assembly = value; }
		}

		[XmlArray("controls"), XmlArrayItem("control", typeof(ProviderControlInfo))]
		public ProviderControlInfo[] Controls;

		public string Path
		{
			get { return _path; }
			set { _path = value; }		
		}

		public string VirtualPath
		{
			get { return _virtualPath; }
			set { _virtualPath = value; }		
		}

		public StoreProviderType Type
		{
			get {return _type;}
			set {_type = value;}
		}

		#endregion
	}

	[XmlRoot("control")]
    [Serializable]
    public sealed class ProviderControlInfo
	{
		#region Private Members

		private string _name = string.Empty;
		private string _value = string.Empty;

		#endregion

		#region Properties

		[XmlAttribute("name")]
		public string Name
		{
			get { return _name; }
			set { _name = value; }
		}

		[XmlAttribute("value")]
		public string Value
		{
			get { return _value; }
			set { _value = value; }
		}

		#endregion
	}
}
