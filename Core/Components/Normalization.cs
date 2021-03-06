﻿/*
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

using System.Globalization;
using System.Text;

namespace DotNetNuke.Modules.Store.Core.Components
{
    public static class Normalization
    {
        /// <summary>
        /// Remove diacritics (accents, cedilla, ...) from the specified string.
        /// </summary>
        /// <param name="text">Text with diacritics</param>
        /// <returns>Text without diacritics</returns>
        public static string RemoveDiacritics(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                int charIndex = 0;
                char[] chars = new char[text.Length];

                text = text.Normalize(NormalizationForm.FormD);
                for (int i = 0; i < text.Length; i++)
                {
                    char c = text[i];
                    if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                        chars[charIndex++] = c;
                }
                return new string(chars, 0, charIndex).Normalize(NormalizationForm.FormC);
            }
            return text;
        }

        /// <summary>
        /// Remove diacritics (accents, cedilla, ...) from the specified string and
        /// replace spaces and punctations by the specified character.
        /// Can be used to normalize URL parameters, but not a full URL!
        /// </summary>
        /// <param name="text">Text with diacritics</param>
        /// <param name="replacement">Replacement character, should be - or _ for URL</param>
        /// <returns>Text without diacritics</returns>
        public static string RemoveDiacritics(string text, char replacement)
        {
            if (!string.IsNullOrEmpty(text))
            {
                // Defined as true by default to avoid string like "!important" converted to "-important"
                bool previousIsReplacement = true;
                int charIndex = -1;
                char[] chars = new char[text.Length];

                text = text.Normalize(NormalizationForm.FormD);
                for (int i = 0; i < text.Length; i++)
                {
                    char c = text[i];
                    switch (CharUnicodeInfo.GetUnicodeCategory(c))
                    {
                        case UnicodeCategory.LowercaseLetter:
                        case UnicodeCategory.UppercaseLetter:
                        case UnicodeCategory.DecimalDigitNumber:
                            previousIsReplacement = false;
                            chars[++charIndex] = c;
                            break;
                        case UnicodeCategory.SpaceSeparator:
                        case UnicodeCategory.ConnectorPunctuation:
                        case UnicodeCategory.DashPunctuation:
                        case UnicodeCategory.OtherPunctuation:
                            if (!previousIsReplacement)
                            {
                                previousIsReplacement = true;
                                chars[++charIndex] = replacement;
                            }
                            break;
                    }
                }
                // Remove last space replacement char, if any!
                if (charIndex > -1 && chars[charIndex] == replacement)
                    charIndex--;

                return new string(chars, 0, ++charIndex).Normalize(NormalizationForm.FormC);
            }
            return text;
        }
    }
}
