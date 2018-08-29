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

using System.Reflection;
using System.Runtime.CompilerServices;

using DotNetNuke.Common.Utilities;

using DotNetNuke.Modules.Store.Core.Components;

namespace DotNetNuke.Modules.Store.Core.Cart
{
    /// <summary>
    /// Module settings for cart modules.
    /// </summary>
    public sealed class ModuleSettings
    {
        #region Public Members

        public MiniCartSettings MiniCart;
		public MainCartSettings MainCart;

        #endregion

        #region Public Methods

        public ModuleSettings(int moduleId, int tabId)
		{
            string cacheKey = string.Format("_Mid{0}_Tid{1}", moduleId, tabId);

            MiniCart = (MiniCartSettings)DataCache.GetCache("StoreMiniCartSettings" + cacheKey);
            if (MiniCart == null)
            {
                MiniCart = new MiniCartSettings(moduleId, tabId);
                DataCache.SetCache("StoreMiniCartSettings" + cacheKey, MiniCart);
            }
            MainCart = (MainCartSettings)DataCache.GetCache("StoreMainCartSettings" + cacheKey);
            if (MainCart == null)
            {
			    MainCart = new MainCartSettings(moduleId, tabId);
                DataCache.SetCache("StoreMainCartSettings" + cacheKey, MainCart);
            }
		}

        public void UpdateCache(int moduleId, int tabId)
        {
            string cacheKey = string.Format("_Mid{0}_Tid{1}", moduleId, tabId);

            DataCache.SetCache("StoreMiniCartSettings" + cacheKey, MiniCart);
            DataCache.SetCache("StoreMainCartSettings" + cacheKey, MainCart);
        }

        #endregion
    }

	#region Mini Cart Settings

    public sealed class MiniCartSettings : SettingsWrapper
    {
        #region Constructor

        public MiniCartSettings(int moduleId, int tabId) : base(moduleId, tabId)
		{
		}

        #endregion

        #region Properties

        [ModuleSetting("minicartshowthumbnail", "false")]
        public bool ShowThumbnail
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                return bool.Parse(GetSetting(m));
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            set
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                SetSetting(m, value.ToString());
            }
        }

        [ModuleSetting("minicartthumbnailwidth", "50")]
        public int ThumbnailWidth
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                return int.Parse(GetSetting(m));
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            set
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                SetSetting(m, value.ToString());
            }
        }

        [ModuleSetting("minicartgifbgcolor", "FFF")]
        public string GIFBgColor
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                return GetSetting(m);
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            set
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                SetSetting(m, value);
            }
        }

        [ModuleSetting("minicartenableimagecaching", "true")]
        public bool EnableImageCaching
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                return bool.Parse(GetSetting(m));
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            set
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                SetSetting(m, value.ToString());
            }
        }

        [ModuleSetting("minicartcacheimageduration", "20")]
        public int CacheImageDuration
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                return int.Parse(GetSetting(m));
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            set
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                SetSetting(m, value.ToString());
            }
        }

        [ModuleSetting("productcolumn", "modelnumber")]
        public string ProductColumn
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                return GetSetting(m);
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            set
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                SetSetting(m, value);
            }
        }

        [ModuleSetting("linktodetail", "false")]
        public bool LinkToDetail
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                return bool.Parse(GetSetting(m));
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            set
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                SetSetting(m, value.ToString());
            }
        }

        [ModuleSetting("includevat", "false")]
        public bool IncludeVAT
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                return bool.Parse(GetSetting(m));
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            set
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                SetSetting(m, value.ToString());
            }
        }

        #endregion
    }
    #endregion

    #region Main Cart Settings Settings

    public sealed class MainCartSettings : SettingsWrapper
	{
        #region Constructor

        public MainCartSettings(int moduleId, int tabId) : base(moduleId, tabId)
		{
        }

        #endregion

        #region Properties

        [ModuleSetting("defaultview", "CustomerCart")]
        public string DefaultView
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                return GetSetting(m);
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            set
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                SetSetting(m, value);
            }
        }

        [ModuleSetting("requiressl", "false")]
        public bool RequireSSL
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                return bool.Parse(GetSetting(m));
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            set
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                SetSetting(m, value.ToString());
            }
        }

        [ModuleSetting("maincartshowthumbnail", "false")]
        public bool ShowThumbnail
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                return bool.Parse(GetSetting(m));
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            set
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                SetSetting(m, value.ToString());
            }
        }

        [ModuleSetting("maincartthumbnailwidth", "100")]
        public int ThumbnailWidth
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                return int.Parse(GetSetting(m));
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            set
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                SetSetting(m, value.ToString());
            }
        }

        [ModuleSetting("maincartgifbgcolor", "FFF")]
        public string GIFBgColor
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                return GetSetting(m);
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            set
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                SetSetting(m, value);
            }
        }

        [ModuleSetting("maincartenableimagecaching", "true")]
        public bool EnableImageCaching
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                return bool.Parse(GetSetting(m));
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            set
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                SetSetting(m, value.ToString());
            }
        }

        [ModuleSetting("maincartcacheimageduration", "20")]
        public int CacheImageDuration
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                return int.Parse(GetSetting(m));
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            set
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                SetSetting(m, value.ToString());
            }
        }

        [ModuleSetting("productcolumn", "producttitle")]
        public string ProductColumn
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                return GetSetting(m);
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            set
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                SetSetting(m, value);
            }
        }

        [ModuleSetting("linktodetail", "false")]
        public bool LinkToDetail
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                return bool.Parse(GetSetting(m));
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            set
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                SetSetting(m, value.ToString());
            }
        }

        [ModuleSetting("includevat", "false")]
        public bool IncludeVAT
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                return bool.Parse(GetSetting(m));
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            set
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                SetSetting(m, value.ToString());
            }
        }

        #endregion
    }

    #endregion
}
