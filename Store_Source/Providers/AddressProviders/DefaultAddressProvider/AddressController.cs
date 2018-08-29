/*
'  DNN (formerly DotNetNuke) - http://www.dnnsoftware.com
'  Copyright (c) 2002-2018
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

using System.Collections.Generic;
using System.Data;

using DotNetNuke.Common.Utilities;
using DotNetNuke.Data;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Users;

using DotNetNuke.Modules.Store.Core.Components;
using DotNetNuke.Modules.Store.Core.Providers;
using DotNetNuke.Modules.Store.Core.Providers.Address;

namespace DotNetNuke.Modules.Store.Providers.Address.DefaultAddressProvider
{
	/// <summary>
	/// Summary description for AddresssController.
	/// </summary>
    public sealed class AddressController : ProviderControllerBase, IAddressProvider
    {
        #region Public Methods

        public AddressSettings GetAddressSettings(int portalID)
        {
            AddressSettings addressSettings = (AddressSettings)DataCache.GetCache("StoreDefaultAddressSettings" + portalID);
            if (addressSettings == null)
            {
                using (IDataReader reader = DataProvider.Instance().ExecuteReader("Store_Administration_GetAddressSettings", portalID))
                {
                    if (reader != null)
                    {
                        if (reader.Read())
                        {
                            string settings = "";
                            settings = (string)Null.SetNull(reader["DefaultAddressSettings"], settings);
                            if (!string.IsNullOrEmpty(settings))
                            {
                                addressSettings = (AddressSettings) ProviderSettingsHelper.DeserializeSettings(settings, typeof (AddressSettings));
                                DataCache.SetCache("StoreDefaultAddressSettings", addressSettings);
                            }
                        }
                        reader.Close();
                    }
                }
                if (addressSettings == null)
                    addressSettings= new AddressSettings();
            }
            return addressSettings;
        }

        public void UpdateAddressSettings(int portalID, AddressSettings addressSettings)
        {
            string settings = ProviderSettingsHelper.SerializeSettings(addressSettings, typeof (AddressSettings));
            DataProvider.Instance().ExecuteNonQuery("Store_Administration_UpdateAddressSettings", portalID, DataHelper.GetNull(settings));
            DataCache.SetCache("StoreDefaultAddressSettings", addressSettings);
        }

        #endregion

        #region IAddressProvider Members

        public int AddAddress(IAddressInfo address)
		{
            return DataProvider.Instance().ExecuteScalar<int>("Store_Addresses_AddAddress",
                address.UserID,
                address.PortalID,
                address.Description,
                DataHelper.GetNull(address.FirstName),
                DataHelper.GetNull(address.LastName),
                DataHelper.GetNull(address.Address1),
                DataHelper.GetNull(address.Address2),
                DataHelper.GetNull(address.City),
                DataHelper.GetNull(address.RegionCode),
                DataHelper.GetNull(address.CountryCode),
                DataHelper.GetNull(address.PostalCode),
                DataHelper.GetNull(address.Email),
                DataHelper.GetNull(address.Phone1),
                DataHelper.GetNull(address.Phone2),
                address.PrimaryAddress,
                address.UserSaved,
                UserController.Instance.GetCurrentUserInfo().UserID);
		}

		public void UpdateAddress(IAddressInfo address)
		{
            DataProvider.Instance().ExecuteNonQuery("Store_Addresses_UpdateAddress",
                address.AddressID,
                address.UserID,
                address.Description,
                DataHelper.GetNull(address.FirstName),
                address.LastName,
                DataHelper.GetNull(address.Address1),
                DataHelper.GetNull(address.Address2),
                DataHelper.GetNull(address.City),
                DataHelper.GetNull(address.RegionCode),
                DataHelper.GetNull(address.CountryCode),
                DataHelper.GetNull(address.PostalCode),
                DataHelper.GetNull(address.Email),
                DataHelper.GetNull(address.Phone1),
                DataHelper.GetNull(address.Phone2),
                address.PrimaryAddress,
                address.UserSaved);
		}

        public List<T> GetAddresses<T>(int portalID, int userID, string addressDefaultDescription) where T : IAddressInfo
        {
            // Get user addresses
            List<T> addresses = CBO.FillCollection<T>(DataProvider.Instance().ExecuteReader("Store_Addresses_GetUserAddresses", userID));

            // Get user registration address
            T address = (T)GetRegistrationAddress(portalID, userID, addressDefaultDescription);
            addresses.Insert(0, address);

            return addresses;
        }

		public IAddressInfo GetAddress(int addressID)
		{
            // IMPORTANT: An address provider have to return a new adress inheriting from IAddressInfo
            // when the parameter addressID is equal to a null integer!
            // This is the only way for the code using the provider to get a new address object
            // because the code can't create an instance from outside the provider.
            if (addressID == Null.NullInteger)
                return new AddressInfo();

            return CBO.FillObject<AddressInfo>(DataProvider.Instance().ExecuteReader("Store_Addresses_GetAddress", addressID));
		}

        public IAddressInfo GetRegistrationAddress(int portalID, int userID, string addressDefaultDescription)
		{
			UserController controller = new UserController();
			UserInfo userInfo = controller.GetUser(portalID, userID);

            if (userInfo != null)
            {
                AddressInfo addressInfo = new AddressInfo
                {
                    AddressID = 0,
                    Description = addressDefaultDescription,
                    FirstName = userInfo.FirstName,
                    LastName = userInfo.LastName,
                    Address1 = userInfo.Profile.Street,
                    Address2 = userInfo.Profile.Unit,
                    City = userInfo.Profile.City,
                    RegionCode = userInfo.Profile.Region,
                    CountryCode = userInfo.Profile.Country,
                    PostalCode = userInfo.Profile.PostalCode,
                    Email = userInfo.Email,
                    Phone1 = userInfo.Profile.Telephone,
                    Phone2 = userInfo.Profile.Cell,
                    UserID = userInfo.UserID
                };

                return addressInfo;
            }

            return null;
		}

        public void UpdateRegistrationAddress(int portalID, IAddressInfo address)
        {
            UserController controller = new UserController();
            UserInfo userInfo = controller.GetUser(portalID, address.UserID);

            userInfo.FirstName = address.FirstName;
            userInfo.LastName = address.LastName;
            userInfo.Profile.Street = address.Address1;
            userInfo.Profile.Unit = address.Address2;
            userInfo.Profile.City = address.City;
            userInfo.Profile.Region = address.RegionCode;
            userInfo.Profile.Country = address.CountryCode;
            userInfo.Profile.PostalCode = address.PostalCode;
            userInfo.Email = address.Email;
            userInfo.Profile.Telephone = address.Phone1;
            userInfo.Profile.Cell = address.Phone2;

            UserController.UpdateUser(portalID, userInfo);
        }

		public void DeleteAddresses(int portalID, int userID)
		{
			DataProvider.Instance().ExecuteNonQuery("Store_Addresses_DeleteUserAddresses", userID);
		}

		public void DeleteAddress(int addressID)
		{
			DataProvider.Instance().ExecuteNonQuery("Store_Addresses_DeleteAddress", addressID);
		}

		public ProviderControlBase GetProfileControl(PortalModuleBase parentControl, string modulePath)
		{
			ProviderControlBase profileControl = LoadControl(parentControl, modulePath, "Profile");
			return profileControl;
		}

        public string TokenReplace(string tokenObjectName, string source, IAddressInfo address)
        {
            AddressTokenReplace tkAddress = new AddressTokenReplace(tokenObjectName, address);
            return tkAddress.ReplaceAddressTokens(source);
        }

		#endregion
	}
}
