/*
'  DNN (formerly DotNetNuke) - http://www.dnnsoftware.com
'  Copyright (c) 2002-2016
'  by DNN Corp.
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

using System.Web;

namespace DotNetNuke.Modules.Store.Components
{
    /// <summary>
    /// Helper class used to verify application trust level.
    /// </summary>
    public sealed class TrustHelper
    {
        private static readonly AspNetHostingPermissionLevel _trustLevel;

        static TrustHelper()
        {
            foreach (AspNetHostingPermissionLevel trustLevel in
                    new AspNetHostingPermissionLevel[] {
                AspNetHostingPermissionLevel.Unrestricted,
                AspNetHostingPermissionLevel.High,
                AspNetHostingPermissionLevel.Medium,
                AspNetHostingPermissionLevel.Low,
                AspNetHostingPermissionLevel.Minimal,
                AspNetHostingPermissionLevel.None
            })
            {
                try
                {
                    new AspNetHostingPermission(trustLevel).Demand();
                }
                catch (System.Security.SecurityException)
                {
                    continue;
                }
                _trustLevel = trustLevel;
                break;
            }
        }

        public static AspNetHostingPermissionLevel TrustLevel
        {
            get { return _trustLevel; }
        }

        public static bool IsFullTrust
        {
            get { return (_trustLevel == AspNetHostingPermissionLevel.Unrestricted); }
        }

        public static bool IsHighTrust
        {
            get { return (_trustLevel == AspNetHostingPermissionLevel.High); }
        }

        public static bool IsMediumTrust
        {
            get { return (_trustLevel == AspNetHostingPermissionLevel.Medium); }
        }

        public static bool IsLowTrust
        {
            get { return (_trustLevel == AspNetHostingPermissionLevel.Low); }
        }

        public static bool IsMinimalTrust
        {
            get { return (_trustLevel == AspNetHostingPermissionLevel.Minimal); }
        }

        public static bool IsNoneTrust
        {
            get { return (_trustLevel == AspNetHostingPermissionLevel.None); }
        }
    }
}
