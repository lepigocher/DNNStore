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
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;

namespace DotNetNuke.Modules.Store.Customer
{
    /// <summary>
    /// Order status class info.
    /// </summary>
    [Serializable]
    public sealed class OrderStatus : IHydratable
    {
        #region Private Members

        private int _orderStatusID;
        private string _orderStatusText;
        private int _listOrder;

        #endregion

        #region Properties

        public int OrderStatusID 
        {
            get { return _orderStatusID; }
            set { _orderStatusID = value; }
        }

        public string OrderStatusText
        {
            get { return _orderStatusText; }
            set { _orderStatusText = value; }
        }

        public int ListOrder 
        {
            get { return _listOrder; }
            set { _listOrder = value; }
        }

        #endregion

        #region IHydratable Members

        public void Fill(System.Data.IDataReader dr)
        {
            _orderStatusID = Convert.ToInt32(dr["OrderStatusID"]);
            _orderStatusText = Convert.ToString(Null.SetNull(dr["OrderStatusText"], _orderStatusText));
            _listOrder = Convert.ToInt32(dr["ListOrder"]);
        }

        public int KeyID
        {
            get { return _orderStatusID; }
            set { _orderStatusID = value; }
        }

        #endregion
    }
}
