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
using DotNetNuke.Modules.Store.Components;

namespace DotNetNuke.Modules.Store.Cart
{
	/// <summary>
	/// Summary description for EmailAdmin.
	/// </summary>
	public partial class EmailAdmin : StoreControlBase
	{
		#region StoreControlBase Overrides

		public override object DataSource
		{
			get
			{
                EmailSettings settings = new EmailSettings();

                base.DataSource = settings.ToString();
				return base.DataSource;
			}
			set
			{
				base.DataSource = value;

                //if (base.DataSource != null)
                //{
                //    string gatewaySettings = base.DataSource as string;
                //    if (gatewaySettings != null)
                //    {
                //        EmailSettings settings = new EmailSettings(gatewaySettings);
                //    }
                //}
			}
		}

		#endregion
	}
}
