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
	/// Customer class info.
	/// </summary>
    [Serializable]
    public sealed class CustomerInfo : IHydratable, IEquatable<CustomerInfo>
	{
		#region Private Members

		private int _userID;
		private string _userName = string.Empty;
		private string _lastName = string.Empty;
		private string _firstName = string.Empty;

		#endregion

		#region Public Properties

		public int UserID
		{
			get { return _userID; }
			set { _userID = value; }
		}

		public string Username
		{
			get { return _userName; }
			set { _userName = value; }
		}

		public string LastName
		{
			get { return _lastName; }
			set { _lastName = value; }
		}

		public string FirstName
		{
			get { return _firstName; }
			set { _firstName = value; }
		}

		public string FullName
		{
			get
			{
				string fullName = string.Empty;
				if (_lastName != string.Empty)
				{
					fullName = _lastName;
					if (_firstName != string.Empty)
					{
						fullName += ", " + _firstName;
					}
				}
				return fullName;
			}
		}

		#endregion

        #region Object Overrides

        public override int GetHashCode()
        {
            return _userID.GetHashCode();
        }

        #endregion

        #region IHydratable Interface

        public void Fill(System.Data.IDataReader dr)
        {
            _userID = Convert.ToInt32(Null.SetNull(dr["UserID"], _userID));
            _userName = Convert.ToString(Null.SetNull(dr["Username"], _userName));
            _lastName = Convert.ToString(Null.SetNull(dr["LastName"], _lastName));
            _firstName = Convert.ToString(Null.SetNull(dr["FirstName"], _firstName));
        }

        public int KeyID
        {
            get { return _userID; }
            set { _userID = value; }
        }

        #endregion

        #region IEquatable<CustomerInfo> Interface

        public bool Equals(CustomerInfo other)
        {
            if (other == null)
                return false;
            return _userID.Equals(other.UserID);
        }

        #endregion
    }
}
