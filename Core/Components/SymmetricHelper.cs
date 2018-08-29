/*
'  DNN (formerly DotNetNuke) - http://www.dnnsoftware.com
'  Copyright (c) 2002-2018
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

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Xml;

namespace DotNetNuke.Modules.Store.Core.Components
{
    /// <summary>
    /// This helper class allow you to encrypt/decrypt confidential data using symmetric algorithm.
    /// 
    /// Two main static methods are available:
    /// 
    /// Encrypt - 5 signatures.
    /// Decrypt - 5 signatures. 
    /// 
    /// Please read member comments for details.
    /// 
    /// </summary>
    public sealed class SymmetricHelper
    {
        #region Constructor

        /// <summary>
        /// Static constructor called when the class is loaded first.
        /// </summary>
        static SymmetricHelper()
        {
            // Initialize static properties
            Initialize();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Specify if default encryption and decryption methods can be called.
        /// If false, the value submitted to Encrypt(string unicodeValue) and to
        /// Decrypt(string base64Value) methods will be returned without changes.
        /// 
        /// This property is set to false if the Initialize() methods can't
        /// read default encryption settings from the web.config file.
        /// 
        /// Some methods can be used safely even if this property is false.
        /// All methods using default settings can't be used safely.
        /// </summary>
        public static bool CanSafelyEncrypt { get; private set; }

        /// <summary>
        ///  Enum value corresponding to the defined symmetric algorithm in web.config
        /// </summary>
        public static SymmAlgorithm ConfigSymmAlgorithm { get; private set; }

        /// <summary>
        /// Symmetric key defined in web.config
        /// </summary>
        public static string ConfigSymmKey { get; private set; }

        /// <summary>
        /// Hash algorithm defined in web.config
        /// </summary>
        public static HashAlgorithm ConfigHashAlgorithm { get; private set; }

        /// <summary>
        /// Hash key defined in web.config.
        /// Begin of this key is used as IV by some methods.
        /// </summary>
        public static string ConfigHashKey { get; private set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Enum of available symmetric algorithms
        /// </summary>
        public enum SymmAlgorithm
        {
            AES,
            DES,
            RC2,
            Rijndael,
            TripleDES
        }

        /// <summary>
        /// Enum of available hash algorithms
        /// </summary>
        public enum HashAlgorithm
        {
            MD5,
            SHA,
            SHA1,
            SHA256,
            SHA384,
            SHA512
        }

        /// <summary>
        /// Return the required symmetric algorithm to encrypt/decrypt data.
        /// This method allow you to control each parameter of encryption.
        /// 
        /// You have to define your own Key and IV. By default the provider
        /// generate random key and IV. Provider methods GenerateKey and GenerateIV
        /// can be used to generate new values. Alternatively, the class static
        /// method GenerateKeyFromPassword can be used to generate key from password.
        /// 
        /// You should clear and dispose the provider after use.
        /// </summary>
        /// <param name="algorithm">Algorithm to create</param>
        /// <returns>An instance of the desired algorithm</returns>
        public static SymmetricAlgorithm GetProvider(SymmAlgorithm algorithm)
        {
            // Create a crypto service provider from the algorithm name
            string algName = Enum.GetName(typeof(SymmAlgorithm), algorithm);
            return SymmetricAlgorithm.Create(algName);
        }

        /// <summary>
        /// Convert an hex string to a byte array.
        /// </summary>
        /// <param name="hexString">Hex string to convert</param>
        /// <returns>A byte array of the specified string</returns>
        public static byte[] HexToByte(string hexString)
        {
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }

        /// <summary>
        /// Adjust a data array to the required size.
        /// Mainly used to create an IV from the hash key defined in web.config.
        /// Also used to create a key from the symmetric key defined in web.config
        /// with the default symmetric algorithm key size.
        /// </summary>
        /// <param name="data">Byte array to reduce</param>
        /// <param name="size">Reduced byte array</param>
        /// <returns></returns>
        public static byte[] AdjustToSize(byte[] data, int size)
        {
            int length = size / 8;
            if (data.Length > length)
            {
                byte[] adjusted = new byte[length];
                Array.Copy(data, adjusted, length);
                return adjusted;
            }

            return data;
        }

        /// <summary>
        /// Generate a key from a password with the specified
        /// algorithms, key size and IV.
        /// </summary>
        /// <param name="password">Password</param>
        /// <param name="algorithm">Symmetric algorithm</param>
        /// <param name="keySize">Symmetric key size</param>
        /// <param name="hash">Hash algorithm</param>
        /// <param name="iv">IV</param>
        /// <returns>Key</returns>
        public static byte[] GenerateKeyFromPassword(string password, SymmAlgorithm algorithm, int keySize, HashAlgorithm hash, byte[] iv)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException("password");

            string algName = Enum.GetName(typeof(SymmAlgorithm), algorithm);
            string hashName = Enum.GetName(typeof(HashAlgorithm), hash);
            // Generate a key from params
            PasswordDeriveBytes pdb = new PasswordDeriveBytes(password, null);
            byte[] key = pdb.CryptDeriveKey(algName, hashName, keySize, iv);
            return key;
        }

        /// <summary>
        /// Encrypt a string with default algorithm, key and iv
        /// defined in web.config.
        /// </summary>
        /// <param name="unicodeValue">Unicode string</param>
        /// <returns>Base64 encrypted string</returns>
        public static string Encrypt(string unicodeValue)
        {
            if (string.IsNullOrEmpty(unicodeValue))
                throw new ArgumentNullException("unicodeValue");

            if (!CanSafelyEncrypt)
                return unicodeValue;

            string encrypted = string.Empty;
            string algName = Enum.GetName(typeof(SymmAlgorithm), ConfigSymmAlgorithm);
            using (SymmetricAlgorithm provider = SymmetricAlgorithm.Create(algName))
            {
                if (provider != null)
                {
                    provider.Key = AdjustToSize(HexToByte(ConfigSymmKey), provider.KeySize);
                    provider.IV = AdjustToSize(HexToByte(ConfigHashKey), provider.BlockSize);
                    encrypted = EncryptTransform(provider, unicodeValue, false);
                }
            }
            return encrypted;
        }

        /// <summary>
        /// Decrypt a string with default algorithm, key and iv
        /// defined in web.config.
        /// </summary>
        /// <param name="base64Value">Base64 encrypted string</param>
        /// <returns>Decrypted unicode string</returns>
        public static string Decrypt(string base64Value)
        {
            if (string.IsNullOrEmpty(base64Value))
                throw new ArgumentNullException("base64Value");

            if (!CanSafelyEncrypt)
                return base64Value;

            string decrypted = string.Empty;
            string algName = Enum.GetName(typeof(SymmAlgorithm), ConfigSymmAlgorithm);
            using (SymmetricAlgorithm provider = SymmetricAlgorithm.Create(algName))
            {
                if (provider != null)
                {
                    provider.Key = AdjustToSize(HexToByte(ConfigSymmKey), provider.KeySize);
                    provider.IV = AdjustToSize(HexToByte(ConfigHashKey), provider.BlockSize);
                    byte[] buffer = Convert.FromBase64String(base64Value);
                    decrypted = DecryptTransform(provider, buffer);
                }
            }
            return decrypted;
        }

        /// <summary>
        /// Encrypt a string with the specified password.
        /// A key is generated from the password using random IV generated
        /// by the default algorithm defined in web.config.
        /// The IV is inserted at the beginning of the encrypted string.
        /// </summary>
        /// <param name="unicodeValue">Unicode string</param>
        /// <param name="password">Password</param>
        /// <returns>Base64 encrypted string</returns>
        public static string Encrypt(string unicodeValue, string password)
        {
            if (string.IsNullOrEmpty(unicodeValue))
                throw new ArgumentNullException("unicodeValue");

            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException("password");

            if (!CanSafelyEncrypt)
                return unicodeValue;

            return Encrypt(unicodeValue, password, ConfigSymmAlgorithm, ConfigHashAlgorithm);
        }

        /// <summary>
        /// Decrypt a string with the specified password.
        /// A key is generated from the password using
        /// the default algorithm defined in web.config.
        /// The IV is extracted from the beginning of the encrypted string.
        /// </summary>
        /// <param name="base64Value">Base64 encrypted string</param>
        /// <param name="password">Password</param>
        /// <returns>Decrypted unicode string</returns>
        public static string Decrypt(string base64Value, string password)
        {
            if (string.IsNullOrEmpty(base64Value))
                throw new ArgumentNullException("base64Value");

            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException("password");

            if (!CanSafelyEncrypt)
                return base64Value;

            return Decrypt(base64Value, password, ConfigSymmAlgorithm, ConfigHashAlgorithm);
        }

        /// <summary>
        /// Encrypt a string with the specified password, symmetric and hash algorithms.
        /// A key is generated from the password using random IV generated
        /// by the specified algorithms.
        /// The IV is inserted at the beginning of the encrypted string.
        /// </summary>
        /// <param name="unicodeValue">Unicode string</param>
        /// <param name="password">Password</param>
        /// <param name="algorithm">Symmetric algorithm</param>
        /// <param name="hash">Hash algorithm</param>
        /// <returns>Base64 encrypted string</returns>
        public static string Encrypt(string unicodeValue, string password, SymmAlgorithm algorithm, HashAlgorithm hash)
        {
            if (string.IsNullOrEmpty(unicodeValue))
                throw new ArgumentNullException("unicodeValue");

            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException("password");

            string encrypted = string.Empty;
            string algName = Enum.GetName(typeof(SymmAlgorithm), algorithm);
            using (SymmetricAlgorithm provider = SymmetricAlgorithm.Create(algName))
            {
                if (provider != null)
                {
                    provider.GenerateIV();
                    byte[] iv = provider.IV;
                    provider.Key = GenerateKeyFromPassword(password, algorithm, provider.KeySize, hash, iv);
                    encrypted = EncryptTransform(provider, unicodeValue, true);
                }
            }
            return encrypted;
        }

        /// <summary>
        /// Decrypt a string with the specified password, symmetric and hash algorithms.
        /// A key is generated from the password using the specified algorithms.
        /// The IV is extracted from the beginning of the encrypted string.
        /// </summary>
        /// <param name="base64Value">Base64 encrypted string</param>
        /// <param name="password">Password</param>
        /// <param name="algorithm">Symmetric algorithm</param>
        /// <param name="hash">Hash algorithm</param>
        /// <returns>Decrypted unicode string</returns>
        public static string Decrypt(string base64Value, string password, SymmAlgorithm algorithm, HashAlgorithm hash)
        {
            if (string.IsNullOrEmpty(base64Value))
                throw new ArgumentNullException("base64Value");

            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException("password");

            return Decrypt(base64Value, password, algorithm, hash, null, true);
        }

        /// <summary>
        /// Encrypt a string with the specified password, symmetric and hash algorithms and IV.
        /// A key is generated from the password using specified IV and algorithms.
        /// 
        /// The IV can be inserted at the beginning of the encrypted string. If you do not include IV,
        /// you have to share and/or store password and IV to be able to decrypt string later.
        /// </summary>
        /// <param name="unicodeValue">Unicode string</param>
        /// <param name="password">Password</param>
        /// <param name="algorithm">Symmetric algorithm</param>
        /// <param name="hash">Hash algorithm</param>
        /// <param name="iv">IV</param>
        /// <param name="includeIV">True to include IV in encrypted string</param>
        /// <returns>Base64 encrypted string</returns>
        public static string Encrypt(string unicodeValue, string password, SymmAlgorithm algorithm, HashAlgorithm hash, byte[] iv, bool includeIV)
        {
            if (string.IsNullOrEmpty(unicodeValue))
                throw new ArgumentNullException("unicodeValue");

            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException("password");

            string encrypted = string.Empty;
            string algName = Enum.GetName(typeof(SymmAlgorithm), algorithm);
            using (SymmetricAlgorithm provider = SymmetricAlgorithm.Create(algName))
            {
                if (provider != null)
                {
                    provider.Key = GenerateKeyFromPassword(password, algorithm, provider.KeySize, hash, iv);
                    provider.IV = iv;
                    encrypted = EncryptTransform(provider, unicodeValue, includeIV);
                }
            }
            return encrypted;
        }

        /// <summary>
        /// Decrypt a string with the specified password, symmetric and hash algorithms and IV.
        /// A key is generated from the password using specified algorithms and IV.
        /// 
        /// The IV can be extracted from the beginning of the encrypted string.
        /// Otherwise, you have to specify the same IV used to encrypt this data. 
        /// </summary>
        /// <param name="base64Value">Base64 encrypted string</param>
        /// <param name="password">Password</param>
        /// <param name="algorithm">Symmetric algorithm</param>
        /// <param name="hash">Hash algorithm</param>
        /// <param name="iv">IV</param>
        /// <param name="includeIV">True to extract IV from encrypted string</param>
        /// <returns>Decrypted unicode string</returns>
        public static string Decrypt(string base64Value, string password, SymmAlgorithm algorithm, HashAlgorithm hash, byte[] iv, bool includeIV)
        {
            if (string.IsNullOrEmpty(base64Value))
                throw new ArgumentNullException("base64Value");

            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException("password");

            if (iv == null || iv.Length == 0)
                throw new ArgumentNullException("iv");

            string decrypted = string.Empty;
            string algName = Enum.GetName(typeof(SymmAlgorithm), algorithm);
            using (SymmetricAlgorithm provider = SymmetricAlgorithm.Create(algName))
            {
                if (provider != null)
                {
                    byte[] buffer;
                    if (includeIV)
                    {
                        Extract(base64Value, out iv, provider.BlockSize / 8, out buffer);
                        provider.IV = iv;
                        provider.Key = GenerateKeyFromPassword(password, algorithm, provider.KeySize, hash, iv);
                    }
                    else
                    {
                        provider.IV = iv;
                        provider.Key = GenerateKeyFromPassword(password, algorithm, provider.KeySize, hash, iv);
                        buffer = Convert.FromBase64String(base64Value);
                    }
                    decrypted = DecryptTransform(provider, buffer);
                }
            }
            return decrypted;
        }

        /// <summary>
        /// Encrypt a string with the specified provider.
        /// You have to define the key and IV used to encrypt data using provider properties.
        /// The IV can be inserted at the beginning of the encrypted string.
        /// </summary>
        /// <param name="unicodeValue">Unicode string</param>
        /// <param name="provider">Symmetric provider</param>
        /// <param name="includeIV">True to include IV in encrypted string</param>
        /// <returns>Base64 encrypted string</returns>
        public static string Encrypt(string unicodeValue, SymmetricAlgorithm provider, bool includeIV)
        {
            if (string.IsNullOrEmpty(unicodeValue))
                throw new ArgumentNullException("unicodeValue");

            return EncryptTransform(provider, unicodeValue, includeIV);
        }

        /// <summary>
        /// Decrypt a string with the specified provider.
        /// You have to define the key and IV used to decrypt data using provider properties.
        /// The IV can be extracted from the encrypted string.
        /// </summary>
        /// <param name="base64Value">Base64 encrypted string</param>
        /// <param name="provider">Symmetric provider</param>
        /// <param name="includeIV">True to extract IV from encrypted string</param>
        /// <returns>Decrypted unicode string</returns>
        public static string Decrypt(string base64Value, SymmetricAlgorithm provider, bool includeIV)
        {
            if (string.IsNullOrEmpty(base64Value))
                throw new ArgumentNullException("base64Value");

            byte[] buffer;
            if (includeIV)
            {
                byte[] iv;
                Extract(base64Value, out iv, provider.BlockSize / 8, out buffer);
                provider.IV = iv;
            }
            else
                buffer = Convert.FromBase64String(base64Value);

            return DecryptTransform(provider, buffer);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Initialize properties with default machine key values specified in web.config
        /// </summary>
        internal static void Initialize()
        {
            CanSafelyEncrypt = false;
            string webConfigFile = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, "web.config");
            using (StreamReader webConfigReader = new StreamReader(webConfigFile))
            {
                XmlTextReader xmlReader = new XmlTextReader(webConfigReader);
                if (xmlReader.ReadToFollowing("system.web") && xmlReader.ReadToDescendant("machineKey"))
                {
                    string algName = xmlReader.GetAttribute("decryption");
                    if (string.IsNullOrEmpty(algName) == false)
                    {
                        if (algName == "3DES")
                            algName = "TripleDES";
                        string hashName = xmlReader.GetAttribute("validation");
                        if (string.IsNullOrEmpty(hashName) == false)
                        {
                            string decryptionKey = xmlReader.GetAttribute("decryptionKey");
                            string validationKey = xmlReader.GetAttribute("validationKey");
                            if (string.IsNullOrEmpty(decryptionKey) == false && string.IsNullOrEmpty(validationKey) == false)
                            {
                                // Set properties
                                ConfigSymmAlgorithm = (SymmAlgorithm)Enum.Parse(typeof(SymmAlgorithm), algName, true);
                                ConfigSymmKey = decryptionKey;
                                ConfigHashAlgorithm = (HashAlgorithm)Enum.Parse(typeof(HashAlgorithm), hashName, true);
                                ConfigHashKey = validationKey;
                                // Enable Safe Encryption
                                CanSafelyEncrypt = true;
                            }
                        }
                    }
                }
                xmlReader.Close();
            }
        }

        /// <summary>
        /// Extract IV and data buffer from the encrypted string.
        /// IV and data buffer are returned with corresponding byte array values.
        /// </summary>
        /// <param name="value">Base64 encrypted string</param>
        /// <param name="iv">IV</param>
        /// <param name="ivLength">IV length</param>
        /// <param name="buffer">Data buffer</param>
        internal static void Extract(string value, out byte[] iv, int ivLength, out byte[] buffer)
        {
            // Convert string
            byte[] bufferValue = Convert.FromBase64String(value);
            // Define recipients
            iv = new byte[ivLength];
            buffer = new byte[bufferValue.Length - ivLength];
            // Extract IV and encrypted data
            Array.Copy(bufferValue, 0, iv, 0, ivLength);
            Array.Copy(bufferValue, ivLength, buffer, 0, buffer.Length);
        }

        /// <summary>
        /// Encrypt a string with the specified provider.
        /// The IV can be inserted at the beginning of the encrypted string.
        /// </summary>
        /// <param name="provider">Symmetric provider</param>
        /// <param name="unicodeValue">Unicode string</param>
        /// <param name="includeIV">True to include IV in encrypted string</param>
        /// <returns>Base64 encrypted string</returns>
        internal static string EncryptTransform(SymmetricAlgorithm provider, string unicodeValue, bool includeIV)
        {
            string encrypted;
            using (MemoryStream ms = new MemoryStream())
            {
                try
                {
                    if (includeIV)
                        ms.Write(provider.IV, 0, provider.IV.Length);
                    using (CryptoStream cs = new CryptoStream(ms, provider.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        try
                        {
                            byte[] buffer = Encoding.Unicode.GetBytes(unicodeValue);
                            cs.Write(buffer, 0, buffer.Length);
                            cs.FlushFinalBlock();
                            encrypted = Convert.ToBase64String(ms.ToArray(), Base64FormattingOptions.None);
                        }
                        finally
                        {
                            cs.Close();
                            cs.Clear();
                        }
                    }
                }
                finally
                {
                    ms.Close();
                }
            }
            return encrypted;
        }

        /// <summary>
        /// Decrypt a byte array with the specified provider.
        /// </summary>
        /// <param name="provider">Symmetric provider</param>
        /// <param name="buffer">Encrypted buffer</param>
        /// <returns>Unicode string</returns>
        internal static string DecryptTransform(SymmetricAlgorithm provider, byte[] buffer)
        {
            string decrypted = buffer.ToString();
            using (MemoryStream ms = new MemoryStream())
            {
                try
                {
                    using (CryptoStream cs = new CryptoStream(ms, provider.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        try
                        {
                            cs.Write(buffer, 0, buffer.Length);
                            cs.FlushFinalBlock();
                            decrypted = Encoding.Unicode.GetString(ms.ToArray());
                        }
                        finally
                        {
                            cs.Close();
                            cs.Clear();
                        }
                    }
                }
                finally
                {
                    ms.Close();
                }
            }
            return decrypted;
        }

        #endregion
    }
}
