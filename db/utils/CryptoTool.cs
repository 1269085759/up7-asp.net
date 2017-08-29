using System;
using System.Security.Cryptography;
using System.Text;

namespace up7.db.utils
{
    public class CryptoTool
    {
        string key = "2C4ED1CC9BAA42A9";
        string iv  = "2C4ED1CC9BAA42A9";

        /// <summary>
        /// aes-cbc-解密
        /// </summary>
        /// <param name="toDecrypt"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        public string cbc_decode(string toDecrypt)
        {
            byte[] keyArray       = UTF8Encoding.UTF8.GetBytes(key);
            byte[] ivArray        = UTF8Encoding.UTF8.GetBytes(iv);
            byte[] toEncryptArray = Convert.FromBase64String(toDecrypt);

            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key             = keyArray;
            rDel.IV              = ivArray;
            rDel.Mode            = CipherMode.CBC;
            rDel.Padding         = PaddingMode.Zeros;

            ICryptoTransform cTransform = rDel.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return UTF8Encoding.UTF8.GetString(resultArray);
        }
    }
}