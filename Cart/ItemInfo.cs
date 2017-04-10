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
using DotNetNuke.Modules.Store.Providers.Cart;

namespace DotNetNuke.Modules.Store.Cart
{
	/// <summary>
	/// Cart item class info.
	/// </summary>
    [Serializable]
    public sealed class ItemInfo : ICartItemInfo, IHydratable
	{
        #region Private Members

        private int _itemID;
		private string _cartID;
		private int _categoryID;
		private int _productID;
        private string _manufacturer;
        private string _modelNumber;
        private string _modelName;
	    private string _productTitle;
        private string _productImage;
        private decimal _unitCost;
		private int _quantity;
	    private DateTime _dateCreated;
        private decimal _productWeight;
        private decimal _productHeight;
        private decimal _productLength;
        private decimal _productWidth;

        #endregion

        #region Properties

        public int ItemID
		{
			get {return _itemID;}
			set {_itemID = value;}
		}

		public string CartID
		{
			get {return _cartID;}
			set {_cartID = value;}
		}

        public int CategoryID
		{
            get { return _categoryID; }
            set { _categoryID = value; }
		}

		public int ProductID
		{
			get {return _productID;}
			set {_productID = value;}
		}

        public string Manufacturer
        {
            get { return _manufacturer; }
            set { _manufacturer = value; }
        }

        public string ModelNumber
        {
            get { return _modelNumber; }
            set { _modelNumber = value; }
        }

        public string ModelName
        {
            get { return _modelName; }
            set { _modelName = value; }
        }

        public string ProductTitle
        {
            get { return _productTitle; }
        }

        public string ProductImage
        {
            get { return _productImage; }
            set { _productImage = value; }
        }

        public decimal UnitCost
        {
            get { return _unitCost; }
            set { _unitCost = value; }
        }

		public int Quantity
		{
			get {return _quantity;}
			set {_quantity = value;}
		}

        public decimal SubTotal
        {
            get { return _unitCost * _quantity; }
        }

	    public decimal Discount { get; set; }

	    public DateTime DateCreated
		{
			get {return _dateCreated;}
			set {_dateCreated = value;}
		}

        public decimal ProductWeight
        {
            get { return _productWeight; }
            set { _productWeight = value; }
        }

        public decimal ProductHeight
        {
            get { return _productHeight; }
            set { _productHeight = value; }
        }

        public decimal ProductLength
        {
            get { return _productLength; }
            set { _productLength = value; }
        }

        public decimal ProductWidth
        {
            get { return _productWidth; }
            set { _productWidth = value; }
        }

        #endregion

        #region IHydratable Members

        public void Fill(System.Data.IDataReader dr)
        {
            _itemID = Convert.ToInt32(dr["ItemID"]);
            _cartID = Convert.ToString(dr["CartID"]);
            _categoryID = Convert.ToInt32(dr["CategoryID"]);
            _productID = Convert.ToInt32(dr["ProductID"]);
            _manufacturer = Convert.ToString(Null.SetNull(dr["Manufacturer"], _manufacturer));
            _modelNumber = Convert.ToString(Null.SetNull(dr["ModelNumber"], _modelNumber));
            _modelName = Convert.ToString(Null.SetNull(dr["ModelName"], _modelName));
            _productTitle += _modelNumber.Trim();
            _productTitle += string.IsNullOrEmpty(_productTitle) ? _modelName.Trim() : " - " + _modelName.Trim();
            _productImage = Convert.ToString(Null.SetNull(dr["ProductImage"], _productImage));
            _unitCost = Convert.ToDecimal(dr["UnitCost"]);
            _quantity = Convert.ToInt32(dr["Quantity"]);
            _dateCreated = Convert.ToDateTime(dr["DateCreated"]);
            _productWeight = Convert.ToDecimal(dr["ProductWeight"]);
            _productHeight = Convert.ToDecimal(dr["ProductHeight"]);
            _productLength = Convert.ToDecimal(dr["ProductLength"]);
            _productWidth = Convert.ToDecimal(dr["ProductWidth"]);
       }

        public int KeyID
        {
            get { return _itemID; }
            set { _itemID = value; }
        }

        #endregion
    }
}
