using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace up7.debug
{
    public partial class test : System.Web.UI.Page
    {
        public String aes_ecb_encode(String str, string strAesKey)
        {
            Byte[] keyArray = new byte[32];
            keyArray = Encoding.UTF8.GetBytes(strAesKey);
            Byte[] toEncryptArray = Encoding.UTF8.GetBytes(str);

            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.None;

            ICryptoTransform cTransform = rDel.CreateEncryptor();
            Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
        public string AesDecrypt(string str, string key)
        {
            if (string.IsNullOrEmpty(str)) return null;
            Byte[] toEncryptArray = Convert.FromBase64String(str);

            RijndaelManaged rm = new RijndaelManaged
            {
                Key = Encoding.UTF8.GetBytes(key),
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7
            };

            ICryptoTransform cTransform = rm.CreateDecryptor();
            Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Encoding.UTF8.GetString(resultArray);
        }

        public string cbc_encode(string toEncrypt, string key, string iv)
        {
            byte[] keyArray = UTF8Encoding.UTF8.GetBytes(key);
            byte[] ivArray = UTF8Encoding.UTF8.GetBytes(iv);
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.IV = ivArray;
            rDel.Mode = CipherMode.CBC;
            rDel.Padding = PaddingMode.Zeros;

            ICryptoTransform cTransform = rDel.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        public string cbc_decode(string toDecrypt, string key, string iv)
        {
            byte[] keyArray = UTF8Encoding.UTF8.GetBytes(key);
            byte[] ivArray = UTF8Encoding.UTF8.GetBytes(iv);
            byte[] toEncryptArray = Convert.FromBase64String(toDecrypt);

            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.IV = ivArray;
            rDel.Mode = CipherMode.CBC;
            rDel.Padding = PaddingMode.Zeros;

            ICryptoTransform cTransform = rDel.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return UTF8Encoding.UTF8.GetString(resultArray);
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            string txt = "3n+VY2X2g57wE9gp2QHj+Q==";
            string key = "2C4ED1CC9BAA42A9";
            string iv  = "2C4ED1CC9BAA42A9";
            string str = "test";

            string str_p = this.cbc_encode(str, key, iv);
            Response.Write(str_p);
            Response.Write("<br/>");

            //string v = this.cbc_decode(txt, key, iv);
            //Response.Write(v);
            //var v = Encoding.UTF8.GetBytes("2C4ED1CC9BAA42A");
            ////Response.Write(this.AESDecrypt("E1D77384EB7DAC9771AD", "254A20FC4E854AEF922A13E784C2EE34", "1234"));
            //string txt_en = (this.aes_ecb_encode("test", "254A20FC4E854AEF922A13E784C2EE34"));
            //Response.Write("加密："+txt_en);
            //txt_en = "wc8+xAzdJXYP0Jx+X7Kcba5gci40S8x54ASwp+d7IYTt5hz8j89o0W1HYFvVYG2x7lvxcAx1v5mzy0+Yu/gC8ZGEJ7PSdUYeGb31gqepBwZOGcGjzaie3EwvUEUPZVi01yFAZ56716Dlsj9ZFitEDPnYy5kE2eoA/YUCX5PaaxIVhghMm5L+2qdDvpEpRU0Hcn+SKDUMaOwnVcdoHE9OFzuQkDp5B0ElF6JYc2E+FCLtpUBELl0iRhQ+Pzj0eRS/pJbsOXc5L0ktQyz07+Isaf5+ZRFCujnh8AN8Iprr8BAPLJzg0/OYrYgwnzW9l7E5EtEDcEjgu0jxDD3XwiuWCpRpDrTZZB+xiCNhTFL84PU/OpNlQcND4qeI1pi2OmA7VcPz94TMsIpproyfMvKxsqgjBCUunn2IpGy6ySSevdoi2YXh0w4j48fXXhZBV10+OvdE3+ZqrV/f3Yv8gjjMdJqcybwx0zkqRo+e0Qp9XrnG6A1LBl+aprJ716JO9ZOWEIRGEu97M7TNsDMvAi/QtE5HUFb+bwBIdssGKdFLLlATOP2vlgIce7fVVaj1CeUQ";
            //txt_en = this.AesDecrypt(txt_en, "2C4ED1CC9BAA42A9A86297C026894154");
            //Response.Write("解密："+txt_en);
        }
    }
}