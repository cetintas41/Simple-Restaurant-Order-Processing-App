using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Services
{
    public static class Extensions
    {
        /// <summary>
        /// Verilen değeri encrypt eder.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Encrypt(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            byte[] clearBytes = Encoding.Unicode.GetBytes(value);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes("Portal2019", new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    value = Convert.ToBase64String(ms.ToArray());
                }
            }
            return value;
        }

        /// <summary>
        /// Verilen değeri decrypt eder.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Decrypt(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            value = value.Replace(" ", "+");
            value = value.Replace("-", "");
            byte[] cipherBytes = Convert.FromBase64String(value);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes("Portal2019", new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    value = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return value;
        }

        /// <summary>
        /// Checks the given date intervals are valid or not.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public static void CheckDates(ref DateTime start, ref DateTime end)
        {
            end = end == new DateTime(0001, 001, 01) ?
                new DateTime(start.Year, start.Month, start.Day, 23, 59, 59) : new DateTime(end.Year, end.Month, end.Day, 23, 59, 59);

            if (end < start)
            {
                var temp = end; end = start; start = temp;
            }
        }
    }
}
