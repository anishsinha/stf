using SiteFuel.Exchange.Logger;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace SiteFuel.Exchange.Utilities
{
    public class CryptoHelperMethods
    {
        public static string GenerateSalt()
        {
            using (var randomNumberGenerator = new RNGCryptoServiceProvider())
            {
                var randomNumber = new byte[32];
                randomNumberGenerator.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public static string GenerateHash(string toBeHashed, string salt, int iterations = 5000)
        {
            using (var rfc2898DeriveBytes = new Rfc2898DeriveBytes(
                                                    toBeHashed,
                                                    Convert.FromBase64String(salt),
                                                    iterations))
            {
                var hashResult = rfc2898DeriveBytes.GetBytes(32);
                return Convert.ToBase64String(hashResult);
            }
        }

        public static string GeneratePassword(int length, string staticPassword)
        {
            bool isStaticPassword = SiteFuel.Exchange.Core.Utilities.AppSettings.GenerateStaticPassword;
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890@#$%&";
            StringBuilder res = new StringBuilder();
            if (!isStaticPassword)
            {
                Random rnd = new Random();
                while (0 < length--)
                {
                    res.Append(valid[rnd.Next(valid.Length)]);
                }
            }
            else
            {
                res.Append(staticPassword);
            }
            return res.ToString();
        }
        
        /// <summary>
        /// To generate random password
        /// </summary>
        /// <param name="lengthOfpassword">To create lenght of password</param>
        /// <returns></returns>
        public static string GenerateRandomPassword(int lengthOfpassword = 8)
        {
            // Create a string of characters, numbers, special characters that allowed in the password  
            string validChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            Random random = new Random();

            // Select one random character at a time from the string  
            // and create an array of chars  
            char[] chars = new char[lengthOfpassword];
            for (int i = 0; i < lengthOfpassword; i++)
            {
                chars[i] = validChars[random.Next(0, validChars.Length)];
            }
            return new string(chars);
        }
       /// <summary>
       /// To encrypt given string with key
       /// </summary>
       /// <param name="key"></param>
       /// <param name="password"></param>
       /// <returns></returns>
        public static string EncryptPassword(string key, string password)
        {
            byte[] iv = new byte[16];
            byte[] array=null;
            try
            {

                using (Aes aes = Aes.Create())
                {
                    aes.Key = Encoding.UTF8.GetBytes(key);
                    aes.IV = iv;

                    ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                        {
                            using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                            {
                                streamWriter.Write(password);
                            }

                            array = memoryStream.ToArray();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("CryptoHelperMethods", "EncryptPassword", ex.Message, ex);
            }

            return Convert.ToBase64String(array);
        }
        /// <summary>
        /// To decrypt string which already encypt using same algorithm
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="password">password</param>
        /// <returns></returns>
        public static string DecryptPassword(string key, string password)
        {
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(password);
            string result = string.Empty;
            try
            {
                using (Aes aes = Aes.Create())
                {
                    aes.Key = Encoding.UTF8.GetBytes(key);
                    aes.IV = iv;
                    ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                    using (MemoryStream memoryStream = new MemoryStream(buffer))
                    {
                        using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                            {
                                result= streamReader.ReadToEnd();
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                LogManager.Logger.WriteException("CryptoHelperMethods", "DecryptPassword", ex.Message, ex);
            }
            return result;
        }
    }
}
