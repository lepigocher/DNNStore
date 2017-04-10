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

namespace DotNetNuke.Modules.Store.Providers.Tax.DefaultTaxProvider
{
	/// <summary>
	/// Summary description for TaxInfo.
	/// </summary>
    [Serializable]
    public sealed class TaxInfo : ITaxInfo, IHydratable, IEquatable<TaxInfo>
    {
		#region ITaxInfo Members

	    public decimal DefaultTaxRate { get; set; }

	    public bool ShowTax { get; set; }

	    public decimal ShippingTax { get; set; }

	    public decimal SalesTax { get; set; }

	    #endregion

        #region IHydratable Membres

        public void Fill(System.Data.IDataReader dr)
        {
            KeyID = Convert.ToInt32(dr["PortalID"]);
            DefaultTaxRate = Convert.ToDecimal(Null.SetNull(dr["DefaultTaxRate"], DefaultTaxRate));
            ShowTax = Convert.ToBoolean(dr["ShowTax"]);
        }

        public int KeyID { get; set; }

        #endregion

        #region IEquatable<TaxInfo> Interface

        public bool Equals(TaxInfo other)
        {
            if (other == null)
                return false;
            return (ShippingTax.Equals(other.ShippingTax) && SalesTax.Equals(other.SalesTax));
        }

        #endregion
    }
}
