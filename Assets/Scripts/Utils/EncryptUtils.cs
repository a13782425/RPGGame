using System;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
//
//                  _ooOoo_
//                 o8888888o
//                 88' . '88
//                 (| -_- |)
//                 O\  =  /O
//              ____/`---'\____
//            .'  \\|     |//  `.
//          /  \\|||  :  |||//  \
//          /  _||||| -:- |||||-  \
//          |   | \\\  -  /// |   |
//          | \_|  ''\---/''  |   |
//          \  .-\__  `-`  ___/-. /
//        ___`. .'  /--.--\  `. . __
//     .'' '<  `.___\_<|>_/___.'  >'''.
//    | | :  `- \`.;`\ _ /`;.`/ - ` : | |
//    \  \ `-.   \_ __\ /__ _/   .-` /  /
//=====`-.____`-.___\_____/___.-`____.-'======
//                  `=---='
//
//^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
//          佛祖保佑       永无Bug
//          快加工资       不改需求
//

namespace RPGGame.Utils
{
    public sealed class EncryptUtils : Singleton<EncryptUtils>
    {
        #region 256加密
        //private Random ran = new Random();
        //char[] dicChar = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };


        //加密和解密采用相同的key,可以任意数字，但是必须为32位
        //private string strkeyValue = '12345678901234567890198915689039';

        private static string strkeyValue = "abcdefghijklmnopqrstuvwxyzzyxwvu";

        public const string GlobalEncryptKey = "abcdefghijklmnopqrstuvwxyzzyxwvu";

        private static string _archivedEncryptKey = null;
        public static string ArchivedEncryptKey 
        {
            get
            {
                if (string.IsNullOrEmpty(_archivedEncryptKey))
                {
                    _archivedEncryptKey = SystemInfo.deviceUniqueIdentifier;

                    while (_archivedEncryptKey.Length < strkeyValue.Length)
                    {
                        _archivedEncryptKey += _archivedEncryptKey;
                    }
                    _archivedEncryptKey = _archivedEncryptKey.Substring(0, strkeyValue.Length);
                }
                return _archivedEncryptKey;
            }
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="inputText">需要加密的字段</param>
        /// <returns></returns>
        public string Encryption(string inputText, string encryptKey)
        {
            return encryptionContent(inputText, encryptKey);
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="inputText">需要解密的字段</param>
        /// <returns></returns>
        public string Decipher(string inputText, string encryptKey)
        {
            return decipheringContent(inputText, encryptKey);
        }

        /// <summary>
        /// 内容加密
        /// </summary>
        /// <param name='ContentInfo'>要加密内容</param>
        /// <param name='strkey'>key值</param>
        /// <returns></returns>
        private string encryptionContent(string ContentInfo, string strkey)
        {

            byte[] keyArray = UTF8Encoding.UTF8.GetBytes(strkey);
            byte[] resultArray;
            using (RijndaelManaged encryption = new RijndaelManaged())
            {
                encryption.Key = keyArray;

                //count = encryption.KeySize;

                encryption.Mode = CipherMode.ECB;

                encryption.Padding = PaddingMode.PKCS7;

                ICryptoTransform cTransform = encryption.CreateEncryptor();

                byte[] _EncryptArray = UTF8Encoding.UTF8.GetBytes(ContentInfo);

                resultArray = cTransform.TransformFinalBlock(_EncryptArray, 0, _EncryptArray.Length);
            }

            return Convert.ToBase64String(resultArray, 0, resultArray.Length);

        }

        /// <summary>
        /// 内容解密
        /// </summary>
        /// <param name='encryptionContent'>被加密内容</param>
        /// <param name='strkey'>key值</param>
        /// <returns></returns>
        private string decipheringContent(string encryptionContent, string strkey)
        {

            try
            {
                byte[] keyArray = UTF8Encoding.UTF8.GetBytes(strkey);
                byte[] resultArray;
                using (RijndaelManaged decipher = new RijndaelManaged())
                {
                    decipher.Key = keyArray;

                    //count = decipher.KeySize;

                    decipher.Mode = CipherMode.ECB;

                    decipher.Padding = PaddingMode.PKCS7;

                    ICryptoTransform cTransform = decipher.CreateDecryptor();

                    byte[] _EncryptArray = Convert.FromBase64String(encryptionContent);//UTF8Encoding.UTF8.GetBytes(encryptionContent);// 

                    resultArray = cTransform.TransformFinalBlock(_EncryptArray, 0, _EncryptArray.Length);
                }

                return UTF8Encoding.UTF8.GetString(resultArray);
            }
            catch (Exception ex)
            {
                Debug.LogError(ex.Message);
                return null;
            }
        }
        #endregion

        #region MD5加密
        /// <summary>
        /// MD5加密,和动网上的16/32位MD5加密结果相同
        /// </summary>
        /// <param name="strSource">待加密字串</param>
        /// <param name="encryptType">16或32值之一,其它则采用.net默认MD5加密算法</param>
        /// <returns>加密后的字串</returns>
        public string MD5Encrypt(string strSource, MD5EncryptEnum encryptEnum = MD5EncryptEnum.Default)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(strSource);
            byte[] hashValue =
                ((System.Security.Cryptography.HashAlgorithm)
                    System.Security.Cryptography.CryptoConfig.CreateFromName("MD5")).ComputeHash(bytes);
            StringBuilder sb = new StringBuilder();
            switch (encryptEnum)
            {
                case MD5EncryptEnum.Short:
                    for (int i = 4; i < 12; i++)
                        sb.Append(hashValue[i].ToString("x2"));
                    break;
                case MD5EncryptEnum.Long:
                    for (int i = 0; i < 16; i++)
                    {
                        sb.Append(hashValue[i].ToString("x2"));
                    }
                    break;
                default:
                    for (int i = 0; i < hashValue.Length; i++)
                    {
                        sb.Append(hashValue[i].ToString("x2"));
                    }
                    break;
            }
            return sb.ToString();
        }
        #endregion

    }

    /// <summary>
    /// MD5加密模式
    /// </summary>
    public enum MD5EncryptEnum
    {
        /// <summary>
        /// 16位
        /// </summary>
        Short,
        /// <summary>
        /// 32位
        /// </summary>
        Long,
        /// <summary>
        /// c#默认
        /// </summary>
        Default
    }

}

