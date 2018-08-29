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

using System;
using System.Reflection;

using DotNetNuke.Entities.Modules;

namespace DotNetNuke.Modules.Store.Core.Components
{
	public enum SettingsWrapperType
	{
		Module,
		TabModule
	}

	/// <summary>
	/// Abstract class used to manage module settings.
	/// </summary>
	public abstract class SettingsWrapper
    {
        #region Private Members

        private readonly int _moduleID;
        private readonly int _tabModuleID;
        private readonly ModuleController _controller;
        private readonly SettingsWrapperType _wrapperType;

        #endregion

        #region Properties

        public int ModuleId
		{
			get { return _moduleID; }
		}

		public int TabModuleId
		{
			get { return _tabModuleID; }
		}

		public SettingsWrapperType WrapperType
		{
			get { return _wrapperType; }
		}

		#endregion

		#region Constructors

	    protected SettingsWrapper(int moduleID)
		{
			_moduleID = moduleID;
			_tabModuleID = 0;
			_wrapperType = SettingsWrapperType.Module;

			_controller = new ModuleController();
		}

	    protected SettingsWrapper(int moduleID, int tabID)
		{
			_moduleID = moduleID;
			_wrapperType = SettingsWrapperType.TabModule;

			_controller = new ModuleController();

			ModuleInfo moduleInfo = _controller.GetModule(moduleID, tabID);

			if (moduleInfo != null)
			{
				_tabModuleID = moduleInfo.TabModuleID;
			}
			else
			{
				throw new ApplicationException("ModuleID " + moduleID + " does not exist on TabID " + tabID + ".");
			}
		}

		#endregion

		#region Protected Methods

		protected string GetSetting(MethodBase mb)
		{
		    ModuleSettingAttribute settingInfo = null;
			object setting = null;

			PropertyInfo propertyInfo = mb.DeclaringType.GetProperty(mb.Name.Remove(0, 4), BindingFlags.Public | BindingFlags.Instance);
			
			if (propertyInfo != null)
				settingInfo = GetPropertyAttribute(propertyInfo);

			if (settingInfo != null)
			{
				switch (_wrapperType)
				{
					case SettingsWrapperType.Module:
                        setting = _controller.GetModule(_moduleID).ModuleSettings[settingInfo.Name];
						break;

					case SettingsWrapperType.TabModule:
                        setting = _controller.GetTabModule(_tabModuleID).TabModuleSettings[settingInfo.Name];
						break;
				}

				if(setting != null && setting.ToString() != "")
					return setting.ToString();

                return settingInfo.Default;
			}

            return null;
		}

		protected void SetSetting(MethodBase mb, string settingValue)
		{
		    ModuleSettingAttribute settingInfo = null;

			string propName = mb.Name;
			if (propName.StartsWith("set_"))
				propName = propName.Substring(4);

			PropertyInfo propertyInfo = mb.DeclaringType.GetProperty(propName, BindingFlags.Public | BindingFlags.Instance);
			
			if (propertyInfo != null)
				settingInfo = GetPropertyAttribute(propertyInfo);

			if (settingInfo != null)
			{
				if (settingValue == null)
					settingValue = string.Empty;

				switch (_wrapperType)
				{
					case SettingsWrapperType.Module:
						_controller.UpdateModuleSetting(_moduleID, settingInfo.Name, settingValue);
						break;

					case SettingsWrapperType.TabModule:
						_controller.UpdateTabModuleSetting(_tabModuleID, settingInfo.Name, settingValue);
						break;
				}
			}
		}

		#endregion

		#region Private Methods

		private static ModuleSettingAttribute GetPropertyAttribute(PropertyInfo propertyInfo)
		{
			object[] attributes;

			if (propertyInfo != null)
			{
				attributes = propertyInfo.GetCustomAttributes(Type.GetType("DotNetNuke.Modules.Store.Core.Components.ModuleSettingAttribute"), false);

				if (attributes.Length > 0)
					return (ModuleSettingAttribute)attributes[0];
			}

            return null;
		}

		#endregion
	}

	#region Custom Attribute

	[AttributeUsage(AttributeTargets.Property, AllowMultiple=false, Inherited=true)]
	public class ModuleSettingAttribute : Attribute
	{
		private readonly string _settingName;
		private readonly string _settingDefault;

        public string Name
        {
            get { return _settingName; }
        }

        public string Default
        {
            get { return _settingDefault; }
        }

		public ModuleSettingAttribute(string name, string defaultValue)
		{
			_settingName = name;
			_settingDefault = defaultValue;
		}
	}

	#endregion
}
