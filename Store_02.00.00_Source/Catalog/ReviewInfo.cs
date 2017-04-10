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

namespace DotNetNuke.Modules.Store.Catalog
{
	/// <summary>
	/// Review class info.
	/// </summary>
    [Serializable]
    public sealed class ReviewInfo : IHydratable, IEquatable<ReviewInfo>
	{
		#region Private Members

		private int _reviewID;
		private int _portalID;
		private int _productID;
		private int _rating;
		private string _userName = string.Empty;
		private string _comments = string.Empty;
		private bool _authorized;
		private DateTime _createdDate;
        private string _modelName;

		#endregion

		#region Properties

		public int ReviewID 
		{
			get { return _reviewID; }
			set { _reviewID = value; }
		}

		public int PortalID 
		{
			get { return _portalID; }
			set { _portalID = value; }
		}

		public int ProductID 
		{
			get { return _productID; }
			set { _productID = value; }
		}

		public int Rating 
		{
			get { return _rating; }
			set { _rating = value; }
		}

		public string Comments 
		{
			// Need to replace any double apostrophe's because the PortalSecurity.InputFilter 
			// replaces the single apostrophe's when using the PortalSecurity.FilterFlag.NoSQL flag.
			// It is necessary here because the non-admin users have edit rights to this field.
			get { return _comments.Replace("''","'"); }
			set { _comments = value; }
		}

		public bool Authorized 
		{
			get { return _authorized; }
			set { _authorized = value; }
		}

		public string UserName 
		{
			get
			{
                // Need to replace any double apostrophe's because the PortalSecurity.InputFilter 
                // replaces the single apostrophe's when using the PortalSecurity.FilterFlag.NoSQL flag.
                // It is necessary here because the non-admin users have edit rights to this field.
                return _userName.Replace("''", "'");
			}
			set { _userName = value; }
		}

		public DateTime CreatedDate 
		{
			get { return _createdDate; }
			set { _createdDate = value; }
		}

        public string ModelName
        {
            get { return _modelName; }
            set { _modelName = value; }
        }

		#endregion

		#region Object Overrides

		public override int GetHashCode() 
		{
			return _reviewID.GetHashCode();
		}

		#endregion

        #region IEquatable<ReviewInfo> Interface

        public bool Equals(ReviewInfo other)
        {
            if (other == null)
                return false;
            return _reviewID.Equals(other.ReviewID);
        }

        #endregion

        #region IHydratable Members

        public void Fill(System.Data.IDataReader dr)
        {
            _reviewID = Convert.ToInt32(dr["ReviewID"]);
            _portalID = Convert.ToInt32(dr["PortalID"]);
            _productID = Convert.ToInt32(dr["ProductID"]);
            _userName = Convert.ToString(Null.SetNull(dr["UserName"], _userName));
            _rating = Convert.ToInt32(dr["Rating"]);
            _comments = Convert.ToString(Null.SetNull(dr["Comments"], _comments));
            _authorized = Convert.ToBoolean(dr["Authorized"]);
            _createdDate = Convert.ToDateTime(Null.SetNull(dr["CreatedDate"], _createdDate));
            _modelName = Convert.ToString(Null.SetNull(dr["ModelName"], _modelName));
        }

        public int KeyID
        {
            get { return _reviewID; }
            set { _reviewID = value; }
        }

        #endregion
    }
}
