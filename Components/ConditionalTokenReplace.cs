using System.Collections;
using System.Data;
using System.Threading;
using System.Web;

using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Host;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Entities.Users;
using DotNetNuke.Services.Tokens;

namespace DotNetNuke.Modules.Store.Components
{
    public class ConditionalTokenReplace : BaseCustomConditionalTokenReplace
    {
        #region Private Members

        private int _moduleId = Null.NullInteger;
        private Hashtable _hostSettings;
        private ModuleInfo _moduleInfo;

        #endregion

        #region Properties

        private Hashtable HostSettings
        {
            get
            {
                if (_hostSettings == null)
                    _hostSettings = DotNetNuke.Entities.Host.HostSettings.GetSecureHostSettings();

                return _hostSettings;
            }
        }

        public int ModuleId
        {
            get { return _moduleId; }
            set { _moduleId = value; }
        }

        public ModuleInfo ModuleInfo
        {
            get
            {
                if (ModuleId != Null.NullInteger && (_moduleInfo == null || _moduleInfo.ModuleID != ModuleId))
                {
                    ModuleController controller = new ModuleController();
                    _moduleInfo = PortalSettings == null || PortalSettings.ActiveTab == null ? controller.GetModule(ModuleId, Null.NullInteger, true) : controller.GetModule(ModuleId, PortalSettings.ActiveTab.TabID, false);
                }
                return _moduleInfo;
            }
            set
            {
                _moduleInfo = value;
            }
        }

        public PortalSettings PortalSettings { get; set; }

        public UserInfo User { get; set; }

        #endregion

        #region Constructors

        public ConditionalTokenReplace() : this(Scope.DefaultSettings, null, null, null, Null.NullInteger)
        { }

        public ConditionalTokenReplace(int moduleID) : this(Scope.DefaultSettings, null, null, null, moduleID)
        { }

        public ConditionalTokenReplace(Scope accessLevel) : this(accessLevel, null, null, null, Null.NullInteger)
        { }

        public ConditionalTokenReplace(Scope accessLevel, int moduleID) : this(accessLevel, null, null, null, moduleID)
        { }

        public ConditionalTokenReplace(Scope accessLevel, string language, PortalSettings portalSettings, UserInfo user)
            : this(accessLevel, language, portalSettings, user, Null.NullInteger)
        { }

        public ConditionalTokenReplace(Scope accessLevel, string language, PortalSettings portalSettings, UserInfo user, int moduleID)
        {
            CurrentAccessLevel = accessLevel;

            if (accessLevel != Scope.NoSettings)
            {
                if (portalSettings == null)
                {
                    if (HttpContext.Current != null)
                        PortalSettings = PortalController.GetCurrentPortalSettings();
                }
                else
                    PortalSettings = portalSettings;

                if (user == null)
                {
                    User = HttpContext.Current == null ? new UserInfo() : (UserInfo)HttpContext.Current.Items["UserInfo"];
                    AccessingUser = User;
                }
                else
                {
                    User = user;
                    if (HttpContext.Current != null)
                        AccessingUser = (UserInfo)HttpContext.Current.Items["UserInfo"];
                    else
                        AccessingUser = new UserInfo();
                }

                if (string.IsNullOrEmpty(language))
                    Language = Thread.CurrentThread.CurrentUICulture.ToString();
                else
                    Language = language;

                if (moduleID != Null.NullInteger)
                    ModuleId = moduleID;
            }

            PropertySource["date"] = new DateTimePropertyAccess();
            PropertySource["datetime"] = new DateTimePropertyAccess();
            PropertySource["ticks"] = new TicksPropertyAccess();
            PropertySource["culture"] = new CulturePropertyAccess();
        }

        #endregion

        #region Public Methods

        public string ReplaceEnvironmentTokens(string sourceText)
        {
            return ReplaceTokens(sourceText);
        }

        public string ReplaceEnvironmentTokens(string sourceText, DataRow row)
        {
            DataRowPropertyAccess rowPropertyAccess = new DataRowPropertyAccess(row);

            PropertySource["field"] = rowPropertyAccess;
            PropertySource["row"] = rowPropertyAccess;

            return ReplaceTokens(sourceText);
        }

        public string ReplaceEnvironmentTokens(string sourceText, ArrayList custom, string customCaption)
        {
            PropertySource[customCaption.ToLower()] = new ArrayListPropertyAccess(custom);

            return ReplaceTokens(sourceText);
        }

        public string ReplaceEnvironmentTokens(string sourceText, IDictionary custom, string customCaption)
        {
            PropertySource[customCaption.ToLower()] = new DictionaryPropertyAccess(custom);

            return ReplaceTokens(sourceText);
        }

        public string ReplaceEnvironmentTokens(string sourceText, IDictionary custom, string[] customCaptions)
        {
            foreach (string caption in customCaptions)
                PropertySource[caption.ToLower()] = new DictionaryPropertyAccess(custom);

            return ReplaceTokens(sourceText);
        }

        public string ReplaceEnvironmentTokens(string sourceText, ArrayList custom, string customCaption, DataRow row)
        {
            DataRowPropertyAccess rowPropertyAccess = new DataRowPropertyAccess(row);

            PropertySource["field"] = rowPropertyAccess;
            PropertySource["row"] = rowPropertyAccess;
            PropertySource[customCaption.ToLower()] = new ArrayListPropertyAccess(custom);

            return ReplaceTokens(sourceText);
        }

        #endregion

        #region Private Members

        protected override string ReplaceTokens(string sourceText)
        {
            InitializePropertySources();

            return base.ReplaceTokens(sourceText);
        }

        private void InitializePropertySources()
        {
            PropertySource.Remove("portal");
            PropertySource.Remove("tab");
            PropertySource.Remove("user");
            PropertySource.Remove("membership");
            PropertySource.Remove("profile");
            PropertySource.Remove("host");
            PropertySource.Remove("module");

            if (CurrentAccessLevel >= Scope.Configuration)
            {
                if (PortalSettings != null)
                {
                    PropertySource["portal"] = PortalSettings;
                    PropertySource["tab"] = PortalSettings.ActiveTab;
                }

                PropertySource["host"] = new HostPropertyAccess();

                if (ModuleInfo != null)
                    PropertySource["module"] = ModuleInfo;
            }

            if (CurrentAccessLevel < Scope.DefaultSettings || User == null || User.UserID == -1)
                return;

            PropertySource["user"] = User;
            PropertySource["membership"] = new MembershipPropertyAccess(User);
            PropertySource["profile"] = new ProfilePropertyAccess(User);
        }

        #endregion
    }
}
