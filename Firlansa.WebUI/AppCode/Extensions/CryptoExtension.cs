using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Firlansa.WebUI.AppCode.Extensions
{
    public static partial class CryptoExtension
    {
        private static string ToMd5(string text)
        {
            byte[] textBuffer = Encoding.UTF8.GetBytes($"php{text}c#20@2#@!");

            var md5Provider = MD5.Create();
            //var md5Provider = new MD5CryptoServiceProvider();

            byte[] cryptoBuffer = md5Provider.ComputeHash(textBuffer);

            string buffText = "";

            foreach (var item in cryptoBuffer)
            {
                buffText += item.ToString("x2");
            }

            return buffText;
        }
        public static string Encrypt(this string text, string key)
        {
            try
            {
                byte[] textBuffer = Encoding.UTF8.GetBytes(text);


                using (var ms = new MemoryStream())
                using (var tdes = new TripleDESCryptoServiceProvider())
                {
                    tdes.Mode = CipherMode.ECB;
                    tdes.Padding = PaddingMode.PKCS7;

                    var md5 = MD5.Create();
                    var keyBuffer = md5.ComputeHash(Encoding.UTF8.GetBytes($"_keycode{key}"));
                    var ivBuffer = md5.ComputeHash(Encoding.UTF8.GetBytes($"_keycode{key}"));


                    var translator = tdes.CreateEncryptor(keyBuffer, ivBuffer);

                    using (CryptoStream cryptoStream = new CryptoStream(ms, translator, CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(textBuffer);
                        cryptoStream.FlushFinalBlock();
                        ms.Position = 0;

                        var result = new byte[ms.Length];

                        ms.Read(result, 0, result.Length);

                        return Convert.ToBase64String(result);

                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
           
        }
        public static string Encrypt(this string text)
        {
            return Encrypt(text,"academy202@cSharp---#@!");
        }
        public static string Decrypt(this string text, string key)
        {
            if (string.IsNullOrWhiteSpace(text))
                text = text.Replace("+", " ");
            try
            {
                using (var ms = new MemoryStream())
                using (var tdes = new TripleDESCryptoServiceProvider())
                {

                    var md5 = MD5.Create();
                    var keyBuffer = md5.ComputeHash(Encoding.UTF8.GetBytes($"_keycode{key}"));
                    var ivBuffer = md5.ComputeHash(Encoding.UTF8.GetBytes($"_keycode{key}"));

                    tdes.Mode = CipherMode.ECB;
                    tdes.Padding = PaddingMode.PKCS7;

                    byte[] textBuffer = Convert.FromBase64String(text);


                    var translator = tdes.CreateDecryptor(keyBuffer, ivBuffer);

                    using (CryptoStream cryptoStream = new CryptoStream(ms, translator, CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(textBuffer);
                        cryptoStream.FlushFinalBlock();

                        ms.Position = 0;

                        var result = new byte[ms.Length];

                        ms.Read(result, 0, result.Length);

                        return Encoding.UTF8.GetString(result);

                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static string Decrypt(this string text)
        {
            return Decrypt(text, "academy202@cSharp---#@!");
        }
    }
}
