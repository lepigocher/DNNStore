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

namespace DotNetNuke.Modules.Store.Providers.Shipping.DefaultShippingProvider
{
	/// <summary>
	/// Summary description for AddressInfo.
	/// </summary>
    [Serializable]
    public sealed class ShippingInfo : IShippingInfo, IHydratable, IEquatable<ShippingInfo>
	{
		#region Private Members

		private int _id;
        private string _description;
        private decimal _minWeight;
        private decimal _maxWeight;
        private decimal _cost;
        private bool _applyTaxRate;
		
        #endregion

		#region Public Properties

        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public decimal MinWeight
        {
            get { return _minWeight; }
            set { _minWeight = value; }
        }

        public decimal MaxWeight
        {
            get { return _maxWeight; }
            set { _maxWeight = value; }
        }

        public decimal Cost
        {
            get { return _cost; }
            set { _cost = value; }
        }

        public bool ApplyTaxRate
        {
            get { return _applyTaxRate; }
            set { _applyTaxRate = value; }
        }

        #endregion

        #region Object Overrides

        public override int GetHashCode()
        {
            return _id.GetHashCode();
        }

        #endregion

        #region IHydratable Interface

        public void Fill(System.Data.IDataReader dr)
        {
            _id = Convert.ToInt32(Null.SetNull(dr["ID"], _id));
            _description = Convert.ToString(Null.SetNull(dr["Description"], _description));
            _minWeight = Convert.ToDecimal(Null.SetNull(dr["MinWeight"], _minWeight));
            _maxWeight = Convert.ToDecimal(Null.SetNull(dr["MaxWeight"], _maxWeight));
            _cost = Convert.ToDecimal(Null.SetNull(dr["Cost"], _cost));
            _applyTaxRate = Convert.ToBoolean(Null.SetNull(dr["ApplyTaxRate"], _applyTaxRate));
        }

        public int KeyID
        {
            get { return _id; }
            set { _id = value; }
        }

        #endregion

        #region IEquatable<ShippingInfo> Interface

        public bool Equals(ShippingInfo other)
        {
            if (other == null)
                return false;
            return (_minWeight.Equals(other.MinWeight) && _maxWeight.Equals(other.MaxWeight) && _cost.Equals(other.Cost));
        }

        #endregion
    }
}
