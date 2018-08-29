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
using System.Xml.Serialization;

namespace DotNetNuke.Modules.Store.Providers.Tax.CountryTaxProvider
{
    [Serializable, XmlType("CountryTaxRate")]
    public sealed class CountryTaxInfo : IComparable<CountryTaxInfo>
    {
        #region Constructor

        public CountryTaxInfo()
        {
            CountryCode = string.Empty;
            RegionCode = string.Empty;
            ZipCode = string.Empty;
        }

        #endregion

        #region Properties

        [XmlAttribute]
        public string CountryCode { get; set; }
        [XmlAttribute]
        public string RegionCode { get; set; }
        [XmlAttribute]
        public string ZipCode { get; set; }
        [XmlAttribute]
        public decimal TaxRate { get; set; }

        #endregion

        #region Static Members

        private static int WildcardCompare(string countryRegion1, string zip1, string countryRegion2, string zip2)
        {
            int crComp = countryRegion1.CompareTo(countryRegion2);

            if (crComp == 0)
            {
                bool zip1IsNull = string.IsNullOrEmpty(zip1);
                bool zip2IsNull = string.IsNullOrEmpty(zip2);

                if (zip1IsNull && zip2IsNull)
                    return 0;

                if (zip1IsNull)
                    return -1;

                if (zip2IsNull)
                    return 1;

                int posWildcardS1 = zip1.IndexOf('*');
                if (posWildcardS1 > 0)
                    zip1 = zip1.Substring(0, posWildcardS1);

                int posWildcardS2 = zip2.IndexOf('*');
                if (posWildcardS2 > 0)
                    zip2 = zip2.Substring(0, posWildcardS2);

                if (zip1.Length < zip2.Length && zip2.StartsWith(zip1))
                    return 1;
                if (zip2.Length < zip1.Length && zip1.StartsWith(zip2))
                    return -1;

                crComp = zip1.CompareTo(zip2) * -1;
            }

            return crComp;
        }

        #endregion

        #region IComparable<CountryTaxInfo> Members

        public int CompareTo(CountryTaxInfo other)
        {
            return WildcardCompare(CountryCode + RegionCode, ZipCode, other.CountryCode + other.RegionCode, other.ZipCode);
        }

        #endregion
    }
}